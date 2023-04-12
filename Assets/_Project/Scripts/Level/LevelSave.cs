using Editarrr.LevelEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.Level
{
    /// <summary>
    /// This class is used to serialize a <c>LevelState</c>
    /// </summary>
    [Serializable]
    public class LevelSave
    {
        [field: SerializeField] public string Creator { get; private set; }
        [field: SerializeField] public bool Published { get; private set; }
        [field: SerializeField] public string Code { get; private set; }

        [field: SerializeField] public TileSave[] Tiles { get; private set; }

        [field: SerializeField] public int ScaleX { get; private set; }
        [field: SerializeField] public int ScaleY { get; private set; }

        public LevelSave(LevelState levelState)
        {
            this.Creator = levelState.Creator;
            this.Published = levelState.Published;
            this.Code = levelState.Code;

            this.ScaleX = levelState.Tiles.GetLength(0);
            this.ScaleY = levelState.Tiles.GetLength(1);

            List<TileSave> tiles = new List<TileSave>();

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    TileState tile = levelState.Tiles[x, y];

                    if (tile.Type != TileType.Empty)
                    {
                        tiles.Add(tile.CreateSave(x, y));
                    }

                    // this.Tiles[index] = levelState.Tiles[x, y].CreateSave();

                    //index++;
                }
            }

            this.Tiles = tiles.ToArray();
        }
    }

}
