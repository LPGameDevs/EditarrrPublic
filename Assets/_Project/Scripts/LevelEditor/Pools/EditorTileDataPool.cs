using System.Runtime.InteropServices;
using Editarrr.Misc;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Data Pool", menuName = "Pool/Editor/new Editor Tile Data Pool")]
    public class EditorTileDataPool : ScriptableObject
    {
        private const string Documentation = "This pool needs to contain all tiles that are editor selectable.\r\n" +
                                             "If a tile is missing from here it cannot be placed.\r\n" +
                                             "This includes both tiles that can be placed using the editor as well as other " +
                                             "tiles that can be placed by game logic.";
        [field: SerializeField, Info(Documentation)] public EditorTileData[] TileData { get; private set; }

        public EditorTileData Get(TileType tileType)
        {
            if (tileType == TileType.Empty)
                return null;

            int length = this.TileData.Length;

            for (int i = 0; i < length; i++)
            {
                EditorTileData editorTileData = this.TileData[i];

                if (editorTileData.Tile.Type == tileType)
                    return editorTileData;
            }

            return null;
        }
    }
}
