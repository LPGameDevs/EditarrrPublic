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
        public void UploadScreenshot(string code, byte[] screenshot);
        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback);
        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback);
    }

    public delegate void RemoteLevelStorage_LevelUploadedCallback(string code, ulong remoteId, bool isSteam = false);
    public delegate void RemoteLevelStorage_LevelLoadedCallback(LevelSave levelsave);
    public delegate void RemoteLevelStorage_AllLevelsLoadedCallback(LevelStub[] levelStubs);

    [Serializable]
    public class LevelStub
    {
        public string Code { get; private set; }
        public string Creator { get; private set; }
        public bool Published { get; private set; }

        public LevelStub(string code, string creator, bool published = false)
        {
            this.Code = code;
            this.Creator = creator;
            this.Published = published;
        }
    }
}
