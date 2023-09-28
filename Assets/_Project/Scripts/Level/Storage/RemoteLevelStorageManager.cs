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
            if (Providers.Contains(RemoteLevelStorageProviderType.Steam))
            {
                _providers.Add(new RemoteLevelStorageProviderSteam());
            }
            if (Providers.Contains(RemoteLevelStorageProviderType.Aws))
            {
                _providers.Add(new RemoteLevelStorageProviderAws());
            }
        }

        public void Upload(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.Upload(levelSave, callback);
            }
        }

        public void UploadScreenshot(string code, byte[] screenshot)
        {
            throw new NotImplementedException();
        }

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.Download(code, callback);
            }
        }

        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            foreach (var provider in _providers)
            {
                provider.LoadAllLevelData(callback);
            }
        }

        public void SubmitScore()
        {
            foreach (var provider in _providers)
            {
                if (!provider.SupportsLeaderboards())
                {
                    continue;
                }
                provider.SubmitScore();
            }
        }
    }
}
