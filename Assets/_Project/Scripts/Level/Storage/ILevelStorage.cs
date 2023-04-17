namespace Editarrr.Level
{
    public interface ILevelStorage
    {
        /**
         * Optional setup tasks for the storage mechanism.
         */
        public void Initialize();
        public void Save(string code, string data);
        public void SaveScreenshot(string code, byte[] screenshot);
        public string GetUniqueCode();
        public void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback);
        public void LoadAllLevelData(LevelStorage_AllLevelsLoadedCallback callback);
    }

    public delegate void LevelStorage_LevelLoadedCallback(LevelSave levelSave);
    public delegate void LevelStorage_AllLevelsLoadedCallback(LevelSave[] levelSaves);

}
