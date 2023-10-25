using Editarrr.Misc;
using Editarrr.Systems;
using Editarrr.UI;
using UI;
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
        [field: SerializeField] public Tilemap Tilemap_Foreground { get; private set; }
        [field: SerializeField] public Tilemap Tilemap_Background { get; private set; }
        [field: SerializeField] public Canvas ModalCanvas { get; private set; }
        [field: SerializeField] public ModalPopup StartModal { get; private set; }
        [field: SerializeField] public ModalPopup InvalidModal { get; private set; }

        protected override void PreAwake()
        {
            this.Manager.SetScreenshotCamera(this.ScreenshotCamera);
            this.Manager.SetSceneCamera(this.Camera);
            this.Manager.SetTilemap_Foreground(this.Tilemap_Foreground);
            this.Manager.SetTilemap_Background(this.Tilemap_Background);
            this.Manager.SetCanvas(this.ModalCanvas);
            this.Manager.SetStartModal(this.StartModal);
            this.Manager.SetInvalidModal(this.InvalidModal);
        }
    }
}
