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

        private void OnEnable()
        {
            HealthSystem.OnDeath += (s, e) => HandleAchievementProgress(DeathAchievements);
            HealthSystem.OnDeath += (s, e) => HandleAchievementProgress(DiedInLevelAchievements, true);
            Chest.OnChestReached += () => HandleAchievementProgress(LevelCompletedAchievements);
        }

        private void OnDisable()
        {
            HealthSystem.OnDeath -= (s, e) => HandleAchievementProgress(DeathAchievements);
            HealthSystem.OnDeath -= (s, e) => HandleAchievementProgress(DiedInLevelAchievements, true);
            Chest.OnChestReached -= () => HandleAchievementProgress(LevelCompletedAchievements);
        }

        public void SetLevelCode(string code)
        {
            _code = code;
        }

        public void Initialize()
        {
            // Nothing needed here.
        }

        public void UnlockAchievement(GameAchievement achievement)
        {
            if (!PreferencesManager.Instance.IsAchievementUnlocked(achievement) && CheckAchievementEligibility())
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

        private void HandleAchievementProgress(ThresholdAchievement achievement, bool needsCode = false)
        {
            var saveString = needsCode ? achievement.SavePrefString + _code : achievement.SavePrefString;

            if (saveString != "" && CheckAchievementEligibility())
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

        private bool CheckAchievementEligibility()
        {
            bool outcome = true;

            try
            {
                var level = FindObjectOfType<LevelPlaySystem>().Manager.Level;
                var currentPlayerID = PreferencesManager.Instance.GetUserId();
                outcome = level.Creator != currentPlayerID && level.Published;
            }
            catch (Exception e)
            {
                Debug.LogError("Error happened while trying to check achievement eligibility");
            }

            return outcome;
        }
    }
}
