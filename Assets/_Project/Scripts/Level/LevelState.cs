using Editarrr.LevelEditor;
using Editarrr.Misc;
using System;
using UnityEngine;

namespace Editarrr.Level
{
    [Serializable]
    public class LevelState
    {
        public string Creator { get; private set; } = "";
        public string CreatorName { get; private set; } = "";
        public bool Published { get; private set; }

        public string Code { get; private set; }
        public bool WasFinished { get; private set; }

        public TileState[,] Tiles { get; set; }

        public int ScaleX { get; private set; }
        public int ScaleY { get; private set; }

        public LevelState(int scaleX, int scaleY)
        {
            this.SetScale(scaleX, scaleY);
        }

        public LevelState(LevelSave levelSave)
        {
            this.Creator = levelSave.Creator;
            this.CreatorName = levelSave.CreatorName;
            this.Published = levelSave.Published;
            this.Code = levelSave.Code;


            this.ScaleX = levelSave.ScaleX;
            this.ScaleY = levelSave.ScaleY;

            this.Tiles = new TileState[this.ScaleX, this.ScaleY];

            foreach (var tileSave in levelSave.Tiles)
            {
                this.Tiles[tileSave.X, tileSave.Y] = new TileState(tileSave);
            }

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    if (this.Tiles[x, y] != null)
                        continue;

                    TileType foreground = TileType.Empty;
                    TileType background = TileType.Empty;
                    Rotation foregroundRotation = Rotation.North;
                    Rotation backgroundRotation = Rotation.North;
                    TileConfig config = null;

                    this.Tiles[x, y] = new TileState(foreground, background, foregroundRotation, backgroundRotation, config);
                }
            }
        }


        public void SetScale(int x, int y)
        {
            this.ScaleX = x;
            this.ScaleY = y;

            this.Tiles = new TileState[this.ScaleX, this.ScaleY];
        }

        public void SetTiles(EditorTileState[,] editorTiles)
        {
            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    EditorTileState editorTileState = editorTiles[x, y];
                    TileType foreground = TileType.Empty;
                    TileType background = TileType.Empty;
                    Rotation foregroundRotation = Rotation.North;
                    Rotation backgroundRotation = Rotation.North;
                    TileConfig config = null;

                    if (editorTileState != null)
                    {
                        foreground = editorTileState.Foreground?.Tile.Type ?? TileType.Empty;
                        background = editorTileState.Background?.Tile.Type ?? TileType.Empty;
                        foregroundRotation = editorTileState.ForegroundRotation;
                        backgroundRotation = editorTileState.BackgroundRotation;
                        config = editorTileState.Config;
                    }

                    this.Tiles[x, y] = new TileState(foreground, background, foregroundRotation, backgroundRotation, config);
                }
            }
        }

        public void SetCode(string code)
        {
            this.Code = code;
        }

        public void SetCreator(string creator, string creatorName)
        {
            this.Creator = creator;
            this.CreatorName = creatorName;
        }

        /**
         * This method should only be called to create a brand new save,
         * never to update and existing save file.
         */
        public LevelSave CreateSave()
        {
            return new LevelSave(this);
        }
        
        /**
         * Check if the level has everything required to be saved or uploaded.
         */
        public bool IsLevelValid()
        {
            bool hasPlayer = false;
            bool hasGoal = false;

            foreach (TileState tile in this.Tiles)
            {
                if (tile.Foreground == TileType.Player)
                {
                    hasPlayer = true;
                }
                else if (tile.Foreground == TileType.Goal)
                {
                    hasGoal = true;
                }

                if (hasPlayer && hasGoal)
                {
                    return true;
                }
            }
            
            return false;
        }
    }

}
