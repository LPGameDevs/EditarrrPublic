using System.Collections.Generic;
using System.IO;
using Editarrr.Misc;
using Level.Storage;
using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "Local Level Storage Manager", menuName = "Managers/Storage/new Local Level Storage Manager")]
    public class LocalLevelStorageManager : LevelStorageManager
    {
        [field: SerializeField,
            Info("The LocalLevelStorageManager class is a C# subclass that manages the storage of levels locally on a device. It has properties that can be serialized for debug purposes, and methods for saving and loading levels in the device's persistent storage. It saves level data as JSON files, creates a directory for each level, and saves level screenshots in PNG format. If a level directory or level JSON file is not found, the class returns null."),
            Header("Path")]
        private string LocalDirectoryName { get; set; } = "levels";


        [field: SerializeField, Header("Debug")] private bool UseDebugCode { get; set; }
        [field: SerializeField] private string DebugCode { get; set; } = "00001";

        public string LocalRootDirectory => Application.persistentDataPath + $"/{this.LocalDirectoryName}/";
        public string DistroRootDirectory => Application.streamingAssetsPath + "/levels/";


        public override void Initialize()
        {
            if (!Directory.Exists(LocalRootDirectory))
            {
                Directory.CreateDirectory(LocalRootDirectory);
            }
        }

        private string GetLevelPath(string code)
        {
            return this.LocalRootDirectory + $"{code}/";
        }

        private string GetCreateLevelPath(string code)
        {
            string levelDirectory = this.GetLevelPath(code);

            // Create Directory if not exists
            if (!Directory.Exists(levelDirectory))
            {
                Directory.CreateDirectory(levelDirectory);
            }

            return levelDirectory;
        }

        public override string GetScreenshotPath(string code)
        {
            string path = this.GetCreateLevelPath(code) + "screenshot.png";
            return path;
        }

        public override void Save(string code, string data)
        {
            string path = this.GetCreateLevelPath(code) + "level.json";

            Debug.Log($"Local Save: {path}");

            // Write to local storage
            File.WriteAllText(path, data);
        }

        public override void SaveScreenshot(string code, byte[] screenshot)
        {
            string path = this.GetCreateLevelPath(code) + "screenshot.png";

            Debug.Log($"Local Save Screenshot: {path}");

            // Write to local storage
            File.WriteAllBytes(path, screenshot);
        }

        public override string GetUniqueCode()
        {
            if (this.UseDebugCode)
            {
                return this.DebugCode ?? "00001";
            }

            // Start at 2, 1 is reserved for Debug!
            int index = 2;
            string code;

            do
            {
                code = index.ToString("00000");

                string directory = this.GetLevelPath(code);

                if (!Directory.Exists(directory))
                    break;

                index = Random.Range(2, 99999);
            } while (true);

            return code;
        }

        public override void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback)
        {
            string path = this.GetLevelPath(code);

            if (!Directory.Exists(path))
            {
                // No level Directory found, return null!
                callback?.Invoke(null);
                return;
            }

            string levelFilePath = path + "level.json";
            if (!File.Exists(levelFilePath))
            {
                // No level found, return null!
                callback?.Invoke(null);
                return;
            }

            string data = File.ReadAllText(levelFilePath);
            LevelSave levelSave = JsonUtility.FromJson<LevelSave>(data);

            callback?.Invoke(levelSave);
        }

        public override void LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback callback)
        {
            DirectoryInfo dir = new DirectoryInfo(LocalRootDirectory);

            List<LevelSave> levels = new List<LevelSave>();

            // Get all directories in dir.
            foreach (DirectoryInfo levelDirectory in dir.GetDirectories())
            {
                string path = this.GetLevelPath(levelDirectory.Name);
                string levelFilePath = path + "level.json";
                if (!File.Exists(levelFilePath))
                {
                    continue;
                }
                string data = File.ReadAllText(levelFilePath);
                LevelSave levelSave = JsonUtility.FromJson<LevelSave>(data);
                levels.Add(levelSave);
            }

            if (LevelManager.DistributionStorageEnabled)
            {
                // @todo Load distribution levels from
                string path = DistroRootDirectory;
            }

            callback?.Invoke(levels.ToArray());
        }

        public override void Delete(string code)
        {
            string path = this.GetLevelPath(code);

            if (!Directory.Exists(path))
            {
                return;
            }

            // @todo Loop through files and delete everything.
            File.Delete(path + "level.json");
            File.Delete(path + "screenshot.png");
            Directory.Delete(path);
        }
    }
}
