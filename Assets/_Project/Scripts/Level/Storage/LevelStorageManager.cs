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

    public class DatabaseLevelStorageManager : LevelStorageManager
    {
        /// <summary>
        /// Help, I do not know what I am doing!
        ///
        /// @todo Find help...
        /// </summary>
        [field: SerializeField] public string ConnectionString { get; private set; }
        [field: SerializeField] public string OtherStuff { get; private set; }

        public override string GetUniqueCode()
        {
            throw new System.NotImplementedException();
        }

        public override void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public override void Save(string code, string data)
        {
            throw new System.NotImplementedException();
        }

        public override void SaveScreenshot(string code, byte[] screenshot)
        {
            throw new System.NotImplementedException();
        }
    }
    //public class LevelManagerSystem : SystemComponent<LevelManager>
    //{

    //}
}
