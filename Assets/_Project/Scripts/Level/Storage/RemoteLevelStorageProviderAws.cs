namespace Level.Storage
{
    public class RemoteLevelStorageProviderAws: IRemoteLevelStorageProvider
    {
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Upload(string code)
        {
            throw new System.NotImplementedException();
        }

        public void Download(string code)
        {
            throw new System.NotImplementedException();
        }

        public void LoadLevelData(string code)
        {
            throw new System.NotImplementedException();
        }

        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public bool SupportsLeaderboards()
        {
            return false;
        }

        public void SubmitScore()
        {
            throw new System.NotImplementedException();
        }
    }
}
