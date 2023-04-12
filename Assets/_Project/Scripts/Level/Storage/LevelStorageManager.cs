using UnityEngine;

namespace Editarrr.Level
{
    public abstract class LevelStorageManager : ScriptableObject, ILevelStorage
    {
        public abstract void Save(string code, string data);
        public abstract void SaveScreenshot(string code, byte[] screenshot);
        public abstract string GetUniqueCode();
        public abstract void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback);
    }

    //public class LevelManagerSystem : SystemComponent<LevelManager>
    //{

    //}
}
