using System;
using Editarrr.Level;
using Editarrr.LevelEditor;
using Gameplay;
using Player;
using SteamIntegration;
using Systems;
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
        DieFirstThreshold = 10,
        DieSecondThreshold = 11,
        DieThirdThreshold = 12,
        DieFourthThreshold = 13,
        DieFifthThreshold = 14, //not implemented atm
        CompleteLevelsFirstThreshold = 20,
        CompleteLevelsSecondThreshold = 21,
        CompleteLevelsThirdThreshold = 22,
        CompleteLevelsFourthThreshold = 23,
        DieInLevelFirstThreshold = 30,
        DieInLevelSecondThreshold = 31,
        DieInLevelThirdThreshold = 32,
        DieInLevelFourthThreshold = 33,
    }

    /**
     * This class is used to store user preferences.
     */
    public class AchievementManager : UnityPersistentSingleton<AchievementManager>
    {
        public static event Action<PopupAchievement> OnShowAchievement;

        private string _code = "";

        [SerializeField] AchievementPool AchievementPool;
        [SerializeField] ThresholdAchievement DeathAchievements;
        [SerializeField] ThresholdAchievement LevelCompletedAchievements;
        [SerializeField] ThresholdAchievement DiedInLevelAchievements;

        public void ProgressAchievement(GameAchievement achievement)
        {
            // For achievements that have thresholds we increment the count with every step.

            // Get current count.
            // Add one.
            // Check if new threshold is reached.
            bool thresholdReached = true;
            if (thresholdReached)
            {
                UnlockAchievement(achievement);
            }
        }

        public void UnlockAchievement(GameAchievement achievement)
        {
            if (!PreferencesManager.Instance.IsAchievementUnlocked(achievement))
            {
                PreferencesManager.Instance.SetAchievementUnlocked(achievement);

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
            HealthSystem.OnDeath += (s, e) => HandleAchievementProgress(DiedInLevelAchievements, true);
            Chest.OnChestOpened += () => HandleAchievementProgress(LevelCompletedAchievements);
        }

        private void OnDisable()
        {
            HealthSystem.OnDeath -= (s, e) => HandleAchievementProgress(DeathAchievements);
            HealthSystem.OnDeath -= (s, e) => HandleAchievementProgress(DiedInLevelAchievements, true);
            Chest.OnChestOpened -= () => HandleAchievementProgress(LevelCompletedAchievements);
        }

        private void HandleAchievementProgress(ThresholdAchievement achievement, bool needsCode = false)
        {
            var level = FindObjectOfType<LevelPlaySystem>().Manager.Level;
            var saveString = needsCode ? achievement.SavePrefString + _code : achievement.SavePrefString;
            var currentPlayerID = PreferencesManager.Instance.GetUserId();

            if (saveString != "" && level.Creator != currentPlayerID && level.Published)
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
