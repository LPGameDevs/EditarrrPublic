using System;
using UI;
using UnityEngine;

namespace Singletons
{
    /**
     * This class is used to store user preferences.
     */
    public class PreferencesManager : UnitySingleton<PreferencesManager>
    {
        public const string UserIdStorageKey = "UserId";
        public const string UserNameStorageKey = "UserName";
        public const string DefaultUserName = "anon";

        public string GetUserId()
        {
            var userId = PlayerPrefs.GetString(UserIdStorageKey, "");

            // Set a new user id if one doesnt exist.
            if (userId.Length == 0)
            {
                userId = Guid.NewGuid().ToString();
                this.SetUserId(userId);
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

        public void SetModalEventTracked(string name, ModalPopupAction action)
        {
            PlayerPrefs.SetInt($"ModalPopupTrack-{name}-{action.ToString()}", 1);
        }

        public bool IsModalEventTracked(string name, ModalPopupAction action)
        {
            int hasTracked = PlayerPrefs.GetInt($"ModalPopupTrack-{name}-{action.ToString()}", 0);
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
    }
}
