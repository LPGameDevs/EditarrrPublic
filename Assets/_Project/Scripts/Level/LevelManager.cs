using System.Collections.Generic;
using Editarrr.Audio;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using Level.Storage;
using Singletons;
using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "Level Manager", menuName = "Managers/Level/new Level Manager")]
    public class LevelManager : ManagerComponent
    {
        #region Properties

        private const string Documentation = "The level manager is a wrapper around level storage and creation.\r\n" +
                                             "It chooses a storage manager and delegates create / load / save calls.";

        [field: SerializeField, Info(Documentation), Header("Settings")]
        private EditorLevelSettings Settings { get; set; }

        [field: SerializeField, Header("Storage")]
        public LevelStorageManager LevelStorage { get; private set; }

        [field: SerializeField] public RemoteLevelStorageManager RemoteLevelStorage { get; private set; }

        LevelManager_LevelLoadedCallback LevelLoadedCallback { get; set; }
        LevelManager_LevelUploadedCallback LevelUploadedCallback { get; set; }
        LevelManager_AllLevelsLoadedCallback LevelsLoadedCallback { get; set; }

        [SerializeField] private bool RemoteStorageEnabled = false;
        [SerializeField] private bool UploadsAllowed = false;
        public static bool DistributionStorageEnabled = true;

        #endregion

        float _saveSoundBufferTime;
        const float _saveSoundBufferDuration = 0.2f;

        public override void DoAwake()
        {
            LevelStorage.Initialize();

            if (RemoteStorageEnabled || UploadsAllowed)
            {
                RemoteLevelStorage.Initialize();
            }
        }

        #region CRUD Operations

        public LevelState Create()
        {
            LevelState levelState = new LevelState(this.Settings.EditorLevelScaleX, this.Settings.EditorLevelScaleY);

            string code = this.LevelStorage.GetUniqueCode();

            levelState.SetCode(code);

            string userId = PreferencesManager.Instance.GetUserId();
            string userName = PreferencesManager.Instance.GetUserName();

            levelState.SetCreator(userId, userName);

            // We are creating a save file and stub so it can be loaded again later.
            LevelSave levelSave = levelState.CreateSave();
            this.Save(levelSave, false);

            return levelState;
        }

        private string GetLocalLevelPath(string code)
        {
            return this.LevelStorage.GetLevelPath(code);
        }

        public void Load(string code, LevelManager_LevelLoadedCallback loadedCallback)
        {
            this.LevelLoadedCallback = loadedCallback;

            this.LevelStorage.LoadLevelData(code, LevelStorage_LevelLoadedCallback);

            // @note I dont normally like nested callbacks but this cleans up the class a bit.
            void LevelStorage_LevelLoadedCallback(LevelSave levelSave)
            {
                if (levelSave == null)
                {
#warning // TODO, Failed to load... >> Load default level, (Display Message???)
                    this.Create();
                    return;
                }

                LevelState levelState = new LevelState(levelSave);

                this.LevelLoadedCallback?.Invoke(levelState);
                this.LevelLoadedCallback = null;
            }
        }

        public void LoadAll(LevelManager_AllLevelsLoadedCallback loadedCallback, RemoteLevelLoadQuery? query = null)
        {
            this.LevelsLoadedCallback = loadedCallback;

            if (RemoteStorageEnabled)
            {
                this.RemoteLevelStorage.LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback, query);
            }
            else
            {
                this.LevelStorage.LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback);
            }

            void LevelStorage_AllLevelsLoadedCallback(LevelStub[] levelStubs, string cursor = "")
            {
                if (levelStubs == null)
                {
                    return;
                }

                List<LevelStub> levels = new List<LevelStub>();

                foreach (var levelStub in levelStubs)
                {
                    levels.Add(levelStub);
                }

                this.LevelsLoadedCallback?.Invoke(levels.ToArray(), cursor);
                this.LevelsLoadedCallback = null;
            }

        }

        public void Delete(string code)
        {
            this.LevelStorage.Delete(code);
        }

        /**
         * This save function loads and updates an existing levelSave.
         *
         * This doesnt work for creation new levelSaves.
         */
        public void SaveState(LevelState levelState, bool uploadToRemote = true)
        {
            this.LevelStorage.LoadLevelData(levelState.Code, SaveLevelAfterLoad);

            void SaveLevelAfterLoad(LevelSave levelSave)
            {
                // @todo what else can have changed in the state?
                levelSave.SetTiles(levelState.Tiles);
                this.Save(levelSave, uploadToRemote);
            }
        }

        public void SaveScreenshot(string code, Texture2D screenshot)
        {
                byte[] byteArray = screenshot.EncodeToPNG();
                this.LevelStorage.SaveScreenshot(code, byteArray);
        }

        public void MarkLevelAsComplete(LevelSave levelSave)
        {
            levelSave.MarkAsCompleted();

            // Store state to filesystem.
            string data = JsonUtility.ToJson(levelSave);
            this.LevelStorage.Save(levelSave.Code, data);
        }

        private void Save(LevelSave levelSave, bool uploadToRemote = false)
        {
            // Increment version string.
            levelSave.SetVersion(levelSave.Version + 1);

            // Store state to filesystem.
            string data = JsonUtility.ToJson(levelSave);
            this.LevelStorage.Save(levelSave.Code, data);

            if (uploadToRemote && this.UploadsAllowed)
            {
                string path = this.GetLocalLevelPath(levelSave.Code);
                levelSave.SetLocalDirectory(path);
                this.RemoteLevelStorage.Upload(levelSave, UploadCompleted);
            }

            AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.LevelSave, $"{levelSave.Code}-{levelSave.Version}");

            void UploadCompleted(string code, string remoteId, bool isSteam = false)
            {
                if (isSteam)
                {
                    ulong steamId = ulong.Parse(remoteId);
                    this.SetSteamUploadId(code, steamId);
                }
                else
                {
                    this.SetRemoteUploadId(code, remoteId);
                }

                this.LevelUploadedCallback?.Invoke(levelSave);
                this.LevelUploadedCallback = null;
            }

            if(Time.time > _saveSoundBufferTime)
            {
                AudioManager.Instance.PlayAudioClip(AudioManager.LEVEL_SAVED_CLIP_NAME);
                _saveSoundBufferTime = Time.time + _saveSoundBufferDuration;
            }

        }

        private void SetRemoteUploadId(string code, string remoteId)
        {
            LevelStorage.LoadLevelData(code, DoSetRemoteUploadId);

            void DoSetRemoteUploadId(LevelSave levelSave)
            {
                levelSave.SetRemoteId(remoteId);
                string data = JsonUtility.ToJson(levelSave);
                this.LevelStorage.Save(levelSave.Code, data);
            }
        }

        private void SetSteamUploadId(string code, ulong steamId)
        {
            LevelStorage.LoadLevelData(code, DoSetSteamUploadId);

            void DoSetSteamUploadId(LevelSave levelSave)
            {
                levelSave.SetSteamId(steamId);
                string data = JsonUtility.ToJson(levelSave);
                this.LevelStorage.Save(levelSave.Code, data);
            }
        }

        #endregion

        #region Remote Operations

        public void PublishAndUpload(string code, LevelManager_LevelUploadedCallback callback)
        {
            if (!UploadsAllowed)
            {
                Debug.LogError("Remote operations are not enabled for this LevelManager.");
                return;
            }

            this.LevelUploadedCallback = callback;
            LevelStorage.LoadLevelData(code, DoPublishAndUpload);

            void DoPublishAndUpload(LevelSave levelSave)
            {
                levelSave.SetPublished(true);
                this.Save(levelSave, true);
            }
        }

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            if (!RemoteStorageEnabled)
            {
                Debug.LogError("Remote operations are not enabled for this LevelManager.");
                return;
            }
            RemoteLevelStorage.Download(code, callback);
        }

        public void DownloadScreenshot(string code, RemoteLevelStorage_LevelScreenshotDownloadedCallback callback)
        {
            if (!RemoteStorageEnabled)
            {
                Debug.LogError("Remote operations are not enabled for this LevelManager.");
                return;
            }
            RemoteLevelStorage.DownloadScreenshot(code, callback);
        }

        public void SaveDownloadedLevel(LevelSave levelSave)
        {
            this.Save(levelSave, false);
        }

#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
        public void CopyLevelFromSteamDirectory(Steamworks.Ugc.Item item)
        {
            string code = item.Title;
            string sourceDirectory = item.Directory;
            this.LevelStorage.CopyLevelFromDirectory(code, sourceDirectory);
        }
#endif

        public void SubmitScore(float score, LevelSave levelSave, RemoteScoreStorage_ScoreSubmittedCallback callback)
        {
            if (!RemoteStorageEnabled)
            {
                Debug.LogError("Remote operations are not enabled for this LevelManager.");
                return;
            }
            RemoteLevelStorage.SubmitScore(score, levelSave, callback);
        }

        public void GetScoresForLevel(string code, RemoteScoreStorage_AllScoresLoadedCallback callback)
        {
            if (!RemoteStorageEnabled)
            {
                // Debug.LogError("Remote operations are not enabled for this LevelManager.");
                // return;
            }
            RemoteLevelStorage.GetScoresForLevel(code, callback);
        }

        public void SubmitRating(int rating, LevelSave levelSave, RemoteRatingStorage_RatingSubmittedCallback callback)
        {
            RemoteLevelStorage.SubmitRating(rating, levelSave, callback);
        }

        public void GetRatingsForLevel(string code, RemoteRatingStorage_AllRatingsLoadedCallback callback)
        {
            RemoteLevelStorage.GetRatingsForLevel(code, callback);
        }

        #endregion

        public string GetScreenshotPath(string levelCode, bool isDistro = false)
        {
            return this.LevelStorage.GetScreenshotPath(levelCode, isDistro);
        }

        public bool LevelExists(string levelCode)
        {
            if (levelCode.Length == 0)
            {
                return false;
            }
            return this.LevelStorage.LevelExists(levelCode);
        }
    }

    public delegate void LevelManager_LevelLoadedCallback(LevelState levelState);
    public delegate void LevelManager_LevelUploadedCallback(LevelSave levelSave);

    public delegate void LevelManager_AllLevelsLoadedCallback(LevelStub[] levelStates, string cursor = "");

    public struct RemoteLevelLoadQuery
    {
        public int limit;
        public string cursor;
    }
}
