#if UNITY_PACKAGE_ANALYTICS


using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class UserAnalytics : MonoBehaviour
{

    public void SendUserNameChosen(string userName)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "userName", userName}
        };
        AnalyticsService.Instance.CustomData("nameChosen", parameters);
    }

    public void SendStartEditor(string code)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "levelCode", code}
        };
        AnalyticsService.Instance.CustomData("startEditor", parameters);
    }

    public void SendStartLevel(string code)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "levelCode", code}
        };
        AnalyticsService.Instance.CustomData("startLevel", parameters);
    }

    public void SendCompleteLevel(string code, string time)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "levelCode", code},
            { "time", time}
        };
        AnalyticsService.Instance.CustomData("completeLevel", parameters);
    }

    public void SendDownloadLevel(string code)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "levelCode", code}
        };
        AnalyticsService.Instance.CustomData("downloadLevel", parameters);
    }

    public void SendUploadLevel(string code)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "levelCode", code}
        };
        AnalyticsService.Instance.CustomData("uploadLevel", parameters);
    }

    private void OnEnable()
    {
        UserNameForm.OnNameChosen += SendUserNameChosen;
        LevelAnalytics.OnLevelEditStart += SendStartEditor;
        LevelAnalytics.OnLevelPlayStart += SendStartLevel;
        WinMenu.OnScoreSubmit += SendCompleteLevel;
        DownloadForm.OnLevelDownload += SendDownloadLevel;
        ConfirmSubmitForm.OnLevelUpload += SendUploadLevel;
    }

    private void OnDisable()
    {
        UserNameForm.OnNameChosen -= SendUserNameChosen;
        LevelAnalytics.OnLevelEditStart -= SendStartEditor;
        LevelAnalytics.OnLevelPlayStart -= SendStartLevel;
        WinMenu.OnScoreSubmit -= SendCompleteLevel;
        DownloadForm.OnLevelDownload -= SendDownloadLevel;
        ConfirmSubmitForm.OnLevelUpload -= SendUploadLevel;
    }
}

#endif
