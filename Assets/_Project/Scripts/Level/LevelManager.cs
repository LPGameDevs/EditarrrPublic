using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Systems;
using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "Level Manager", menuName = "Managers/Level/new Level Manager")]
    public class LevelManager : ManagerComponent
    {
        [field: SerializeField, Header("Level")] public LevelState LevelState { get; private set; }


        [field: SerializeField, Header("Storage")] public LevelStorageManager LevelStorage { get; private set; }

        LevelManager_LevelLoadedCallback LevelLoadedCallback { get; set; }

        public void Create(int scaleX, int scaleY)
        {
            this.LevelState = new LevelState(scaleX, scaleY);

            string code = this.LevelStorage.GetUniqueCode();

            this.LevelState.SetCode(code);
        }

        public void Load(string code, LevelManager_LevelLoadedCallback loadedCallback)
        {
            this.LevelLoadedCallback = loadedCallback;

            this.LevelStorage.LoadLevelData(code, this.LevelStorage_LevelLoadedCallback);
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
            this.LevelState = new LevelState(levelSave);

            this.LevelLoadedCallback?.Invoke(this.LevelState);
        }

    }

    public delegate void LevelManager_LevelLoadedCallback(LevelState levelState);
}
