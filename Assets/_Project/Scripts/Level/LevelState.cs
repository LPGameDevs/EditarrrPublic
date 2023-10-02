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
        public bool Published { get; private set; }

        public string Code { get; private set; }
        public bool WasFinished { get; private set; }

        public TileState[,] Tiles { get; set; }

        public int ScaleX { get; private set; }
        public int ScaleY { get; private set; }

        public LevelState(int scaleX, int scaleY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;

            this.Tiles = new TileState[this.ScaleX, this.ScaleY];
        }

        public LevelState(LevelSave levelSave)
        {
            this.Creator = levelSave.Creator;
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

                    TileType tileType = TileType.Empty;
                    Rotation rotation = Rotation.North;

                    this.Tiles[x, y] = new TileState(tileType, rotation);
                }
            }
        }


        public void SetTiles(EditorTileState[,] editorTiles)
        {
            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    EditorTileState editorTileState = editorTiles[x, y];
                    TileType tileType = TileType.Empty;
                    Rotation rotation = Rotation.North;

                    if (editorTileState != null)
                    {
                        tileType = editorTileState.TileData.Tile.Type;
                        rotation = editorTileState.Rotation;
                    }

                    this.Tiles[x, y] = new TileState(tileType, rotation);
                }
            }
        }

        public void SetCode(string code)
        {
            this.Code = code;
        }

        public void SetCreator(string creator)
        {
            this.Creator = creator;
        }

        /**
         * This method should only be called to create a brand new save,
         * never to update and existing save file.
         */
        public LevelSave CreateSave()
        {
            return new LevelSave(this);
        }
    }

}
