using System;
using Editarrr.Misc;
using Level.Storage;
using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "SteamLevelStorageManager", menuName = "Managers/Storage/new Steam Level Storage Manager")]
    public class SteamLevelStorageManager : LevelStorageManager
    {
        [field: SerializeField,
            Info("This allows storage management of levels in Steamworks Workshop UGC."),
            Header("Path")]
        private string LocalDirectoryName { get; set; } = "levels";


        [field: SerializeField, Header("Debug")] private bool UseDebugCode { get; set; }
        [field: SerializeField] private string DebugCode { get; set; } = "00001";


        public override void Save(string code, string data)
        {
            throw new NotImplementedException();
        }

        public override void SaveScreenshot(string code, byte[] screenshot)
        {
            throw new NotImplementedException();
        }

        public override string GetUniqueCode()
        {
            throw new NotImplementedException();
        }

        public override void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback)
        {
            // Call async function.
            LoadLevelDataAsync( code, callback );
        }

        private async void LoadLevelDataAsync(string code, LevelStorage_LevelLoadedCallback callback)
        {
            ulong ucode = Convert.ToUInt64( code );
            var itemInfo = await Steamworks.Ugc.Item.GetAsync( ucode );
            callback?.Invoke(null);
        }


        public override void LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback callback)
        {
            // Call async function.
            LoadAllLevelDataAsync( callback );
        }

        private async void LoadAllLevelDataAsync(LevelStorage_AllLevelsLoadedCallback callback)
        {
            var q = Steamworks.Ugc.Query.Items
                .WithTag("level")
                .AllowCachedResponse(1)
                .MatchAnyTag();

            var result = await q.GetPageAsync( 1 );

            Debug.Log( $"ResultCount: {result?.ResultCount}" );
            Debug.Log( $"TotalCount: {result?.TotalCount}" );

            foreach ( Steamworks.Ugc.Item entry in result.Value.Entries )
            {
                Debug.Log( $"{entry.Title}" );
            }

            callback?.Invoke(null);
        }

        public override void Delete(string code)
        {
            throw new NotImplementedException();
        }

        public override string GetScreenshotPath(string code)
        {
            throw new NotImplementedException();
        }
    }
}
