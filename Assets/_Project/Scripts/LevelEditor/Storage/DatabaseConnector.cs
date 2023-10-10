using System;
using System.Collections;
using System.Text;
using Singletons;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace LevelEditor
{

    public enum DatabaseRequestType
    {
        GetData,
        InsertData,
        UpdateData,
        InsertComment,
        GetLevelComments,
        FileDownload,
    }

    public class DatabaseConnector : MonoBehaviour, IDbConnector
    {
        private const string codeChars = "bcdfghjklmnpqrstvwxyz123456789";
        // private const string serverUrl = "http://161.35.3.150:80/db_test.php";
        private const string serverUrl = "https://editarrr.lpgam.es/db_test.php";

        private string _code;
        private string _user;
        private string _data;

        public void GetData(string code)
        {
            _code = code;

            _user = PreferencesManager.Instance.GetUserName();
            StartCoroutine(nameof(SendRequest), DatabaseRequestType.GetData);
        }

        public void CreateData(string code, string data)
        {
            _code = code;
            _data = data;
            _user = PreferencesManager.Instance.GetUserName();
            StartCoroutine(nameof(SendRequest), DatabaseRequestType.InsertData);
        }

        public void UpdateData(string code, string data)
        {
            _code = code;
            _data = data;
            _user = PreferencesManager.Instance.GetUserName();
            StartCoroutine(nameof(SendRequest), DatabaseRequestType.UpdateData);
        }

        public void CreateComment(string code, string time)
        {
            throw new NotImplementedException();
        }

        public void CreateComment(string code, string time, string ghost)
        {
            throw new NotImplementedException();
        }

        public void GetComments(string code)
        {
            throw new NotImplementedException();
        }

        private IEnumerator SendRequest(DatabaseRequestType method)
        {

            PostRequestData requestData = new PostRequestData();

            switch (method)
            {
                default:
                case DatabaseRequestType.GetData:
                    requestData.method = "get";
                    break;

                case DatabaseRequestType.InsertData:
                    requestData.method = "create";
                    break;

                case DatabaseRequestType.UpdateData:
                    requestData.method = "update";
                    break;
            }

            // @todo get from steam.
            requestData.user = _user;
            requestData.code = _code;
            requestData.data = _data;

            string requestDataString = JsonUtility.ToJson(requestData);

            var www = new UnityWebRequest (serverUrl, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestDataString);
            www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                PostRequestData result = JsonUtility.FromJson<PostRequestData>(www.downloadHandler.text);
            }
        }

        public string GetUniqueCode()
        {
            // @todo Check with database for uniqueness.
            return GetRandomCode();
        }

        private string GetRandomCode(int length = 5)
        {
            string code = "";
            for(int i=0; i<length; i++)
            {
                code += codeChars[Random.Range(0, codeChars.Length)];
            }

            return code;
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

    public interface IDbConnector
    {
        void GetData(string levelCode);

        string GetUniqueCode();
        void CreateData(string code, string levelData);
        void UpdateData(string code, string levelData);
        void CreateComment(string code, string time);
        void CreateComment(string code, string time, string ghost);
        void GetComments(string code);
    }
}
