using System;
using UnityEngine;

namespace SteamIntegration
{
    public class SteamManager: UnitySingleton<SteamManager>
    {
        public const uint SteamAppsId = 2609410;
        private bool _initialized = false;
        // public const uint SteamAppsId = 1769870;

        public void Init()
        {
            try
            {
                if (_initialized)
                {
                    Debug.Log( "Steam is already initialised" );
                    return;
                }

#if !UNITY_WEBGL
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
#if !UNITY_WEBGL
            Steamworks.SteamClient.Shutdown();
#endif
        }

    }
}
