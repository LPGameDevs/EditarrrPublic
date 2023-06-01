using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Editarrr.LevelEditor.TileData;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "LevelPlayManager", menuName = "Managers/Level/new Level Play Manager")]
    public class LevelPlayManager : ManagerComponent
    {
        private const string Documentation = "This manager will build a level and instanciate its prefabs.\r\n";

        [field: SerializeField, Info(Documentation)] private EditorLevelExchange Exchange { get; set; }

        [field: SerializeField, Header("Managers")] private LevelManager LevelManager { get; set; }


        [field: SerializeField, Header("Pools")] private EditorTileDataPool EditorTileDataPool { get; set; }

        private Tilemap _walls, _damage;

        public void SetTilemapWalls(Tilemap walls)
        {
            _walls = walls;
        }

        public void SetTilemapDamage(Tilemap damage)
        {
            _damage = damage;
        }

        public override void DoStart()
        {
            string code = Exchange.CodeToLoad;
            LevelManager.Load(code, OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelState levelState)
        {
            PaintTilesFromFile(levelState);
        }

        private void PaintTilesFromFile(LevelState level)
        {
            for (int y = 0; y < level.ScaleY; y++)
            {
                for (int x = 0; x < level.ScaleX; x++)
                {
                    TileState tileState = level.Tiles[x, y];

                    if (tileState == null || tileState.Type == TileType.Empty)
                    {
                        continue;
                    }

                    TileData tile = GetTileDataFromType(tileState.Type);
                    Vector3Int position = new Vector3Int(x, y);

                    PlaceTile(tile, position);
                    InstantiateTile(tile, position);
                }
            }
        }

        private TileData GetTileDataFromType(TileType tileStateType)
         {
             EditorTileData tileData = EditorTileDataPool.Get(tileStateType);
             return tileData.Tile;
         }

        private void InstantiateTile(TileData tileData, Vector3Int position)
        {
            if (!tileData.GameObject)
            {
                return;
            }

            Instantiate(tileData.GameObject, position + new Vector3(0.5f, 0.5f, 0),
                Quaternion.identity);
        }

        private void PlaceTile(TileData tileData, Vector3Int position)
        {
            if (!tileData.TileMapTileBase)
            {
                return;
            }

            SetTile(position, tileData.TileMapTileBase);
        }

         private void SetTile(Vector3Int position, TileBase tile)
         {

             _walls.SetTile(position, tile);
         }
    }
}