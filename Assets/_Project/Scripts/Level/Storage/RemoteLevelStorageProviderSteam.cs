using System;
using System.Collections.Generic;
using Editarrr.Level;
using UnityEngine;

namespace Level.Storage
{
    public class RemoteLevelStorageProviderSteam: IRemoteLevelStorageProvider
    {
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Upload(string code)
        {
            throw new System.NotImplementedException();
        }

        public void Download(string code)
        {
            throw new System.NotImplementedException();
        }

        public void LoadLevelData(string code)
        {
            // Call async function.
            LoadLevelDataAsync( code );
        }

        private async void LoadLevelDataAsync(string code)
        {
            ulong ucode = Convert.ToUInt64( code );
            var itemInfo = await Steamworks.Ugc.Item.GetAsync( ucode );
        }

        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            // Call async function.
            LoadAllLevelDataAsync(callback);
        }

        private async void LoadAllLevelDataAsync(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            var q = Steamworks.Ugc.Query.Items
                .WithTag("level")
                .AllowCachedResponse(1)
                .MatchAnyTag();

            var result = await q.GetPageAsync( 1 );

            Debug.Log( $"ResultCount: {result?.ResultCount}" );
            Debug.Log( $"TotalCount: {result?.TotalCount}" );

            var levels = new List<LevelSave>();

            foreach ( Steamworks.Ugc.Item entry in result.Value.Entries )
            {
                var save = new LevelSave(entry.Owner.Name.ToString(), entry.Id.ToString());
                levels.Add(save);
                Debug.Log( $"{entry.Title}" );
            }

            callback?.Invoke(levels.ToArray());
        }

        public bool SupportsLeaderboards()
        {
            return false;
        }

        public void SubmitScore()
        {
            throw new System.NotImplementedException();
        }
    }
}
