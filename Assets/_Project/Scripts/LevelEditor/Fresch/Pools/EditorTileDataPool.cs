using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Data Pool", menuName = "Pool/Editor/new Editor Tile Data Pool")]
    public class EditorTileDataPool : ScriptableObject
    {
        [field: SerializeField] public EditorTileData[] TileData { get; private set; }

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
