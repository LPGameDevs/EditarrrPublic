using Editarrr.Input;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using Gameplay.GUI;
using Systems;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Editarrr.LevelEditor.TileData;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "LevelPlayManager", menuName = "Managers/Level/new Level Play Manager")]
    public class LevelPlayManager : ManagerComponent
    {
        private const string Documentation = "This manager will build a level and instanciate its prefabs.\r\n";

        #region Properties

        [field: SerializeField, Info(Documentation)] private EditorLevelExchange Exchange { get; set; }

        [field: SerializeField, Header("Managers")] public LevelManager LevelManager { get; private set; }

        [field: SerializeField, Header("Settings")] public EditorLevelSettings Settings { get; private set; }

        [field: SerializeField, Header("Pools")] private EditorTileDataPool EditorTileDataPool { get; set; }

        private Tilemap _walls, _damage;
        private GameplayGuiManager _gameplayGuiManager;

        public override void DoAwake()
        {
            LevelManager.DoAwake();
        }

        public void SetTilemapWalls(Tilemap walls)
        {
            _walls = walls;
        }

        public void SetTilemapDamage(Tilemap damage)
        {
            _damage = damage;
        }

        public void SetGuiManager(GameplayGuiManager gameplayGuiManager)
        {
            _gameplayGuiManager = gameplayGuiManager;
        }


        #endregion

        public override void DoStart()
        {
            string code = Exchange.CodeToLoad;
            _gameplayGuiManager.SetLevelCode(code);

            LevelManager.Load(code, OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelState levelState)
        {
            PaintTilesFromFile(levelState);
            _gameplayGuiManager.SetLevelState(levelState);
            //GameEvent.Trigger(GameEventType.Unpause);
        }

        #region Tile Operations

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
                    InstantiateTile(tile, position, tileState.Rotation);
                }
            }
        }

        private TileData GetTileDataFromType(TileType tileStateType)
        {
            EditorTileData tileData = EditorTileDataPool.Get(tileStateType);
            return tileData.Tile;
        }

        private void InstantiateTile(TileData tileData, Vector3Int position, Rotation rotation)
        {
            if (!tileData.GameObject)
            {
                return;
            }

            Instantiate(tileData.GameObject, position + new Vector3(0.5f, 0.5f, 0),
                Quaternion.Euler(0, 0, rotation.ToDegree()));
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


            //if (this.ActiveEditorTileData.Tile.CanRotate)
            //    rotate = this.EditorTileSelectionManager.Rotation.ToDegree();
        }

        #endregion


        private void OnScoreSubmitRequested(string code, float time)
        {
            LevelManager.LevelStorage.LoadLevelData(code, ScoreLevelLoaded);

            void ScoreLevelLoaded(LevelSave levelSave)
            {
                LevelManager.SubmitScore(time, levelSave, ScoreSubmitted);

                void ScoreSubmitted(string code, string remoteid, bool issteam)
                {
                    // @todo do we need this?
                }
            }
        }

        public override void DoOnEnable()
        {
            WinMenu.OnScoreSubmit += OnScoreSubmitRequested;
        }

        public override void DoOnDisable()
        {
            WinMenu.OnScoreSubmit -= OnScoreSubmitRequested;
        }
    }
}
