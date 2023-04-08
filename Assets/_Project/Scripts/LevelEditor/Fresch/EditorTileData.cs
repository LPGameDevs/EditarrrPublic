using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Data", menuName = "Data/Editor/new Editor Tile Data")]
    public class EditorTileData : ScriptableObject
    {
        [field: SerializeField] public Sprite UISprite { get; private set; }

        [field: SerializeField] public TileBase EditorGridTile { get; private set; }

        [field: SerializeField] public TileData Tile { get; private set; }


        [field: SerializeField] public int LevelLimit { get; private set; }
        public bool IsInfinite { get => this.LevelLimit <= 0; }
    }
}
