using Editarrr.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Data", menuName = "Data/Editor/new Editor Tile Data")]
    public class EditorTileData : ScriptableObject
    {
        private const string Documentation = "Editor Tiles are a represenation of something that can be placed in the Level Editor.\r\n" +
                                             "It contains information necessary for level editing, not level playing.\r\n" +
                                             "Any property that is necessary for both editing and playing should be stored on TileData.\r\n";

        [field: SerializeField, Info(Documentation)] public Sprite UISprite { get; private set; }

        [field: SerializeField] public TileBase EditorGridTile { get; private set; }

        [field: SerializeField] public TileData Tile { get; private set; }


        [field: SerializeField] public int LevelLimit { get; private set; }
        public bool IsInfinite { get => this.LevelLimit <= 0; }

        [field: SerializeField] public bool IsBackground { get; private set; }
    }
}
