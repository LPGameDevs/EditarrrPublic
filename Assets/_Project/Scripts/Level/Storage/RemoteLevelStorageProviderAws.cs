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
            throw new NotImplementedException();
        }

        public void Upload(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            string user = levelSave.Creator;

            AwsLevel request = new AwsLevel
            {
                id = levelSave.Code,
                name = levelSave.Code,
                creator = new AwsCreator()
                {
                    name = user
                },
                status = levelSave.Published ? "published" : "draft",
                data = new AwsData()
                {
                    scaleX = levelSave.ScaleX,
                    scaleY = levelSave.ScaleY,
                    tiles = levelSave.Tiles
                }
            };

            RestClient.Post<AwsUploadResponse>($"{_awsLevelUrl}/dev/levels", JsonUtility.ToJson(request)).Then(res =>
            {
                callback?.Invoke(levelSave);
                this.LogMessage("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err => { this.LogMessage("Error", err.Message); });
        }

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            string response =
                "{  \"id\": \"00001\",  \"name\": \"Level 00001\",  \"creator\": {    \"id\": \"user1\",    \"name\": \"User 1\"  },  \"status\": \"published\",  \"createdAt\": 1686495335,  \"updatedAt\": 1686495335,  \"data\": {}}";


            RestClient.Get<AwsLevel>($"{_awsLevelUrl}/dev/levels/{code}").Then(res =>
            {
                LevelState state = new LevelState(res.data.scaleX, res.data.scaleY);
                state.SetCode(res.id);
                state.SetCreator(res.creator.name);
                state.SetPublished(res.status == "published");

                LevelSave save = state.CreateSave();
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
                    LevelStub levelStub = new LevelStub(level.id, level.creator.name, level.status == "published");
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
        public TileSave[] tiles;
    }

    [Serializable]
    public class AwsUploadResponse
    {
        public string message;
    }
}
