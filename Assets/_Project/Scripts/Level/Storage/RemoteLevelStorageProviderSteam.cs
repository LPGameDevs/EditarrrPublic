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

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            throw new System.NotImplementedException();

            DownloadAsync(code);
        }

        private async void DownloadAsync(string code)
        {
            ulong ucode = Convert.ToUInt64(code);
            var itemInfo = await Steamworks.Ugc.Item.GetAsync(ucode);

            if (!itemInfo.HasValue)
            {
                Debug.Log("Item not found");
                return;
            }

            var item = itemInfo.Value;

            Debug.Log($"Title: {item.Title}");
            Debug.Log($"IsInstalled: {item.IsInstalled}");
            Debug.Log($"IsDownloading: {item.IsDownloading}");
            Debug.Log($"IsDownloadPending: {item.IsDownloadPending}");
            Debug.Log($"IsSubscribed: {item.IsSubscribed}");
            Debug.Log($"NeedsUpdate: {item.NeedsUpdate}");
            Debug.Log($"Description: {item.Description}");

            if (item.IsInstalled || item.IsDownloading || item.IsDownloadPending)
            {
                Debug.Log("Item is already downloaded or downloading");
                return;
            }

            var downloadProgress = new Action<float>((progress) =>
            {
                Debug.Log($"Download Progress: {progress}");

                if (progress >= 1f)
                {
                    // OnBrowserLevelDownloadComplete?.Invoke(item.Id.ToString() ?? "");
                }
            });

            // OnBrowserLevelDownload?.Invoke(item);

            bool isSubscribing = await item.Subscribe();
            bool isDownloading = await item.DownloadAsync(downloadProgress);
            Debug.Log("Subscribing" + isSubscribing);
            Debug.Log("Downloading" + isDownloading);
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

            var levels = new List<LevelStub>();

            foreach ( Steamworks.Ugc.Item entry in result.Value.Entries )
            {
                var save = new LevelStub(entry.Id.ToString(), entry.Owner.Name.ToString(), true);
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
