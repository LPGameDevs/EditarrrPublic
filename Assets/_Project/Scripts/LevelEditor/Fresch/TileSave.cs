using Editarrr.Misc;
using Editarrr.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [Serializable]
    public class LevelState
    {
        public string Creator { get; private set; } = "";
        public bool Published { get; private set; }

        Dictionary<TileType, HashSet<Int2D>> TileLocations { get; set; } = new Dictionary<TileType, HashSet<Int2D>>();
        public TileState[,] Tiles { get; set; }

        public LevelState()
        {
            this.Tiles = new TileState[100, 50];
        }

        private LevelState(LevelSave levelSaveState)
        {
            this.Creator = levelSaveState.Creator;
            this.Published = levelSaveState.Published;

            int scaleX = levelSaveState.ScaleX;
            int scaleY = levelSaveState.Tiles.Length / scaleX;

            this.Tiles = new TileState[scaleX, scaleY];

            int i = 0;
            for (int y = 0; y < scaleY; y++)
            {
                for (int x = 0; x < scaleX; x++)
                {
                    this.Tiles[x, y] = levelSaveState.Tiles[i];

                    i++;
                }
            }
        }

        public void Place(TileType tileType, int x, int y, TileOption tileOption = null)
        {
            this.Remove(x, y);

            this.Tiles[x, y] = new TileState(tileType, tileOption);

            this.AddLocation(tileType, x, y);
        }

        public void Remove(int x, int y)
        {
            TileState tileState = this.Tiles[x, y];

            if (tileState == null)
                return;

            this.RemoveLocation(tileState.Type, x, y);
        }

        private void AddLocation(TileType tileType, int x, int y)
        {
            if (!this.TileLocations.ContainsKey(tileType))
                this.TileLocations[tileType] = new HashSet<Int2D>();

            this.TileLocations[tileType].Add(new Int2D(x, y));
        }

        private void RemoveLocation(TileType tileType, int x, int y)
        {
            if (!this.TileLocations.ContainsKey(tileType))
                return;

            this.TileLocations[tileType].Remove(new Int2D(x, y));
        }

        public TileState Get(int x, int y)
        {
            return this.Tiles[x, y];
        }


        public void SetPublished(bool value)
        {
            if (this.Published)
                return;

            this.Published = value;
            // Trigger Events?
        }

        public void SetCreator(string creator)
        {
            this.Creator = creator;
        }



        public static LevelState Load(LevelSave levelSaveState)
        {
            return new LevelState(levelSaveState);
        }
    }

    [Serializable]
    public abstract class TileOption
    {
        public abstract void Next();
    }

    [Serializable]
    public class TileOption_MovingPlatform : TileOption
    {
        public int Type { get; private set; } // 0: No Spike, 1: Spike
        public int Direction { get; private set; } // 0: Left, 1: Right

        public override void Next()
        {
            this.Direction = (this.Direction + 1).Loop(2);
        }
    }

    [Serializable]
    public class TileOptions
    {
        public string options;
        public Vector3Int position;

        public TileOptions(string options = "")
        {
            this.options = options;
        }
    }

    /// <summary>
    /// This class is used to serialize a <c>LevelState</c>
    /// </summary>
    [Serializable]
    public class LevelSave
    {
        public string Creator;
        public bool Published;
        public TileState[] Tiles;

        public int ScaleX;

        public LevelSave(LevelState levelState)
        {
            this.Creator = levelState.Creator;
            this.Published = levelState.Published;
            this.ScaleX = levelState.Tiles.GetLength(0);
            int scaleY = levelState.Tiles.GetLength(1);

            this.Tiles = new TileState[scaleY * this.ScaleX];

            int i = 0;
            for (int y = 0; y < scaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    this.Tiles[i] = levelState.Get(x, y);

                    i++;
                }
            }
        }
    }

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

    [Serializable]
    public class TileState
    {
        public TileType Type { get; private set; }
        public TileOption Option { get; private set; }
        public Rotation Rotation { get; private set; }

        public TileState(TileType type, TileOption option)
        {
            this.Type = type;
            this.Option = option;
        }

        public void SetRotation(Rotation rotation)
        {
            this.Rotation = rotation;
        }
    }

    [System.Serializable]
    public class TileSave
    {

    }
}
