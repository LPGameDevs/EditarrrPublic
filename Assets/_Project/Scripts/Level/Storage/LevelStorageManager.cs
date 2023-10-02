using UnityEngine;

namespace Level.Storage
{
    public abstract class LevelStorageManager : ScriptableObject, ILevelStorage
    {
        public abstract void Save(string code, string data);
        public abstract void SaveScreenshot(string code, byte[] screenshot);
        public abstract string GetScreenshotPath(string code);
        public abstract bool LevelExists(string code);
        public abstract void CopyLevelFromDirectory(string code, string sourceDirectory);
        public abstract string GetLevelPath(string code);
        public abstract string GetUniqueCode();
        public abstract void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback);
        public abstract void LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback callback);
        public abstract void Delete(string code);

        // This is optional depending on if it is required.
        public virtual void Initialize()
        {

        }
    }

}
