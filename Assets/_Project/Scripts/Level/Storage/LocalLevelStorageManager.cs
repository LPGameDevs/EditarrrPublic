using System.Collections;
using System.Collections.Generic;
using System.IO;
using Editarrr.Misc;
using Level.Storage;
using Proyecto26;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "Local Level Storage Manager", menuName = "Managers/Storage/new Local Level Storage Manager")]
    public class LocalLevelStorageManager : LevelStorageManager
    {
        [field: SerializeField,
            Info("The LocalLevelStorageManager class is a C# subclass that manages the storage of levels locally on a device. It has properties that can be serialized for debug purposes, and methods for saving and loading levels in the device's persistent storage. It saves level data as JSON files, creates a directory for each level, and saves level screenshots in PNG format. If a level directory or level JSON file is not found, the class returns null."),
            Header("Path")]
        private static string LocalDirectoryName { get; set; } = "levels";


        [field: SerializeField, Header("Debug")] private bool UseDebugCode { get; set; }
        [field: SerializeField] private string DebugCode { get; set; } = "00001";

        public static string LocalRootDirectory => Application.persistentDataPath + $"/{LocalDirectoryName}/";
        public string DistroRootDirectory => Application.streamingAssetsPath + "/levels/";


        public override void Initialize()
        {
            if (!Directory.Exists(LocalRootDirectory))
            {
                Directory.CreateDirectory(LocalRootDirectory);
            }
        }

        public override string GetLevelPath(string code)
        {
            return LocalRootDirectory + $"{code}/";
        }

        private string GetDistroLevelPath(string code)
        {
            return DistroRootDirectory + $"{code}/";
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

        public override string GetScreenshotPath(string code, bool isDistro = false)
        {
            string path = "";
            if (isDistro)
            {
                path = this.GetDistroLevelPath(code) + "screenshot.png";
            }
            else
            {
                path = this.GetCreateLevelPath(code) + "screenshot.png";
            }

            return path;
        }

        public override void CopyLevelFromDirectory(string code, string sourceDirectory)
        {
            string destinationDirectory = this.GetCreateLevelPath(code);

            // copy all files from source to destination
            foreach (string file in Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(file);
                string dest = Path.Combine(destinationDirectory, fileName);
                File.Copy(file, dest, true);
            }
        }

        public override void Save(string code, string data, bool isDistro = false)
        {
            code = code.ToLower();
            string path = "";

            if (isDistro)
            {
                path = this.GetDistroLevelPath(code) + "level.json";
            }
            else
            {
                path = this.GetCreateLevelPath(code) + "level.json";
            }

            // Write to local storage
            File.WriteAllText(path, data);
        }

        public override void SaveScreenshot(string code, byte[] screenshot)
        {
            string path = this.GetCreateLevelPath(code) + "screenshot.png";

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
            int index = Random.Range(2, 99999);
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

        public override bool LevelExists(string code)
        {
            string path = this.GetLevelPath(code);

            if (!Directory.Exists(path))
            {
                return false;
            }

            string levelFilePath = path + "level.json";
            if (!File.Exists(levelFilePath))
            {
                return false;
            }

            return true;
        }

        public override void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback)
        {
            string path = this.GetLevelPath(code);

            if (!Directory.Exists(path))
            {
                path = this.GetDistroLevelPath(code);

                if (!Directory.Exists(path))
                {
                    // No level Directory found, return null!
                    callback?.Invoke(null);
                    return;
                }
            }

            string levelFilePath = path + "level.json";
            if (!File.Exists(levelFilePath))
            {
                // Fallback just in case.
                path = this.GetDistroLevelPath(code);
                levelFilePath = path + "level.json";

                if (!File.Exists(levelFilePath))
                {
                    // No level found, return null!
                    callback?.Invoke(null);
                    return;
                }
            }

            string data = File.ReadAllText(levelFilePath);
            LevelSave levelSave = JsonUtility.FromJson<LevelSave>(data);

            callback?.Invoke(levelSave);
        }

        public override void LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback callback)
        {
            DirectoryInfo dir = new DirectoryInfo(LocalRootDirectory);

            List<LevelStub> levelsStubs = new List<LevelStub>();

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

                LevelStub stub = new LevelStub(levelSave.Code, levelSave.Creator, levelSave.CreatorName, levelSave.RemoteId ?? "", levelSave.Published);

                foreach (var label in levelSave.GetLabels())
                {
                    stub.SetLabel(label);
                }

                stub.SetTotalRatings(levelSave.TotalRatings);
                stub.SetTotalScores(levelSave.TotalScores);

                levelsStubs.Add(stub);
            }

            if (LevelManager.DistributionStorageEnabled)
            {
#if UNITY_WEBGL
                // @todo Distros are currently not supported for webgl.
                if (false)
                {
                    Debug.Log("levels enabled");

                    dir = new DirectoryInfo(DistroRootDirectory);

                    Debug.Log(DistroRootDirectory);
                    Debug.Log(dir.Name);

                    string[] demos = new[]
                    {
                        "demo1",
                        "demo2",
                        "demo3",
                        "demo4",
                        "demo5",
                    };

                    foreach (string demo in demos)
                    {
                        Debug.Log(demo);
                        string levelFilePath =
                            Path.Combine(Application.streamingAssetsPath, "levels", demo, "level.json");
                        Debug.Log(levelFilePath);

                        // if (!File.Exists(levelFilePath))
                        // {
                        // continue;
                        // }
                        GetDistroLevelFromUrl(levelFilePath);
                    }
                }
#else
                dir = new DirectoryInfo(DistroRootDirectory);

                foreach (DirectoryInfo levelDirectory in dir.GetDirectories())
                {
                    string path = this.GetDistroLevelPath(levelDirectory.Name);
                    string levelFilePath = path + "level.json";
                    if (!File.Exists(levelFilePath))
                    {
                        continue;
                    }
                    string data = File.ReadAllText(levelFilePath);
                    LevelSave levelSave = JsonUtility.FromJson<LevelSave>(data);

                    LevelStub stub = new LevelStub(levelSave.Code, levelSave.Creator, levelSave.CreatorName, levelSave.RemoteId ?? "", levelSave.Published);
                    stub.SetDistro(true);

                    foreach (var label in levelSave.GetLabels())
                    {
                        stub.SetLabel(label);
                    }

                    stub.SetTotalRatings(levelSave.TotalRatings);
                    stub.SetTotalScores(levelSave.TotalScores);

                    levelsStubs.Add(stub);
                }
#endif
            }

            callback?.Invoke(levelsStubs.ToArray());
        }

        private async void GetDistroLevelFromUrl(string uri)
        {
            RestClient.Get<AwsLevel>(uri).Then(res =>
            {
                // @todo make sure we store the remote level id in the save.
                var save = new LevelSave(res.creator.id, res.creator.name, res.name);

                save.SetRemoteId(res.id);

                var tiles = JsonUtility.FromJson<AwsTileData>(res.data.tiles);
                var tileStates = new TileState[res.data.scaleX, res.data.scaleY];

                foreach (var tileSave in tiles.tiles)
                {
                    tileStates[tileSave.X, tileSave.Y] = new TileState(tileSave);
                }

                save.SetTiles(tileStates);
                save.SetTotalRatings(res.totalRatings);
                save.SetTotalScores(res.totalScores);
                save.SetVersion(res.version);

                foreach (var label in res.labels)
                {
                    save.SetLabel(label);
                }
                save.SetPublished(res.status == "PUBLISHED");
                // callback?.Invoke(save);

                // @todo return level data.
                Debug.Log(JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                Debug.LogError(err.Message);
            });
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
