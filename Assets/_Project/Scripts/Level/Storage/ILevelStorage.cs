namespace Editarrr.Level
{
    public interface ILevelStorage
    {
        public void Save(string code, string data);
        public void SaveScreenshot(string code, byte[] screenshot);
        public string GetUniqueCode();
        public void LoadLevelData(string code, LevelStorage_LevelLoadedCallback callback);
    }

    public delegate void LevelStorage_LevelLoadedCallback(LevelSave levelSave);

    //public class LevelManagerSystem : SystemComponent<LevelManager>
    //{

    //}
}
