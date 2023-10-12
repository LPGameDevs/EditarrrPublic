using Editarrr.Level;
using Editarrr.LevelEditor;
using Editarrr.Misc;
using Editarrr.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.Level
{
    [System.Serializable]
    public class TileSave
    {
        [field: SerializeField] public int X { get; private set; }
        [field: SerializeField] public int Y { get; private set; }
        [field: SerializeField] public TileType Foreground { get; private set; }
        [field: SerializeField] public Rotation ForegroundRotation { get; private set; }

        [field: SerializeField] public TileType Background { get; private set; }
        [field: SerializeField] public Rotation BackgroundRotation { get; private set; }

        public TileSave(int x, int y, TileState tileState)
        {
            this.X = x;
            this.Y = y;
            this.Foreground = tileState.Foreground;
            this.Background = tileState.Background;
            this.ForegroundRotation = tileState.ForegroundRotation;
            this.BackgroundRotation = tileState.BackgroundRotation;
        }
    }
}
