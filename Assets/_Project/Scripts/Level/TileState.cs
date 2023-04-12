using Editarrr.LevelEditor;
using Editarrr.Misc;
using System;
using UnityEngine;

namespace Editarrr.Level
{
    public class TileState
    {
        public TileType Type { get; private set; }
        public Rotation Rotation { get; private set; }

        public TileState(TileType type, Rotation rotation)
        {
            this.Type = type;
            this.Rotation = rotation;
        }

        public TileState(TileSave tileSave)
        {
            this.Type = tileSave.Type;
            this.Rotation = tileSave.Rotation;
        }

        public TileSave CreateSave(int x, int y)
        {
            return new TileSave(x, y, this);
        }
    }
}
