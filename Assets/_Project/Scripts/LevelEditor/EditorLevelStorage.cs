using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelEditor
{
    public class EditorLevelStorage : UnitySingleton<EditorLevelStorage>
    {
        // Trigger this event when the level selection should be updated.
        public static event Action OnLevelRefresh;

        public static Action<DatabaseRequestType, PostRequestData> OnRequestComplete;
        public static Action<DatabaseRequestType, CommentsResponseData> OnCommentsRequestComplete;

        public static bool _remoteStorageEnabled = false;
        private bool _remoteStorageDrupal = false;
        private bool _includeDistroLevels = true;
        private int _localStorageCode;

        private IDbConnector _db;

        private string _SaveLevelScreenshotPath;
        private string _SaveLevelCode;
        private string _SaveLevelData;

        public static string DistroLevelStoragePath => Application.streamingAssetsPath + "/levels/";
        public static string ScreenshotStoragePath => Application.persistentDataPath + "/screenshots/";
        public static string LevelStoragePath => Application.persistentDataPath + "/levels/";
        public static string LevelStorageEditorLevel => Application.persistentDataPath + "/levels/level.json";


        public string DefaultLevelCode;


        protected override void Awake()
        {
            base.Awake();
            _localStorageCode = PlayerPrefs.GetInt("LocalStorageCode", 0);
            CheckLevelDirectoriesExists();
            CheckEditorLevelExists();

            if (!_remoteStorageEnabled)
            {
                return;
            }

            if (_remoteStorageDrupal)
            {
                _db = GetComponent<DrupalConnector>();
            }
            else
            {
                _db = GetComponent<DatabaseConnector>();
            }
        }

        public string[] GetStoredLevelFiles()
        {
            CheckLevelDirectoriesExists();
            DirectoryInfo dir = new DirectoryInfo(LevelStoragePath);

            List<string> info = new List<string>();

            // Get all directories in dir.
            foreach (DirectoryInfo directory in dir.GetDirectories())
            {
                FileInfo[] files = directory.GetFiles("*.json");
                if (files.Length == 0)
                {
                    continue;
                }

                info.Add(directory.Name);
            }

            // We may want to package a few levels with the game
            // so that the player can play without having to download
            // anything from the server.
            if (_includeDistroLevels)
            {
                DirectoryInfo dir2 = new DirectoryInfo(DistroLevelStoragePath);

                foreach (DirectoryInfo directory in dir2.GetDirectories())
                {
                    FileInfo[] files = directory.GetFiles("*.json");
                    if (files.Length == 0)
                    {
                        continue;
                    }
                    info.Add(directory.Name);
                }
            }

            return info.ToArray();
        }

        public LevelSave GetLevelData(string levelCode)
        {
            if (levelCode.Length > 0)
            {
                string data = "";
                if (File.Exists(LevelStoragePath + levelCode + ".json"))
                {
                    data = File.ReadAllText(LevelStoragePath + levelCode + ".json");
                }
                else if (File.Exists(DistroLevelStoragePath + levelCode + ".json"))
                {
                    data = File.ReadAllText(DistroLevelStoragePath + levelCode + ".json");
                }

                if (data.Length > 2)
                {
                    return JsonUtility.FromJson<LevelSave>(data);
                }
            }

            return new LevelSave();
        }

        public void DownloadLevel(string levelCode)
        {
            if (levelCode.Length == 0)
            {
                levelCode = DefaultLevelCode;
            }

            if (_remoteStorageEnabled)
            {
                _db.GetData(levelCode);
            }
        }

        public void DeleteLevel(string code)
        {
            if (code.Length == 0)
            {
                return;
            }

            // @todo rewrite this to loop through files in directory and delete.
            File.Delete(LevelStoragePath + code + "/level.json");
            File.Delete(LevelStoragePath + code + "/screenshot.png");
            Directory.Delete(LevelStoragePath + code);

            OnLevelRefresh?.Invoke();
        }

        public void UploadLevel(string code)
        {
            if (code.Length == 0)
            {
                return;
            }

            LevelSave data = GetLevelData(code);
            data.published = true;
            string dataString = JsonUtility.ToJson(data);

            SaveLevel(code, dataString, true);
            OnLevelRefresh?.Invoke();
        }

        public void SaveLevel(string code = "", string levelData = "", bool upload = false)
        {
            bool levelExists = code.Length > 0;
            if (levelData.Length == 0)
            {
                levelData = File.ReadAllText(LevelStorageEditorLevel);
            }

            if (!levelExists)
            {
                if (_remoteStorageEnabled)
                {
                    code = _db.GetUniqueCode();
                }
                else
                {
                    _localStorageCode++;
                    PlayerPrefs.SetInt("LocalStorageCode", _localStorageCode);
                    code = _localStorageCode.ToString("00000");
                }
            }

            _SaveLevelScreenshotPath = $"{ScreenshotStoragePath}{code}.png";
            _SaveLevelCode = code;
            _SaveLevelData = levelData;

            if (upload)
            {
                if (!levelExists)
                {
                    StartCoroutine(nameof(DoSaveLevelCreate));
                }
                else
                {
                    StartCoroutine(nameof(DoSaveLevelUpdate));
                }
            }

            File.WriteAllText(LevelStoragePath + code + ".json", levelData);
        }

        public void SubmitLevelScore(string code, string time)
        {
            string ghostFile = Path.Combine(Application.persistentDataPath, "ghosts", code + ".json");
            if (File.Exists(ghostFile))
            {
                string data = File.ReadAllText(ghostFile);
                _db.CreateComment(code, time, data);
                return;
            }

            _db.CreateComment(code, time);
        }

        public void GetLevelScores(string code)
        {
            _db.GetComments(code);
        }

        public IEnumerator DoSaveLevelCreate()
        {
            yield return DoTakeScreenshot();
            if (_remoteStorageEnabled)
            {
                _db.CreateData(_SaveLevelCode, _SaveLevelData);
            }
        }

        public IEnumerator DoSaveLevelUpdate()
        {
            LevelSave save = JsonUtility.FromJson<LevelSave>(_SaveLevelData);
            if (!save.published)
            {
                yield return DoTakeScreenshot();
            }

            if (_remoteStorageEnabled)
            {
                _db.UpdateData(_SaveLevelCode, _SaveLevelData);
            }
        }

        public IEnumerator DoTakeScreenshot()
        {
            // Wait till the last possible moment before screen rendering to hide the UI
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            canvas.enabled = false;

            // Wait for screen rendering to complete
            yield return new WaitForEndOfFrame();

            // Take screenshot
            ScreenCapture.CaptureScreenshot(_SaveLevelScreenshotPath);

            // Show UI after we're done
            canvas.enabled = true;
        }

        private void CheckLevelDirectoriesExists()
        {
            CheckLevelDirectoryExists(DistroLevelStoragePath);
            CheckLevelDirectoryExists(LevelStoragePath);
            CheckLevelDirectoryExists(ScreenshotStoragePath);
        }

        private void CheckLevelDirectoryExists(string path)
        {
            var directoryInfo = new FileInfo(path).Directory;
            if (directoryInfo != null)
            {
                directoryInfo.Create();
            }
        }

        private void CheckEditorLevelExists()
        {
            if (!File.Exists(LevelStorageEditorLevel))
            {
                File.WriteAllText(LevelStorageEditorLevel, "{}");
            }
        }

        private void OnEnable()
        {
            OnRequestComplete += DatabaseRequestComplete;
        }

        private void OnDisable()
        {
            OnRequestComplete -= DatabaseRequestComplete;
        }


        private void DatabaseRequestComplete(DatabaseRequestType method, PostRequestData result)
        {
            if (method == DatabaseRequestType.FileDownload)
            {
                OnLevelRefresh?.Invoke();
                return;
            }

            if (method != DatabaseRequestType.GetData)
            {
                return;
            }

            if (result.data == null || result.data.Length < 3)
            {
                return;
            }

            SaveLevel(result.code, result.data);
            OnLevelRefresh?.Invoke();
        }
    }

    [Serializable]
    public class PostRequestData
    {
        public string method;
        public string user;
        public string code;
        public string data;
    }

    [Serializable]
    public class CommentsResponseData
    {
        public CommentResponseData[] comments;
    }

    [Serializable]
    public class CommentResponseData
    {
        public string time;
        public string user;

        public CommentResponseData(string user, string time)
        {
            this.user = user;
            this.time = time;
        }
    }

    public enum DatabaseRequestType
    {
        GetData,
        InsertData,
        UpdateData,
        InsertComment,
        GetLevelComments,
        FileDownload,
    }
}
