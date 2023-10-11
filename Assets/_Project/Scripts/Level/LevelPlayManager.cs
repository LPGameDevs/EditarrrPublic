using Editarrr.Input;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using Gameplay.GUI;
using LevelEditor;
using Singletons;
using Systems;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Editarrr.LevelEditor.TileData;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "LevelPlayManager", menuName = "Managers/Level/new Level Play Manager")]
    public class LevelPlayManager : ManagerComponent
    {
        public static LevelLoading OnLevelLoading { get; set; }
        public delegate void LevelLoading(LevelState levelState);


        private const string Documentation = "This manager will build a level and instanciate its prefabs.\r\n";

        #region Properties
        [field: SerializeField, Info(Documentation)] private EditorLevelExchange Exchange { get; set; }

        [field: SerializeField, Header("Managers")] public LevelManager LevelManager { get; private set; }

        [field: SerializeField, Header("Settings")] public EditorLevelSettings Settings { get; private set; }

        [field: SerializeField, Header("Pools")] private EditorTileDataPool EditorTileDataPool { get; set; }

        private Tilemap _walls, _damage;
        private GameplayGuiManager _gameplayGuiManager;
        private GhostRecorder _recorder;

        public LevelState Level { get; private set; }
        #endregion

        public override void DoAwake()
        {
            LevelManager.DoAwake();

            string code = this.Exchange.CodeToLoad;
            _gameplayGuiManager.SetLevelCode(code);
            _recorder.SetLevelCode(code);
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

        public void SetRecorder(GhostRecorder ghostRecorder)
        {
            _recorder = ghostRecorder;
            _recorder.SetLevelManager(this.LevelManager);
        }

        public override void DoStart()
        {
            string code = this.Exchange.CodeToLoad;

            LevelManager.Load(code, OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelState levelState)
        {
            OnLevelLoading?.Invoke(levelState);

            this.Level = levelState;
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

                void ScoreSubmitted(string code, string remoteId, bool isSteam)
                {
                    // @todo do we need this?
                }
            }
        }

        private void OnRatingSubmitRequested(string code, int rating)
        {
            LevelManager.LevelStorage.LoadLevelData(code, RatingLevelLoaded);

            void RatingLevelLoaded(LevelSave levelSave)
            {
                LevelManager.SubmitRating(rating, levelSave, RatingSubmitted);

                void RatingSubmitted(string code, string remoteId, bool isSteam)
                {
                    PreferencesManager.Instance.SetLevelRating(code, rating);
                }
            }
        }

        public override void DoOnEnable()
        {
            WinMenu.OnScoreSubmit += OnScoreSubmitRequested;
            WinMenu.OnRatingSubmit += OnRatingSubmitRequested;
        }

        public override void DoOnDisable()
        {
            WinMenu.OnScoreSubmit -= OnScoreSubmitRequested;
            WinMenu.OnRatingSubmit -= OnRatingSubmitRequested;
        }
    }
}
