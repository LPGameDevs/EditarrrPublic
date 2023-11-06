using System;
using Editarrr.Level;

namespace Level.Storage
{
    public interface IRemoteLevelStorage
    {
        /**
         * Optional setup tasks for the storage mechanism.
         */
        public void Initialize();
        public void Upload(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback);
        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback);
        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback);
    }

    public delegate void RemoteRatingStorage_RatingSubmittedCallback(string code, string remoteId, bool isSteam = false);
    public delegate void RemoteRatingStorage_AllRatingsLoadedCallback(RatingStub[] ratingStubs);
    public delegate void RemoteScoreStorage_ScoreSubmittedCallback(string code, string remoteId, bool isSteam = false);
    public delegate void RemoteScoreStorage_AllScoresLoadedCallback(ScoreStub[] scoreStubs);
    public delegate void RemoteLevelStorage_LevelUploadedCallback(string code, string remoteId, bool isSteam = false);
    public delegate void RemoteLevelStorage_LevelLoadedCallback(LevelSave levelsave);
    public delegate void RemoteLevelStorage_LevelScreenshotDownloadedCallback();
    public delegate void RemoteLevelStorage_AllLevelsLoadedCallback(LevelStub[] levelStubs);

    [Serializable]
    public class LevelStub
    {
        public string Code { get; private set; }
        public string Creator { get; private set; }
        public string CreatorName { get; private set; }
        public string RemoteId { get; private set; }
        public bool Published { get; private set; }
        public int TotalRatings { get; private set; } = 0;
        public int TotalScores { get; private set; } = 0;
        public bool IsDistro { get; private set; }

        public LevelStub(string code, string creator, string creatorName, string remoteId = "", bool published = false)
        {
            this.Code = code;
            this.Creator = creator;
            this.CreatorName = creatorName;
            this.RemoteId = remoteId;
            this.Published = published;
            this.IsDistro = false;
        }

        public void SetDistro(bool isDistro)
        {
            this.IsDistro = isDistro;
        }

        public void SetTotalRatings(int ratings)
        {
            this.TotalRatings = ratings;
        }

        public void SetTotalScores(int scores)
        {
            this.TotalScores = scores;
        }
    }

    [Serializable]
    public class ScoreStub
    {
        public string Code { get; private set; }
        public string Creator { get; private set; }
        public string CreatorName { get; private set; }
        public float Score { get; private set; }

        public ScoreStub(string code, string creator, string creatorName, float score)
        {
            this.Code = code;
            this.Creator = creator;
            this.CreatorName = creatorName;
            this.Score = score;
        }
    }

    [Serializable]
    public class RatingStub
    {
        public string Code { get; private set; }
        public string Creator { get; private set; }
        public string CreatorName { get; private set; }
        public int Rating { get; private set; }

        public RatingStub(string code, string creator, string creatorName, int rating)
        {
            this.Code = code;
            this.Creator = creator;
            this.CreatorName = creatorName;
            this.Rating = rating;
        }
    }
}
