using System;
using Level.Storage;
using Proyecto26;
using UnityEngine;

namespace Singletons
{
    public enum AnalyticsEvent
    {
        NewUserRegistered = 0,
        UserSessionStarted = 1,
        UserSessionEnded = 2,

        // Levels
        LevelCreate = 10,
        LevelSave = 11,
        LevelPlay = 12,
        LevelComplete = 13,

        LevelUpload = 15,
        LevelDownload = 16,

        // Scores
        LevelScoreSubmitted = 20,

        // Rating
        LevelRatingSubmitted = 30,
    }

    /**
     * This class is used to store user preferences.
     */
    public class AnalyticsManager : UnitySingleton<AnalyticsManager>, IUnitySinglton
    {
        private string _creatorId;
        private string _creatorName;

        private bool _enableTracking = false;

        public void Initialize()
        {
            _creatorId = PreferencesManager.Instance.GetUserId();
            _creatorName = PreferencesManager.Instance.GetUserName();
        }

        public void TrackEvent(AnalyticsEvent trackedEvent, string value = "")
        {
            if (!_enableTracking)
            {
                return;
            }

            DoSubmitRequest(trackedEvent.ToString(), value);
        }

        private void DoSubmitRequest(string type, string value)
        {
            AnalyticsRequest request = new AnalyticsRequest()
            {
                creatorId = _creatorId,
                creatorName = _creatorName,
                type = type,
                value = value,
            };

            RestClient.Post<AnalyticsResponse>($"{RemoteLevelStorageProviderAws.AwsLevelUrl}/analytics",
                JsonUtility.ToJson(request)).Then(res =>
            {
                Debug.Log("Analytics event tracked");
                Debug.Log(res.message);
            }).Catch(err =>
            {
                Debug.LogError(err.Message);
            });
        }

        private void OnApplicationQuit()
        {
            string sessionId = PreferencesManager.Instance.GetSessionId();
            this.TrackEvent(AnalyticsEvent.UserSessionEnded, sessionId);
        }
    }

    [Serializable]
    public class AnalyticsRequest
    {
        public string creatorId;
        public string creatorName;
        public string type;
        public string value;
    }

    [Serializable]
    public class AnalyticsResponse
    {
        public string message;
        public string id;
    }
}
