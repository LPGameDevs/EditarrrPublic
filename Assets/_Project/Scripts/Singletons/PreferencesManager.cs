using System;
using System.IO;
using SteamIntegration;
using UI;
using UnityEngine;

namespace Singletons
{
    /**
     * This class is used to store user preferences.
     */
    public class PreferencesManager : UnitySingleton<PreferencesManager>, IUnitySinglton
    {
        public static event Action<string> OnStreamerChannelChanged;

        public const string UserIdStorageKey = "UserId";
        public const string UserNameStorageKey = "UserName";
        public const string SessionStorageKey = "UserSession";
        public const string DefaultUserName = "anon";
        public const string ScreenShakeKey = "screenShake";
        public const string ScreenFlashKey = "screenFlash";
        public const string StreamerKey = "userIsStreamer";

        public string SyncFilePath => Application.persistentDataPath + "/preferences-sync.json";
        private SyncSaveFile _syncSave;

        public void Initialize()
        {
            if (!File.Exists(SyncFilePath))
            {
                File.WriteAllText(SyncFilePath, "{}");
            }

            string data = File.ReadAllText(SyncFilePath);
            _syncSave = JsonUtility.FromJson<SyncSaveFile>(data);

            // If we have a user id check if it needs to override the preference.
            if (_syncSave.uuid.Length > 0)
            {
                if (_syncSave.uuid != GetUserId(true))
                {
                    SetUserId(_syncSave.uuid);
                }
            }
            else
            {
                // If no user id in the sync, set whatever is the current value.
                SetUserId(GetUserId());
            }
        }

        private void UpdateSaveFile()
        {
            string dataString = JsonUtility.ToJson(_syncSave);
            File.WriteAllText(SyncFilePath, dataString);
        }

        /**
         * @param bool readOnly
         *   If true will not generate a default value.
         */
        public string GetUserId(bool readOnly = false)
        {
            var userId = PlayerPrefs.GetString(UserIdStorageKey, "");

            // Set a new user id if one doesnt exist.
            if (!readOnly && userId.Length == 0)
            {
                userId = Guid.NewGuid().ToString();
                this.SetUserId(userId);

                // AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.NewUserRegistered);
            }

            return userId;
        }

        private void SetUserId(string userId)
        {
            PlayerPrefs.SetString(UserIdStorageKey, userId);

            // Update sync file.
            _syncSave.uuid = userId;
            UpdateSaveFile();
        }

        public string GetUserName()
        {
            var userName = PlayerPrefs.GetString(UserNameStorageKey, "");

            if (userName.Length > 0)
            {
                return userName;
            }

            // Set a new user id if one doesnt exist.
            if (SteamManager.Instance.IsInitialized)
            {
                userName = SteamManager.Instance.GetUserName();
            }
            else
            {
                userName = DefaultUserName;
            }

            this.SetUserName(userName);
            return userName;
        }

        public void SetUserName(string userName)
        {
            PlayerPrefs.SetString(UserNameStorageKey, userName);
        }

        public string GetSessionId()
        {
            return PlayerPrefs.GetString(SessionStorageKey, "");
        }

        public string StartNewSession()
        {
            string sessionId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString(SessionStorageKey, sessionId);
            return sessionId;
        }

        public void SetModalEventTracked(string name, ModalPopupAction action)
        {
            PlayerPrefs.SetInt($"ModalPopupTrack-{name}-{action.ToString()}", 1);
        }

        public bool IsModalEventTracked(string name, ModalPopupAction action)
        {
            int hasTracked = PlayerPrefs.GetInt($"ModalPopupTrack-{name}-{action.ToString()}", 0);
            return hasTracked == 1;
        }

        public void SetAchievementUnlocked(GameAchievement achievement)
        {
            PlayerPrefs.SetInt($"AchievementUnlock-{achievement.ToString()}", 1);
        }

        public bool IsAchievementUnlocked(GameAchievement achievement)
        {
            int hasTracked = PlayerPrefs.GetInt($"AchievementUnlock-{achievement.ToString()}", 0);
            return hasTracked == 1;
        }

        public float GetSlider(string volumeParameter)
        {
            return PlayerPrefs.GetFloat($"SliderVolume-{volumeParameter}", 0.7f);
        }

        public void SetSlider(string volumeParameter, float sliderValue)
        {
            PlayerPrefs.SetFloat($"SliderVolume-{volumeParameter}", sliderValue);
        }

        public void SetLevelRating(string code, int rating)
        {
            PlayerPrefs.SetInt($"LevelRating-{code}", 1);
        }

        public bool HasLevelRating(string code)
        {
            int hasLevelRating = PlayerPrefs.GetInt($"LevelRating-{code}", 0);
            return hasLevelRating == 1;
        }

        public int GetLevelRating(string code)
        {
            return PlayerPrefs.GetInt($"LevelRating-{code}", 0);
        }

        public void SetBoolean(string key, bool value)
        {
            int storedValue = value ? 1 : 0;
            PlayerPrefs.SetInt(key, storedValue);
        }

        public bool GetBoolean(string key, int defaultValue = 1)
        {
            int storedValue = PlayerPrefs.GetInt(key, defaultValue);
            return storedValue == 1 ? true : false;
        }

        public string GetStreamerChannel()
        {
            return PlayerPrefs.GetString($"StreamerChannel", "");
        }

        public void SetStreamerChannel(string channel)
        {
            PlayerPrefs.SetString($"StreamerChannel", channel);
            OnStreamerChannelChanged?.Invoke(channel);
        }

        public bool GetIsStreamer()
        {
            int isStreamer = PlayerPrefs.GetInt($"StreamerChannelIsStreamer", 0);
            return isStreamer == 1;
        }

        public void SetIsStreamer()
        {
            PlayerPrefs.SetInt($"StreamerChannelIsStreamer", 1);
        }

        public int GetFps()
        {
            return PlayerPrefs.GetInt($"TargetFPS", 120);
        }

        public void SetFps(int fps)
        {
            PlayerPrefs.SetInt($"TargetFPS", fps);
        }

        public bool IsOnboarded()
        {
            int isOnboarded = PlayerPrefs.GetInt("OnboardingCompleted", 0);
            return isOnboarded == 1;
        }

        public void SetOnboarded()
        {
            PlayerPrefs.SetInt("OnboardingCompleted", 1);
        }

        public UserTagType GetUserTypeTag()
        {
            int tag = PlayerPrefs.GetInt("UserTypeTag", (int) UserTagType.None);
            UserTagType tagType = (UserTagType) tag;

            return tagType;
        }

        public void SetUserTypeTag(UserTagType currentTag)
        {
            PlayerPrefs.SetInt("UserTypeTag", (int) currentTag);
        }

        public void ResetAll()
        {
            var id = this.GetUserId();
            PlayerPrefs.DeleteAll();
            this.SetUserId(id);
            this.StartNewSession();
        }

        public float GetBestLevelTime(string code)
        {
            return PlayerPrefs.GetFloat($"BestLevelScore-{code}", 0);
        }

        public void SetBestLevelTime(string code, float time)
        {
            PlayerPrefs.SetFloat($"BestLevelScore-{code}", time);
        }

        [Serializable]
        public class SyncSaveFile
        {
            public string uuid = "";
        }
    }
}
