using Editarrr.Level;

namespace Level.Storage
{
    public interface IRemoteLevelStorage
    {
        /**
         * Optional setup tasks for the storage mechanism.
         */
        public void Initialize();
        public void Upload(string code, string data);
        public void UploadScreenshot(string code, byte[] screenshot);
        public void Download(string code, string data);
        public void LoadLevelData(string code, RemoteLevelStorage_LevelLoadedCallback callback);
        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback);
    }

    public delegate void RemoteLevelStorage_LevelLoadedCallback(LevelSave levelSave);
    public delegate void RemoteLevelStorage_AllLevelsLoadedCallback(LevelSave[] levelSaves);

}
