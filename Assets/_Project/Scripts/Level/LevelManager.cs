using System.Collections.Generic;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using Level.Storage;
using Unity.VisualScripting;
using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "Level Manager", menuName = "Managers/Level/new Level Manager")]
    public class LevelManager : ManagerComponent
    {
        #region Properties

        private const string Documentation = "The level manager is a wrapper around level storage and creation.\r\n" +
                                             "It chooses a storage manager and delegates create / load / save calls.";

        [field: SerializeField, Info(Documentation)]
        public LevelState LevelState { get; private set; }

        [field: SerializeField, Header("Settings")]
        private EditorLevelSettings Settings { get; set; }

        [field: SerializeField, Header("Storage")]
        public LevelStorageManager LevelStorage { get; private set; }

        [field: SerializeField] public RemoteLevelStorageManager RemoteLevelStorage { get; private set; }

        LevelManager_LevelLoadedCallback LevelLoadedCallback { get; set; }
        LevelManager_AllLevelsLoadedCallback LevelsLoadedCallback { get; set; }

        [SerializeField] private bool RemoteStorageEnabled = false;
        public static bool DistributionStorageEnabled = false;

        #endregion

        public override void DoAwake()
        {
            LevelStorage.Initialize();

            if (RemoteStorageEnabled)
            {
                RemoteLevelStorage.Initialize();
            }
        }

        #region CRUD Operations

        public void Create()
        {
            this.LevelState = new LevelState(this.Settings.EditorLevelScaleX, this.Settings.EditorLevelScaleY);

            string code = this.LevelStorage.GetUniqueCode();

            this.LevelState.SetCode(code);

            string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);

            this.LevelState.SetCreator(userName);
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

                this.LevelState = new LevelState(levelSave);

                this.LevelLoadedCallback?.Invoke(this.LevelState);
                this.LevelLoadedCallback = null;
            }
        }

        public void LoadAll(LevelManager_AllLevelsLoadedCallback loadedCallback)
        {
            this.LevelsLoadedCallback = loadedCallback;

            if (RemoteStorageEnabled)
            {
                this.RemoteLevelStorage.LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback);
            }
            else
            {
                this.LevelStorage.LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback);
            }

            void LevelStorage_AllLevelsLoadedCallback(LevelStub[] levelStubs)
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

                this.LevelsLoadedCallback?.Invoke(levels.ToArray());
                this.LevelsLoadedCallback = null;
            }

        }

        public void Delete(string code)
        {
            this.LevelStorage.Delete(code);
        }

        public void Save(EditorTileState[,] editorTileData, Texture2D screenshot)
        {
            // Update state with changes.
            this.LevelState.SetTiles(editorTileData);

            // Store state to filesystem.
            LevelSave levelSave = this.LevelState.CreateSave();
            string data = JsonUtility.ToJson(levelSave);
            this.LevelStorage.Save(levelSave.Code, data);

            // @todo Store the state to remote storage.
            if (RemoteStorageEnabled)
            {
                this.RemoteLevelStorage.Upload(levelSave.Code, data);
            }

            // Store screenshot to filesystem.
            SaveScreenshot(levelSave, screenshot);

            void SaveScreenshot(LevelSave levelSave, Texture2D screenshot)
            {
                byte[] byteArray = screenshot.EncodeToPNG();
                this.LevelStorage.SaveScreenshot(levelSave.Code, byteArray);
            }
        }

        #endregion

        #region Remote Operations

        public void Upload(string code, string data)
        {
            if (!RemoteStorageEnabled)
            {
                Debug.LogError("Remote operations are not enabled for this LevelManager.");
                return;
            }
            RemoteLevelStorage.Upload(code, data);
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

        public void SaveDownloadedLevel(LevelSave levelSave)
        {
            string data = JsonUtility.ToJson(levelSave);
            this.LevelStorage.Save(levelSave.Code, data);
        }

        public void SubmitScore()
        {
            if (!RemoteStorageEnabled)
            {
                Debug.LogError("Remote operations are not enabled for this LevelManager.");
                return;
            }
            RemoteLevelStorage.SubmitScore();
        }

        public void Publish()
        {
            if (!RemoteStorageEnabled)
            {
                Debug.LogError("Remote operations are not enabled for this LevelManager.");
                return;
            }

            this.LevelState.SetPublished();
            LevelSave levelSave = this.LevelState.CreateSave();
            string data = JsonUtility.ToJson(levelSave);
            this.LevelStorage.Save(levelSave.Code, data);

            Upload(levelSave.Code, data);
        }

        #endregion

        public string GetScreenshotPath(string levelCode)
        {
            return this.LevelStorage.GetScreenshotPath(levelCode);
        }

        public bool LevelExists(string levelCode)
        {
            return this.LevelStorage.LevelExists(levelCode);
        }
    }

    public delegate void LevelManager_LevelLoadedCallback(LevelState levelState);

    public delegate void LevelManager_AllLevelsLoadedCallback(LevelStub[] levelStates);
}
