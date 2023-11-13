using System;
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

        public string GetUserId()
        {
            var userId = PlayerPrefs.GetString(UserIdStorageKey, "");

            // Set a new user id if one doesnt exist.
            if (userId.Length == 0)
            {
                userId = Guid.NewGuid().ToString();
                this.SetUserId(userId);

                AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.NewUserRegistered);
            }

            return userId;
        }

        private void SetUserId(string userId)
        {
            PlayerPrefs.SetString(UserIdStorageKey, userId);
        }

        public string GetUserName()
        {
            var userName = PlayerPrefs.GetString(UserNameStorageKey, "");

            // Set a new user id if one doesnt exist.
            if (userName.Length == 0)
            {
                this.SetUserName(DefaultUserName);
            }

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

        public void Initialize()
        {
            // Nothing needed here.
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
    }
}
