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

        public void DownloadLevel(string levelCode)
        {
            OnLevelRefresh?.Invoke();
            throw new NotImplementedException();
        }

        public void UploadLevel(string code)
        {
            throw new NotImplementedException();

        }

        public void SubmitLevelScore(string code, string time)
        {
            throw new NotImplementedException();

        }

        public void GetLevelScores(string code)
        {
            throw new NotImplementedException();
        }
    }
}
