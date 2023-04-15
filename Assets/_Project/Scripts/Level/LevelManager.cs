using System.Collections.Generic;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "Level Manager", menuName = "Managers/Level/new Level Manager")]
    public class LevelManager : ManagerComponent
    {
        private const string Documentation = "The level manager is a wrapper around level storage and creation.\r\n" +
                                             "It chooses a storage manager and delegates create / load / save calls.";

        [field: SerializeField, Info(Documentation)] public LevelState LevelState { get; private set; }

        [field: SerializeField, Header("Settings")] private EditorLevelSettings Settings { get; set; }

        [field: SerializeField, Header("Storage")] public LevelStorageManager LevelStorage { get; private set; }

        LevelManager_LevelLoadedCallback LevelLoadedCallback { get; set; }
        LevelManager_AllLevelsLoadedCallback LevelsLoadedCallback { get; set; }

        public void Create()
        {
            this.LevelState = new LevelState(this.Settings.EditorLevelScaleX, this.Settings.EditorLevelScaleY);

            string code = this.LevelStorage.GetUniqueCode();

            this.LevelState.SetCode(code);

            string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);

            this.LevelState.SetCreator(userName);
        }

        public void Load(string code, LevelManager_LevelLoadedCallback loadedCallback)
        {
            this.LevelLoadedCallback = loadedCallback;

            this.LevelStorage.LoadLevelData(code, this.LevelStorage_LevelLoadedCallback);
        }

        public void LoadAll(LevelManager_AllLevelsLoadedCallback loadedCallback)
        {
            this.LevelsLoadedCallback = loadedCallback;

            this.LevelStorage.LoadAllLevelData(this.LevelStorage_AllLevelsLoadedCallback);
        }

        public void Delete(string code)
        {
            this.LevelStorage.Delete(code);
        }

        public void Save(EditorTileState[,] editorTileData, Texture2D screenshot)
        {
            this.LevelState.SetTiles(editorTileData);

            LevelSave levelSave = this.LevelState.CreateSave();
            string data = JsonUtility.ToJson(levelSave);

            this.LevelStorage.Save(levelSave.Code, data);
            this.SaveScreenshot(levelSave, screenshot);
        }

        private void SaveScreenshot(LevelSave levelSave, Texture2D screenshot)
        {
            byte[] byteArray = screenshot.EncodeToPNG();
            this.LevelStorage.SaveScreenshot(levelSave.Code, byteArray);
        }


        private void LevelStorage_LevelLoadedCallback(LevelSave levelSave)
        {
            if (levelSave == null)
            {
#warning // TODO, Failed to load... >> Load default level, (Display Message???)
                this.Create();
                return;
            }

            this.LevelState = new LevelState(levelSave);

            this.LevelLoadedCallback?.Invoke(this.LevelState);
            this.LevelLoadedCallback = null;
        }

        private void LevelStorage_AllLevelsLoadedCallback(LevelSave[] levelSaves)
        {

            List<LevelState> levels = new List<LevelState>();

            foreach (var levelSave in levelSaves)
            {
                levels.Add(new LevelState(levelSave));
            }

            this.LevelsLoadedCallback?.Invoke(levels.ToArray());
            this.LevelsLoadedCallback = null;
        }

        public string GetScreenshotPath(string levelCode)
        {
            return this.LevelStorage.GetScreenshotPath(levelCode);
        }
    }

    public delegate void LevelManager_LevelLoadedCallback(LevelState levelState);
    public delegate void LevelManager_AllLevelsLoadedCallback(LevelState[] levelStates);
}
