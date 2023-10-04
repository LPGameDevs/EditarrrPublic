using System;
using System.Collections.Generic;
using Editarrr.Level;
using Proyecto26;
using UnityEditor;
using UnityEngine;

namespace Level.Storage
{
    public class RemoteLevelStorageProviderAws : IRemoteLevelStorageProvider
    {
        private const string _awsLevelUrl = "https://tlfb41owe5.execute-api.eu-north-1.amazonaws.com";
        private bool _showDebug = false;

        public void Initialize()
        {
            // No initialization needed.
        }

        public void Upload(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            string userId = PlayerPrefs.GetString(UserNameForm.UserIdStorageKey);

            AwsTileData tileData = new AwsTileData()
            {
                tiles = levelSave.Tiles
            };
            AwsLevel request = new AwsLevel
            {
                id = levelSave.RemoteId,
                name = levelSave.Code,
                creator = new AwsCreator()
                {
                    name = levelSave.CreatorName,
                    id = userId
                },
                status = levelSave.Published ? "PUBLISHED" : "DRAFT",
                data = new AwsData()
                {
                    scaleX = levelSave.ScaleX,
                    scaleY = levelSave.ScaleY,
                    tiles = JsonUtility.ToJson(tileData),
                    version = levelSave.Version
                }
            };

            RestClient.Get<AwsLevel>($"{_awsLevelUrl}/dev/levels/{levelSave.RemoteId}").Then(res =>
            {
                Debug.Log("UPLOAD - Existing level found.");

               // Existing level found.
               this.Update(request, callback);
            }).Catch(err =>
            {
                Debug.Log("UPLOAD - No level found");
                // No level found.
                this.Insert(request, callback);
            });
        }

        private void Insert(AwsLevel request, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            RestClient.Post<AwsUploadResponse>($"{_awsLevelUrl}/dev/levels", JsonUtility.ToJson(request)).Then(res =>
            {
                callback?.Invoke(request.name, res.id);
                this.LogMessage("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);
            });
        }

        private void Update(AwsLevel request, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            RestClient.Patch<AwsUploadResponse>($"{_awsLevelUrl}/dev/levels/{request.id}", JsonUtility.ToJson(request)).Then(res =>
            {
                callback?.Invoke(request.name, res.id.ToString());
                this.LogMessage("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);
            });
        }

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            RestClient.Get<AwsLevel>($"{_awsLevelUrl}/dev/levels/{code}").Then(res =>
            {
                // @todo make sure we store the remote level id in the save.
                LevelSave save = new LevelSave(res.creator.id, res.creator.name, res.name);

                save.SetRemoteId(res.id);

                var tiles = JsonUtility.FromJson<AwsTileData>(res.data.tiles);
                TileState[,] tileStates = new TileState[res.data.scaleX, res.data.scaleY];

                foreach (TileSave tileSave in tiles.tiles)
                {
                    tileStates[tileSave.X, tileSave.Y] = new TileState(tileSave);
                }

                save.SetTiles(tileStates);
                save.SetVersion(res.data.version);
                save.SetPublished(res.status == "PUBLISHED");
                callback?.Invoke(save);

                // @todo return level data.
                this.LogMessage(res.id, JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                callback?.Invoke(null);
                this.LogMessage("Error", err.Message);
            });
        }

        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            // Get request to /levels
            RestClient.Get<AwsLevels>($"{_awsLevelUrl}/dev/levels").Then(res =>
            {
                List<LevelStub> levelStubs = new List<LevelStub>();
                foreach (AwsLevel level in res.levels)
                {
                    LevelStub levelStub = new LevelStub(level.name, level.creator.id, level.creator.name, level.id, level.status == "PUBLISHED");
                    levelStubs.Add(levelStub);
                }

                callback?.Invoke(levelStubs.ToArray());
                this.LogMessage("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                callback?.Invoke(null);
                this.LogMessage("Error", err.Message);
            });
        }

        public bool SupportsLeaderboards()
        {
            return false;
        }

        public void SubmitScore()
        {
            throw new NotImplementedException();
        }

        private void LogMessage(string title, string message)
        {
#if UNITY_EDITOR
            if (_showDebug)
            {
                EditorUtility.DisplayDialog(title, message, "Ok");
            }
            else
            {
                Debug.Log(message);
            }
#else
		    Debug.Log(message);
#endif
        }
    }

    [Serializable]
    public class AwsLevels
    {
        public AwsLevel[] levels;
    }

    [Serializable]
    public class AwsLevel
    {
        public string id;
        public string name;
        public AwsCreator creator;
        public string status;
        public uint createdAt;
        public uint updatedAt;
        public AwsData data;
    }

    [Serializable]
    public class AwsCreator
    {
        public string id;
        public string name;
    }

    [Serializable]
    public class AwsData
    {
        public int scaleX;
        public int scaleY;
        public string tiles;
        public int version;
    }

    [Serializable]
    public class AwsTileData
    {
        public TileSave[] tiles;
    }

    [Serializable]
    public class AwsUploadResponse
    {
        public string message;
        // @todo update the web interface.
        public string id;
        public string data;
    }
}
