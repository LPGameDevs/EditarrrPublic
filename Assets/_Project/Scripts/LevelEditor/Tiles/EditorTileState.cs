using Editarrr.Misc;
using System.Diagnostics;
using System;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    public class EditorTileState
    {
        public EditorTileData Foreground { get; private set; }
        public EditorTileData Background { get; private set; }
        public Rotation ForegroundRotation { get; private set; }
        public Rotation BackgroundRotation { get; private set; }
        public TileConfig Config { get; private set; }

        //public EditorTileState(EditorTileData foreground, EditorTileData background, Rotation rotation)
        //{
        //    this.Foreground = foreground;
        //    this.Background = background;
        //    this.Rotation = rotation;
        //}

        public void SetForeground(EditorTileData foreground)
        {
            this.Foreground = foreground;
        }

        public void SetBackground(EditorTileData background)
        {
            this.Background = background;
        }

        public void SetForegroundRotation(Rotation rotation)
        {
            this.ForegroundRotation = rotation;
        }

        public void SetBackgroundRotation(Rotation rotation)
        {
            this.BackgroundRotation = rotation;
        }

        public void SetConfig(TileConfig config)
        {
            this.Config = config;
        }
    }
}
