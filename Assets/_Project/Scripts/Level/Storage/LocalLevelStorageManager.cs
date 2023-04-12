using System.IO;
using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "Local Level Storage Manager", menuName = "Managers/Storage/new Local Level Storage Manager")]
    public class LocalLevelStorageManager : LevelStorageManager
    {
        [field: SerializeField, Header("Path")] private string LocalDirectoryName { get; set; } = "levels";


        [field: SerializeField, Header("Debug")] private bool UseDebugCode { get; set; }
        [field: SerializeField] private string DebugCode { get; set; } = "00001";

        public string LocalRootDirectory { get; private set; }


        private void OnEnable()
        {
            this.LocalRootDirectory = Application.persistentDataPath + $"/{this.LocalDirectoryName}/";
        }

        private string GetLevelPath(string code)
        {
            return this.LocalRootDirectory + $"{code}/";
        }

        private string GetCreateLevelPath(string code)
        {
            string levelDirectory = this.GetLevelPath(code);

            // Create Directory if not exists
            Directory.CreateDirectory(levelDirectory);

            return levelDirectory;
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
            string code; // = index.ToString("00000");

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
    }

    //public class LevelManagerSystem : SystemComponent<LevelManager>
    //{

    //}
}
