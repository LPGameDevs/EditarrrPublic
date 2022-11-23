using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelEditor
{
    [Serializable]
    public class LevelSave
    {
        public string creator = "";
        public bool published = false;
        public List<Vector3Int> platformTiles = new List<Vector3Int>();
        public List<Vector3Int> groundTiles = new List<Vector3Int>();
        public List<Vector3Int> wallTiles = new List<Vector3Int>();
        public List<Vector3Int> spikeTiles = new List<Vector3Int>();
        public List<Vector3Int> enemyTiles = new List<Vector3Int>();
        public List<Vector3Int> enemyFollowTiles = new List<Vector3Int>();
        public List<Vector3Int> movingPlatformTiles = new List<Vector3Int>();
        public List<Vector3Int> movingPlatformDeathTiles = new List<Vector3Int>();
        // @todo make serializable
        public List<TileOptions> optionTilesOptions = new List<TileOptions>();
        public Vector3Int playerSpawn;
        public Vector3Int playerWin;

        public void StorePlacedTile(IFrameSelectable tileSO, Vector3Int position, TileOptions options = null)
        {
            string tileName = tileSO.getName();
            RemovePlacedTile(tileSO, position, options);

            if (tileName == "PlayerSpawn")
            {
                playerSpawn = position;
                return;
            }
            if (tileName == "Chest")
            {
                playerWin = position;
                return;
            }

            var placeLayer = GetPlaceLayerFromTile(tileName);
            placeLayer.Add(position);

            if (options != null)
            {
                RemoveTileOption(position);
                options.position = position;
                optionTilesOptions.Add(options);
            }
        }

        public void RemovePlacedTile(IFrameSelectable tileSO, Vector3Int position, TileOptions options = null)
        {
            string tileName = tileSO.getName();

            if (tileName == "PlayerSpawn")
            {
                playerSpawn = Vector3Int.zero;
                return;
            }

            var removeLayers = GetRemoveLayersFromTile(tileName);
            foreach (var removeLayer in removeLayers)
            {
                if (removeLayer.Contains(position))
                {
                    removeLayer.Remove(position);
                }
            }

            if (options != null)
            {
                RemoveTileOption(position);
            }
        }

        public TileOptions GetTileOptions(Vector3Int position)
        {
            return optionTilesOptions.FirstOrDefault(i => i.position == position);
        }

        private void RemoveTileOption(Vector3Int position)
        {
            var item = optionTilesOptions.SingleOrDefault(x => x.position == position);
            if (item != null)
            {
                optionTilesOptions.Remove(item);
            }
        }

        private List<Vector3Int>[] GetRemoveLayersFromTile(string tileName)
        {
            List<Vector3Int>[] backgroundLayers = {
                wallTiles,
                groundTiles,
                spikeTiles,
                movingPlatformTiles,
                movingPlatformDeathTiles
            };
            string[] backgroundLayerNames = {
                "Background",
                "Wall",
                "Spikes",
                "MovingPlatform",
                "MovingPlatformDeath"
            };

            if (backgroundLayerNames.Contains(tileName))
            {
                return backgroundLayers;
            }

            if (tileName == "Enemy")
            {
                return new[] {
                    enemyTiles
                };
            }

            if (tileName == "EnemyFollow")
            {
                return new[] {
                    enemyFollowTiles
                };
            }


            return new[] {
                platformTiles
            };
        }

        private List<Vector3Int> GetPlaceLayerFromTile(string tileName)
        {
            if (tileName == "Background")
            {
                return groundTiles;
            }
            if (tileName == "Wall")
            {
                return wallTiles;
            }
            if (tileName == "Spikes")
            {
                return spikeTiles;
            }
            if (tileName == "Enemy")
            {
                return enemyTiles;
            }
            if (tileName == "EnemyFollow")
            {
                return enemyFollowTiles;
            }
            if (tileName == "MovingPlatform")
            {
                return movingPlatformTiles;
            }
            if (tileName == "MovingPlatformDeath")
            {
                return movingPlatformDeathTiles;
            }

            return platformTiles;
        }


        public void ShiftTiles()
        {
            platformTiles = platformTiles.Select(t => {t += new Vector3Int(1,1,0); return t;}).ToList();
            groundTiles = groundTiles.Select(t => {t += new Vector3Int(1,1,0); return t;}).ToList();
            wallTiles = wallTiles.Select(t => {t += new Vector3Int(1,1,0); return t;}).ToList();
            spikeTiles = spikeTiles.Select(t => {t += new Vector3Int(1,1,0); return t;}).ToList();
            enemyTiles = enemyTiles.Select(t => {t += new Vector3Int(1,1,0); return t;}).ToList();
            enemyFollowTiles = enemyFollowTiles.Select(t => {t += new Vector3Int(1,1,0); return t;}).ToList();
            movingPlatformTiles = movingPlatformTiles.Select(t => {t += new Vector3Int(1,1,0); return t;}).ToList();
            movingPlatformDeathTiles = movingPlatformDeathTiles.Select(t => {t += new Vector3Int(1,1,0); return t;}).ToList();
            optionTilesOptions = optionTilesOptions.Select(t => {t.position += new Vector3Int(1,1,0); return t;}).ToList();
            playerSpawn += new Vector3Int(1,1,0);
            playerWin += new Vector3Int(1,1,0);
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
}
