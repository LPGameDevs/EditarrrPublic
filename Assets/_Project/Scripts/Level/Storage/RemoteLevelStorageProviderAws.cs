using System;
using Proyecto26;
using UnityEditor;
using UnityEngine;

namespace Level.Storage
{
    public class RemoteLevelStorageProviderAws: IRemoteLevelStorageProvider
    {

        private const string _awsLevelUrl = "https://tlfb41owe5.execute-api.eu-north-1.amazonaws.com";
        private bool _showDebug = true;

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Upload(string code)
        {
            throw new NotImplementedException();
        }

        public void Download(string code)
        {
            throw new NotImplementedException();
        }

        public void LoadLevelData(string code)
        {
            string response = "{  \"id\": \"00001\",  \"name\": \"Level 00001\",  \"creator\": {    \"id\": \"user1\",    \"name\": \"User 1\"  },  \"status\": \"published\",  \"createdAt\": 1686495335,  \"updatedAt\": 1686495335,  \"data\": {}}";


            RestClient.Get<AwsLevel>($"{_awsLevelUrl}/dev/levels/{code}").Then(res => {

                // @todo return level data.
                this.LogMessage(res.id, JsonUtility.ToJson(res, true));

            }).Catch(err =>
            {
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
                this.LogMessage ("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
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

        private void LogMessage(string title, string message) {
#if UNITY_EDITOR
            if (_showDebug)
            {
                EditorUtility.DisplayDialog (title, message, "Ok");
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
        public string id;
        public string name;
    }
}
