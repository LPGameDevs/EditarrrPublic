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
        public bool SupportsLeaderboards();
        public void SubmitScore(float score, LevelSave levelSave, RemoteScoreStorage_ScoreSubmittedCallback callback);
        public void GetScoresForLevel(string code, RemoteScoreStorage_AllScoresLoadedCallback callback);
    }
}
