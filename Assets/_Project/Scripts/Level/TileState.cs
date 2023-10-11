using Editarrr.LevelEditor;
using Editarrr.Misc;
using System;
using UnityEngine;

namespace Editarrr.Level
{
    public class TileState
    {
        public TileType Foreground { get; private set; }
        public Rotation ForegroundRotation { get; private set; }

        public TileType Background { get; private set; }
        public Rotation BackgroundRotation { get; private set; }

        public TileState(TileType foreground, TileType background, Rotation foregroundRotation, Rotation backgroundRotation)
        {
            this.Foreground = foreground;
            this.Background = background;
            this.ForegroundRotation = foregroundRotation;
            this.BackgroundRotation = backgroundRotation;
        }

        public TileState(TileSave tileSave)
        {
            this.Foreground = tileSave.Foreground;
            this.Background = tileSave.Background;
            this.ForegroundRotation = tileSave.ForegroundRotation;
            this.BackgroundRotation = tileSave.BackgroundRotation;
        }

        public TileSave CreateSave(int x, int y)
        {
            return new TileSave(x, y, this);
        }
    }
}
