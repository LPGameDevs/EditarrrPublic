using Editarrr.LevelEditor;
using System;
using System.Collections.Generic;
using Editarrr.Misc;
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

        [field: SerializeField] public TileSave[] Tiles { get; private set; } = new TileSave[0];

        [field: SerializeField] public int ScaleX { get; private set; }
        [field: SerializeField] public int ScaleY { get; private set; }
        [field: SerializeField] public string LocalDirectory { get; private set; }
        [field: SerializeField] public ulong RemoteId { get; private set; }
        [field: SerializeField] public ulong SteamId { get; private set; }

        public LevelSave(string creator, string code)
        {
            this.Creator = creator;
            this.Published = false;
            this.Code = code;
        }

        /**
         * Only LevelState::CreateSave() should call this constructor.
         */
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

                    // Initialise empty tile if not done already.
                    if (tile == null)
                    {
                        tile = new TileState(TileType.Empty, Rotation.North);
                        levelState.Tiles[x, y] = tile;
                    }

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

        public void SetLocalDirectory(string localDirectory)
        {
            this.LocalDirectory = localDirectory;
        }

        public void SetRemoteId(ulong remoteId)
        {
            this.RemoteId = remoteId;
        }

        public void SetSteamId(ulong steamId)
        {
            this.SteamId = steamId;
        }

        public void SetTiles(TileState[,] tileState)
        {
            this.ScaleX = tileState.GetLength(0);
            this.ScaleY = tileState.GetLength(1);

            List<TileSave> tiles = new List<TileSave>();

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    TileState tile = tileState[x, y];

                    if (tile.Type != TileType.Empty)
                    {
                        tiles.Add(tile.CreateSave(x, y));
                    }

                    // this.Tiles[index] = tileState[x, y].CreateSave();

                    //index++;
                }
            }

            this.Tiles = tiles.ToArray();
        }

        public void SetPublished(bool levelStatePublished)
        {
            this.Published = levelStatePublished;
        }
    }

}
