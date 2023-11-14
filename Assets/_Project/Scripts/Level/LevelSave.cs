using Editarrr.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Editarrr.Misc;
using Singletons;
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
        [field: SerializeField] public string CreatorName { get; private set; }
        [field: SerializeField] public bool Published { get; private set; }
        [field: SerializeField] public string Code { get; private set; }

        [field: SerializeField] public TileSave[] Tiles { get; private set; } = new TileSave[0];

        [field: SerializeField] private List<string> Labels { get; set; } = new List<string>();
        [field: SerializeField] public int ScaleX { get; private set; }
        [field: SerializeField] public int ScaleY { get; private set; }
        [field: SerializeField] public string LocalDirectory { get; private set; }
        [field: SerializeField] public string RemoteId { get; private set; } = "";
        [field: SerializeField] public ulong SteamId { get; private set; }
        [field: SerializeField] public int TotalRatings { get; private set; } = 0;
        [field: SerializeField] public int TotalScores { get; private set; } = 0;
        [field: SerializeField] public int Version { get; private set; } = 0;
        [field: SerializeField] public bool Completed { get; private set; } = false;

        public LevelSave(string creator, string creatorName,  string code)
        {
            this.Creator = creator;
            this.CreatorName = creatorName;
            this.Published = false;
            this.Code = code;
        }

        /**
         * Only LevelState::CreateSave() should call this constructor.
         */
        public LevelSave(LevelState levelState)
        {
            this.Creator = levelState.Creator;
            this.CreatorName = levelState.CreatorName;
            this.Published = levelState.Published;
            this.Code = levelState.Code;

            this.ScaleX = levelState.Tiles.GetLength(0);
            this.ScaleY = levelState.Tiles.GetLength(1);

            UserTagType tagType = PreferencesManager.Instance.GetUserTypeTag();
            if (tagType == UserTagType.GDFG)
            {
                this.SetLabel("GDFG");
            }
            else if (tagType == UserTagType.Stream)
            {
                string streamer = PreferencesManager.Instance.GetStreamerChannel();
                this.SetLabel(streamer);
            }

            List<TileSave> tiles = new List<TileSave>();

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    TileState tile = levelState.Tiles[x, y];

                    // Initialise empty tile if not done already.
                    if (tile == null)
                    {
                        tile = new TileState(TileType.Empty, TileType.Empty, Rotation.North, Rotation.North, null);
                        levelState.Tiles[x, y] = tile;
                    }

                    if (tile.Foreground != TileType.Empty)
                    {
                        tiles.Add(tile.CreateSave(x, y));
                    }

                    // this.Tiles[index] = levelState.Tiles[x, y].CreateSave();

                    //index++;
                }
            }

            this.Tiles = tiles.ToArray();
        }

        public string[] GetLabels()
        {
            return this.Labels.ToArray();
        }

        public void SetLabel(string label)
        {
            if (!this.Labels.Contains(label))
            {
                this.Labels.Add(label);
            }
        }

        public void UnsetLabel(string label)
        {
            if (this.Labels.Contains(label))
            {
                this.Labels.Remove(label);
            }
        }

        public void SetLocalDirectory(string localDirectory)
        {
            this.LocalDirectory = localDirectory;
        }

        public void SetRemoteId(string remoteId)
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

                    // Initialise empty tile if not done already.
                    if (tile == null)
                    {
                        tile = new TileState(TileType.Empty, TileType.Empty, Rotation.North, Rotation.North, null);
                        tileState[x, y] = tile;
                    }

                    if (tile.Foreground != TileType.Empty || tile.Background != TileType.Empty)
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

        public void SetVersion(int version)
        {
            this.Version = version;
            this.Completed = false;
        }

        public void SetTotalRatings(int ratings)
        {
            this.TotalRatings = ratings;
        }

        public void SetTotalScores(int scores)
        {
            this.TotalScores = scores;
        }

        public void MarkAsCompleted()
        {
            this.Completed = true;
        }

        /**
         * Check if the level has everything required to be saved or uploaded.
         */
        public bool IsLevelValid()
        {
            bool hasPlayer = false;
            bool hasGoal = false;

            foreach (var tile in this.Tiles)
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
