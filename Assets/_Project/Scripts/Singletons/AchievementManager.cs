using System;
using Editarrr.LevelEditor;
using SteamIntegration;
using UI;
using UnityEditor;

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

        private AchievementPool _achievementPool;

        public void UnlockAchievement(GameAchievement achievement)
        {
            if (SteamManager.Instance.IsInitialized)
            {
                var success = SteamManager.Instance.UnlockAchievement(achievement);
                return;
            }

            PopupAchievement popup = _achievementPool.Get(achievement);
            if (popup != null)
            {
                OnShowAchievement?.Invoke(popup);
            }
        }

        public void Initialize()
        {
            // Nothing needed here.
            _achievementPool = (AchievementPool) AssetDatabase.LoadAssetAtPath("Assets/_Project/ScriptableObjects/Achievement/GameAchievementPool.asset", typeof(AchievementPool));
        }
    }
}
