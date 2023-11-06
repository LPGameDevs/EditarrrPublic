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
        public void DownloadScreenshot(string code, RemoteLevelStorage_LevelScreenshotDownloadedCallback callback);
        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback, RemoteLevelLoadQuery? query = null);
        public bool SupportsLeaderboards();
        public void SubmitScore(float score, LevelSave levelSave, RemoteScoreStorage_ScoreSubmittedCallback callback);
        public void GetScoresForLevel(string code, RemoteScoreStorage_AllScoresLoadedCallback callback);
        public void SubmitRating(int rating, LevelSave levelSave, RemoteRatingStorage_RatingSubmittedCallback callback);
        public void GetRatingsForLevel(string code, RemoteRatingStorage_AllRatingsLoadedCallback callback);
    }
}
