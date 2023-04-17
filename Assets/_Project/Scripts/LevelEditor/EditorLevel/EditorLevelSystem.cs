using Editarrr.Misc;
using Editarrr.Systems;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    public class EditorLevelSystem : SystemComponent<EditorLevelManager>
    {
        private const string Documentation = "This System is responsible for adding an EditorLevelManager to the Scene.\r\n" +
                                             "It is also for responsible for passing Game Objects like the camera or the tilemap to the EditorLevelManager.\r\n" +
                                             "It will then handle all of the required logic for managing the level editor features.";

        [field: SerializeField, Info(Documentation)] public Camera ScreenshotCamera { get; private set; }
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public Tilemap Tilemap { get; private set; }

        protected override void PreAwake()
        {
            this.Manager.SetScreenshotCamera(this.ScreenshotCamera);
            this.Manager.SetSceneCamera(this.Camera);
            this.Manager.SetTilemap(this.Tilemap);
        }
    }
}
