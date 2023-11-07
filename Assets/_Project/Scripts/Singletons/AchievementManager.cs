using System;
using Editarrr.LevelEditor;
using Gameplay;
using Player;
using SteamIntegration;
using UI;
using UnityEngine;

namespace Singletons
{
    public enum GameAchievement
    {
        None = 0,
        LevelSubmitted = 1,
        LevelRated = 2,
        LevelCompleted = 3,
        LevelScoreSubmitted = 4,
        LevelLeaderboardTopped = 5,
        DieFiveTimes = 10,
        DieTenTimes = 11,
        DieTwentyTimes = 12,
        DieFiftyTimes = 13,
        DieHundredTimes = 14,
        CompleteFiveLevels = 20,
        CompleteTenLevels = 21,
        CompleteTwentyLevels = 22,
        CompleteFiftyLevels = 23,
        PlayLevelFiveTimes = 30,
        PlayLevelTenTimes = 31,
        PlayLevelTwentyTimes = 32,
        PlayLevelFiftyTimes = 33,
    }

    /**
     * This class is used to store user preferences.
     */
    public class AchievementManager : UnitySingleton<AchievementManager>, IUnitySinglton
    {
        public static event Action<PopupAchievement> OnShowAchievement;

        private string _code = "";

        [SerializeField] AchievementPool AchievementPool;
        [SerializeField] ThresholdAchievement DeathAchievements;
        [SerializeField] ThresholdAchievement LevelCompletedAchievements;
        [SerializeField] ThresholdAchievement LevelPlayedAchievements;

        public void UnlockAchievement(GameAchievement achievement)
        {
            if (SteamManager.Instance.IsInitialized)
            {
                var success = SteamManager.Instance.UnlockAchievement(achievement);
                return;
            }

            PopupAchievement popup = AchievementPool.Get(achievement);
            if (popup != null)
            {
                OnShowAchievement?.Invoke(popup);
            }
        }

        public void SetLevelCode(string code)
        {
            _code = code;
        }

        public void Initialize()
        {
            // Nothing needed here.
        }

        private void OnEnable()
        {
            HealthSystem.OnDeath += (s, e) => HandleAchievementProgress(DeathAchievements);
            HealthSystem.OnDeath += (s, e) => HandleAchievementProgress(LevelPlayedAchievements, true);
            Chest.OnChestOpened += () => HandleAchievementProgress(LevelCompletedAchievements);
            Chest.OnChestOpened += () => HandleAchievementProgress(LevelPlayedAchievements, true);
        }

        private void OnDisable()
        {
            HealthSystem.OnDeath -= (s, e) => HandleAchievementProgress(DeathAchievements);
            HealthSystem.OnDeath -= (s, e) => HandleAchievementProgress(LevelPlayedAchievements, true);
            Chest.OnChestOpened -= () => HandleAchievementProgress(LevelCompletedAchievements);
            Chest.OnChestOpened -= () => HandleAchievementProgress(LevelPlayedAchievements, true);
        }

        private void HandleAchievementProgress(ThresholdAchievement achievement, bool needsCode = false)
        {
            var saveString = needsCode ? achievement.SavePrefString + _code : achievement.SavePrefString;

            if (saveString != "")
            {
                if (!PlayerPrefs.HasKey(saveString))
                {
                    PlayerPrefs.SetInt(saveString, 1);
                }
                else
                {
                    var currentProgress = PlayerPrefs.GetInt(saveString);

                    foreach (var threshold in achievement.AchievementDictionary)
                    {
                        if (currentProgress < threshold.Value && currentProgress + 1 == threshold.Value)
                        {
                            Instance.UnlockAchievement(threshold.Key);
                            break;
                        }
                    }

                    currentProgress++;
                    PlayerPrefs.SetInt(saveString, currentProgress);
                }
            }
        }
    }
}
