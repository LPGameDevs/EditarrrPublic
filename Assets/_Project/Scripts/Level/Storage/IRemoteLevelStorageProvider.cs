using Editarrr.Level;

namespace Level.Storage
{
    public interface IRemoteLevelStorageProvider
    {
        /**
         * Optional setup tasks for the storage mechanism.
         */
        public void Initialize();
        public void Upload(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback);
        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback);
        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback);
        bool SupportsLeaderboards();
        void SubmitScore();
    }
}
