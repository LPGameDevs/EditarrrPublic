using Editarrr.Misc;

namespace Editarrr.LevelEditor
{
    public class EditorTileState
    {
        public EditorTileData TileData { get; private set; }
        public Rotation Rotation { get; private set; }

        public EditorTileState(EditorTileData tileData, Rotation rotation)
        {
            this.TileData = tileData;
            this.Rotation = rotation;
        }
    }
}
