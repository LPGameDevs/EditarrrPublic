using System;
using UnityEngine;

namespace Editarrr.Level
{
    public abstract class LevelStorageManager : ScriptableObject, ILevelStorage
    {
        public abstract void Save(string code, string data);
        public abstract void SaveScreenshot(string code, byte[] screenshot);
        public abstract string GetScreenshotPath(string code);
        public abstract string GetUniqueCode();
        public abstract void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback);
        public abstract void LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback callback);
        public abstract void Delete(string code);
    }

    public class DatabaseLevelStorageManager : LevelStorageManager
    {
        /// <summary>
        /// Help, I do not know what I am doing!
        ///
        /// @todo Find help...
        /// </summary>
        [field: SerializeField] public string ConnectionString { get; private set; }
        [field: SerializeField] public string OtherStuff { get; private set; }

        public override string GetScreenshotPath(string code)
        {
            throw new NotImplementedException();
        }

        public override string GetUniqueCode()
        {
            throw new NotImplementedException();
        }

        public override void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback)
        {
            throw new NotImplementedException();
        }

        public override void LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback callback)
        {
            throw new NotImplementedException();
        }

        public override void Save(string code, string data)
        {
            throw new NotImplementedException();
        }

        public override void SaveScreenshot(string code, byte[] screenshot)
        {
            throw new NotImplementedException();
        }

        public override void Delete(string code)
        {
            throw new NotImplementedException();
        }


    }
}
