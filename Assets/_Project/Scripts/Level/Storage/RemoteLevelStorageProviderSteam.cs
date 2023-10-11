#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Editarrr.Level;
using SteamIntegration;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace Level.Storage
{
    public class RemoteLevelStorageProviderSteam : IRemoteLevelStorageProvider
    {
        public static event Action<Steamworks.Ugc.Item> OnSteamLevelDownloadComplete;

        private RemoteLevelStorage_LevelUploadedCallback _uploadCompleteCallback;
        private LevelSave _uploadCompleteLevelSave;

        public void Initialize()
        {
            // This is for dev only. Should be called in the user name scene.
            // SteamManager.Instance.Init();
            Steamworks.SteamUGC.OnDownloadItemResult += OnDownloadItemResult;
        }

        private void OnDownloadItemResult(Result obj)
        {
            throw new NotImplementedException();
        }

        public void Upload(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            if (levelSave == null)
            {
                return;
            }

            // We only want to upload levels that are published.
            if (!levelSave.Published)
            {
                return;
            }

            _uploadCompleteCallback = callback;
            _uploadCompleteLevelSave = levelSave;

            if (levelSave.SteamId == 0)
            {
                DoInsert();
            }
            else
            {
                // This shouldn't really happen, but is supported for now.
                DoUpdate();
            }
        }

        private async void DoInsert()
        {
            LevelSave levelSave = _uploadCompleteLevelSave;

            var data = Steamworks.Ugc.Editor.NewCommunityFile
                .WithTitle(levelSave.Code)
                .WithDescription($"A level created by {levelSave.CreatorName}")
                .WithContent(levelSave.LocalDirectory)
                .WithPreviewFile($"{levelSave.LocalDirectory}/screenshot.png")
                .WithTag("level");

            if (levelSave.Published)
            {
                data.WithPublicVisibility();
            }

            DoUpload(data);
        }
        private async void DoUpdate()
        {
            LevelSave levelSave = _uploadCompleteLevelSave;

            ulong ucode = Convert.ToUInt64(levelSave.SteamId);
            var data = new Steamworks.Ugc.Editor(ucode)
                .WithTitle(levelSave.Code)
                .WithDescription($"A level created by {levelSave.CreatorName}")
                .WithContent(levelSave.LocalDirectory)
                .WithPreviewFile($"{levelSave.LocalDirectory}/screenshot.png")
                .WithTag("level");

            if (levelSave.Published)
            {
                data.WithPublicVisibility();
            }

            DoUpload(data);
        }

        private async void DoUpload(Steamworks.Ugc.Editor data)
        {
            LevelSave levelSave = _uploadCompleteLevelSave;
            _uploadCompleteLevelSave = null;
            RemoteLevelStorage_LevelUploadedCallback callback = _uploadCompleteCallback;
            _uploadCompleteCallback = null;

            var progress = new UploadProgress();
            var result =  await data.SubmitAsync(progress);

            ulong id = result.FileId;

            callback.Invoke(levelSave.Code, id.ToString(), true);
            Debug.Log($"Result: {result.Result}");
        }

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            return;

            DownloadAsync(code, callback);
        }

        public void DownloadScreenshot(string code, RemoteLevelStorage_LevelScreenshotDownloadedCallback callback)
        {
            throw new NotImplementedException();
        }

        private async void DownloadAsync(string code, RemoteLevelStorage_LevelLoadedCallback callback)
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
                // Debug.Log("Item is already downloaded or downloading");
                // return;
            }

            var downloadProgress = new Action<float>((progress) =>
            {
                Debug.Log($"Download Progress: {progress}");

                if (progress >= 1f)
                {
                    OnSteamLevelDownloadComplete?.Invoke(item);
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
            // Disabling level browsing for now.
            return;

            // Call async function.
            LoadAllLevelDataAsync(callback);
        }

        private async void LoadAllLevelDataAsync(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            var q = Steamworks.Ugc.Query.All
                .WithTag("level")
                .MatchAnyTag();

            var result = await q.GetPageAsync(1);

            Debug.Log($"ResultCount: {result?.ResultCount}");
            Debug.Log($"TotalCount: {result?.TotalCount}");

            var levels = new List<LevelStub>();

            foreach (Steamworks.Ugc.Item entry in result.Value.Entries)
            {
                string code = entry.Title.Length > 0 ? entry.Title : entry.Id.ToString();
                // @todo figure out creator id.
                string creatorId = "";
                var save = new LevelStub(code, entry.Owner.Name, creatorId, entry.Id.ToString(), true);
                levels.Add(save);
                Debug.Log($"{entry.Title}");
            }

            callback?.Invoke(levels.ToArray());
        }

        public bool SupportsLeaderboards()
        {
            return false;
        }

        public void SubmitScore(float score, LevelSave levelSave, RemoteScoreStorage_ScoreSubmittedCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public void GetScoresForLevel(string code, RemoteScoreStorage_AllScoresLoadedCallback callback)
        {
            throw new System.NotImplementedException();
        }
    }

    public class UploadProgress : IProgress<float>
    {
        float lastvalue = 0;

        public void Report( float value )
        {
            if ( lastvalue >= value ) return;

            lastvalue = value;
            Debug.Log($"Upload progress: {value}");
        }
    }
}
#endif
