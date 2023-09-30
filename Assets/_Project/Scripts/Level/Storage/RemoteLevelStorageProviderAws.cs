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
            if (!levelSave.Published)
            {
                return;
            }

            RestClient.Get<AwsLevel>($"{_awsLevelUrl}/dev/levels/{levelSave.RemoteId}").Then(res =>
            {
               // Existing level found.
               this.Update(levelSave, callback);
            }).Catch(err =>
            {
                // No level found.
                this.Insert(levelSave, callback);
            });
        }

        private void Insert(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            string userId = PlayerPrefs.GetString(UserNameForm.UserIdStorageKey);

            AwsTileData tileData = new AwsTileData()
            {
                tiles = levelSave.Tiles
            };
            AwsLevel request = new AwsLevel
            {
                id = levelSave.Code,
                name = levelSave.Code,
                creator = new AwsCreator()
                {
                    name = levelSave.Creator,
                    id = userId
                },
                status = levelSave.Published ? "PUBLISHED" : "DRAFT",
                data = new AwsData()
                {
                    scaleX = levelSave.ScaleX,
                    scaleY = levelSave.ScaleY,
                    tiles = JsonUtility.ToJson(tileData)
                }
            };

            RestClient.Post<AwsUploadResponse>($"{_awsLevelUrl}/dev/levels", JsonUtility.ToJson(request)).Then(res =>
            {
                callback?.Invoke(levelSave.Code, res.id);
                this.LogMessage("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);
            });
        }
        private void Update(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            string userId = PlayerPrefs.GetString(UserNameForm.UserIdStorageKey);

            AwsTileData tileData = new AwsTileData()
            {
                tiles = levelSave.Tiles
            };
            AwsLevel request = new AwsLevel
            {
                id = levelSave.Code,
                name = levelSave.Code,
                creator = new AwsCreator()
                {
                    name = levelSave.Creator,
                    id = userId
                },
                status = levelSave.Published ? "PUBLISHED" : "DRAFT",
                data = new AwsData()
                {
                    scaleX = levelSave.ScaleX,
                    scaleY = levelSave.ScaleY,
                    tiles = JsonUtility.ToJson(tileData)
                }
            };

            RestClient.Patch<AwsUploadResponse>($"{_awsLevelUrl}/dev/levels/{request.id}", JsonUtility.ToJson(request)).Then(res =>
            {
                callback?.Invoke(levelSave.Code, res.id.ToString());
                this.LogMessage("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);
            });
        }

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            string response =
                "{  \"id\": \"00001\",  \"name\": \"Level 00001\",  \"creator\": {    \"id\": \"user1\",    \"name\": \"User 1\"  },  \"status\": \"published\",  \"createdAt\": 1686495335,  \"updatedAt\": 1686495335,  \"data\": {}}";


            RestClient.Get<AwsLevel>($"{_awsLevelUrl}/dev/levels/{code}").Then(res =>
            {
                // @todo make sure we store the remote level id in the save.
                LevelSave save = new LevelSave(res.creator.name, res.name);

                save.SetRemoteId(res.id);

                var tiles = JsonUtility.FromJson<AwsTileData>(res.data.tiles);
                TileState[,] tileStates = new TileState[res.data.scaleX, res.data.scaleY];

                foreach (TileSave tileSave in tiles.tiles)
                {
                    tileStates[tileSave.X, tileSave.Y] = new TileState(tileSave);
                }

                save.SetTiles(tileStates);
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
            // Make API request to get all levels. URL/levels
            // Parse JSON response into LevelSave[]
            // Call callback with LevelSave[]

            string response =
                "{\"levels\":[{\"id\":\"level2\",\"name\":\"Level 2\",\"creator\":{\"id\":\"user2\",\"name\":\"User 2\"},\"status\":\"published\",\"createdAt\":1695649746,\"updatedAt\":1695649746},{\"id\":\"level1\",\"name\":\"Level 1\",\"creator\":{\"id\":\"user1\",\"name\":\"User 1\"},\"status\":\"published\",\"createdAt\":1686495335,\"updatedAt\":1686495335}]}";


            // Get request to /node/level
            RestClient.Get<AwsLevels>($"{_awsLevelUrl}/dev/levels").Then(res =>
            {
                List<LevelStub> levelStubs = new List<LevelStub>();
                foreach (AwsLevel level in res.levels)
                {
                    LevelStub levelStub = new LevelStub(level.name, level.creator.name, level.id, level.status == "PUBLISHED");
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
    }
}
