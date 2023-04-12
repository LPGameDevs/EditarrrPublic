using Editarrr.Systems;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    public class EditorLevelSystem : SystemComponent<EditorLevelManager>
    {
        [field: SerializeField] public Camera ScreenshotCamera { get; private set; }
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