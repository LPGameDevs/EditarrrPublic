using Editarrr.Misc;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    public class EditorHoverTile : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }


        public void Set(EditorTileData editorTileData, Rotation rotation)
        {
            if (editorTileData == null)
            {
                this.SetActive(false);
                return;
            }

            this.SpriteRenderer.sprite = editorTileData.UISprite;
            float degree = 0;

            if (editorTileData.Tile.CanRotate)
                degree = rotation.ToDegree();

            this.SpriteRenderer.transform.localEulerAngles = degree * Vector3.forward;
        }

        public void SetActive(bool value)
        {
            this.SpriteRenderer.enabled = value;
        }
    }
}
