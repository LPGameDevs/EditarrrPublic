using System;
using Steamworks;
using UnityEngine;

namespace SteamIntegration
{
    public class SteamManager: UnitySingleton<SteamManager>
    {
        public const uint SteamAppsId = 2609410;
        // public const uint SteamAppsId = 1769870;

        public void Init()
        {
            try
            {
                SteamClient.Init( SteamAppsId );
                Debug.Log( "Steam initialised" );

                Debug.Log(Steamworks.SteamApps.AvailableLanguages);
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
            Steamworks.SteamClient.Shutdown();
        }

    }
}