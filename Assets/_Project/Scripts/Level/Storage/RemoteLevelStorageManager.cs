using System;
using System.Collections.Generic;
using System.Linq;
using Editarrr.Level;
using Editarrr.Misc;
using UnityEngine;

namespace Level.Storage
{
    public enum RemoteLevelStorageProviderType
    {
        Steam,
        Aws
    }

    [CreateAssetMenu(fileName = "RemoteLevelStorageManager", menuName = "Managers/Storage/new Remote Level Storage Manager")]
    public class RemoteLevelStorageManager : ScriptableObject, IRemoteLevelStorage
    {
        [field: SerializeField,
                Info("The RemoteLevelStorageManager class manages storage of levels remotely. It can have multiple different providers who all are chained together.")]
        private RemoteLevelStorageProviderType[] Providers { get; set; }

        private List<IRemoteLevelStorageProvider> _providers = new List<IRemoteLevelStorageProvider>();

        public void Initialize()
        {
            _providers.Clear();

            IRemoteLevelStorageProvider provider;
            if (Providers.Contains(RemoteLevelStorageProviderType.Aws))
            {
                provider = new RemoteLevelStorageProviderAws();
                provider.Initialize();
                _providers.Add(provider);
            }
            // @todo Change this so multiple providers can be used at once.
#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
            if (SteamIntegration.SteamManager.Instance.IsInitialized && Providers.Contains(RemoteLevelStorageProviderType.Steam))
            {
                provider = new RemoteLevelStorageProviderSteam();
                provider.Initialize();
                _providers.Add(provider);
            }
#endif
        }

        public void Upload(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.Upload(levelSave, callback);
            }
        }

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.Download(code, callback);
            }
        }

        public void DownloadScreenshot(string code, RemoteLevelStorage_LevelScreenshotDownloadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.DownloadScreenshot(code, callback);
            }
        }

        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.LoadAllLevelData(callback);
            }
        }

        public void SubmitScore(float score, LevelSave levelSave, RemoteScoreStorage_ScoreSubmittedCallback callback)
        {
            foreach (var provider in _providers)
            {
                if (!provider.SupportsLeaderboards())
                {
                    continue;
                }

                provider.SubmitScore(score, levelSave, callback);
            }
        }

        public void GetScoresForLevel(string code, RemoteScoreStorage_AllScoresLoadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                if (!provider.SupportsLeaderboards())
                {
                    continue;
                }

                provider.GetScoresForLevel(code, callback);
            }
        }

        public void SubmitRating(int score, LevelSave levelSave, RemoteRatingStorage_RatingSubmittedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.SubmitRating(score, levelSave, callback);
            }
        }

        public void GetRatingsForLevel(string code, RemoteRatingStorage_AllRatingsLoadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.GetRatingsForLevel(code, callback);
            }
        }
    }
}
