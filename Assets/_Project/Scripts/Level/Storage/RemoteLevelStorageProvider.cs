using System;
using UnityEngine;

namespace Level.Storage
{
    [CreateAssetMenu(fileName = "Remote Level Storage Provider", menuName = "Managers/Storage/new Remote Level Storage Provider")]
    public class RemoteLevelStorageProvider : ScriptableObject
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Upload(string code, string data)
        {
            throw new NotImplementedException();
        }

        public void Download(string code, string data)
        {
            throw new NotImplementedException();
        }

        public void LoadLevelData(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            throw new NotImplementedException();
            callback?.Invoke(null);
        }

        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            throw new NotImplementedException();
            callback?.Invoke(null);
        }
    }
}
