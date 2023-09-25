namespace Level.Storage
{
    public interface IRemoteLevelStorageProvider
    {
        /**
         * Optional setup tasks for the storage mechanism.
         */
        public void Initialize();
        public void Upload(string code);
        public void Download(string code);
        public void LoadLevelData(string code);
        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback);
        bool SupportsLeaderboards();
        void SubmitScore();
    }
}
