using System;
using System.Collections.Generic;
using Singletons;
using Steamworks.Data;
using UnityEngine;

namespace SteamIntegration
{
    public class SteamManager: UnitySingleton<SteamManager>
    {
        public const uint SteamAppsId = 2609410;
        public bool IsInitialized => _initialized;
        private bool _initialized = false;

#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
        private Dictionary<GameAchievement, Steamworks.Data.Achievement> _achievements = new Dictionary<GameAchievement, Achievement>();
#endif

        // public const uint SteamAppsId = 1769870;

#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
        protected override void Awake()
        {
            base.Awake();

            TrackAchievements();
        }
#endif


        public void Init()
        {
            try
            {
                if (_initialized)
                {
                    Debug.Log( "Steam is already initialised" );
                    return;
                }

#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
                Steamworks.SteamClient.Init( SteamAppsId );
#endif

                Debug.Log( "Steam initialised" );
                _initialized = true;
            }
            catch ( Exception e )
            {
                // Couldn't init for some reason (steam is closed etc)
                Debug.Log( "Steam initialising failed" );
                Debug.LogException(e);
            }
        }

        private void OnApplicationQuit()
        {
            // Code to execute when the game is closed or the editor stops playing
            Debug.Log("Game is quitting or editor play mode stopped.");
            // Your code here
#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
            Steamworks.SteamClient.Shutdown();
#endif
        }

#if !UNITY_WEBGL && !UNITY_EDITOR_OSX

        private void TrackAchievements()
        {
            foreach (Achievement achievement in Steamworks.SteamUserStats.Achievements)
            {
                var achievementName = (GameAchievement) Enum.Parse(typeof(GameAchievement), achievement.Name);
                if (achievementName == GameAchievement.None)
                {
                    continue;
                }

                _achievements.Add(achievementName, achievement);

            }
        }

        public bool UnlockAchievement(GameAchievement achievement)
        {
            if (!_achievements.ContainsKey(achievement))
            {
                Debug.LogError($"Achievement {achievement} not found.");
                return false;
            }

            Steamworks.Data.Achievement steamAchievement = _achievements[achievement];
            if (steamAchievement.State)
            {
                Debug.Log($"Achievement {achievement} already unlocked.");
                return false;
            }

            return steamAchievement.Trigger();
        }
#endif

    }
}
