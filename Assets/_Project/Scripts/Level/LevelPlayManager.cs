using System;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using Gameplay;
using Gameplay.GUI;
using LevelEditor;
using Singletons;
using UI;
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

        [field: SerializeField] private Tilemap Tilemap_Foreground { get; set; }
        [field: SerializeField] private Tilemap Tilemap_Background { get; set; }
        private Canvas ModalCanvas { get; set; }

        private GameplayGuiManager _gameplayGuiManager;
        private GhostRecorder _recorder;
        private string _code;
        private AchievementPopupBlock AchievementBlock { get; set; }

        public LevelState Level { get; private set; }
        #endregion

        public override void DoAwake()
        {
            this.LevelManager.DoAwake();

            string code = this.Exchange.CodeToLoad;
            this._gameplayGuiManager.SetLevelCode(code);
            this._recorder.SetLevelCode(code);
            if (AchievementManager.Instance != null)
                AchievementManager.Instance.SetLevelCode(code);
        }

        public void SetForeground(Tilemap tilemap)
        {
            this.Tilemap_Foreground = tilemap;
        }

        public void SetBackground(Tilemap tilemap)
        {
            this.Tilemap_Background = tilemap;
        }

        public void SetGuiManager(GameplayGuiManager gameplayGuiManager)
        {
            this._gameplayGuiManager = gameplayGuiManager;
            gameplayGuiManager.SetLevelManager(this.LevelManager);
        }

        public void SetRecorder(GhostRecorder ghostRecorder)
        {
            this._recorder = ghostRecorder;
            this._recorder.SetLevelManager(this.LevelManager);
        }

        public void SetCanvas(Canvas modalCanvas)
        {
            this.ModalCanvas = modalCanvas;
        }

        public void SetAchievementBlock(AchievementPopupBlock block)
        {
            this.AchievementBlock = block;
        }


        public override void DoStart()
        {
            _code = this.Exchange.CodeToLoad;

            this.LevelManager.Load(_code, this.OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelState levelState)
        {
            OnLevelLoading?.Invoke(levelState);

            this.Level = levelState;
            this.PaintTilesFromFile(levelState);
            this._gameplayGuiManager.SetLevelState(levelState);
            //GameEvent.Trigger(GameEventType.Unpause);
        }

        private void OnLevelCompleted()
        {
            this.LevelManager.LevelStorage.LoadLevelData(_code, LevelCompletedLevelLoaded);

            AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.LevelComplete, _code);

            void LevelCompletedLevelLoaded(LevelSave levelSave)
            {
                if (levelSave.Completed)
                {
                    return;
                }

                this.LevelManager.MarkLevelAsComplete(levelSave);
            }

        }

        #region Tile Operations

        private void PaintTilesFromFile(LevelState level)
        {
            for (int y = 0; y < level.ScaleY; y++)
            {
                for (int x = 0; x < level.ScaleX; x++)
                {
                    TileState tileState = level.Tiles[x, y];

                    if (tileState == null || (tileState.Foreground == TileType.Empty && tileState.Background == TileType.Empty))
                    {
                        continue;
                    }

                    Vector3Int position = new Vector3Int(x, y);

                    TileData foreground = this.GetTileDataFromType(tileState.Foreground);
                    TileData background = this.GetTileDataFromType(tileState.Background);

                    Rotation foregroundRotation = tileState.ForegroundRotation;
                    Rotation backgroundRotation = tileState.BackgroundRotation;

                    TileConfig tileConfig = tileState.Config;

                    this.PlaceTile(foreground, background, position);
                    this.InstantiateTile(foreground, foregroundRotation, background, backgroundRotation, position, tileConfig);
                }
            }
        }

        private TileData GetTileDataFromType(TileType tileStateType)
        {
            EditorTileData tileData = this.EditorTileDataPool.Get(tileStateType);

            if (tileData == null)
                return null;

            return tileData.Tile;
        }

        private void InstantiateTile(TileData foreground, Rotation foregroundRotation, TileData background, Rotation backgroundRotation, Vector3Int position, TileConfig tileConfig)
        {
            if (foreground?.GameObject != null)
            {
                var gObj = Instantiate(foreground.GameObject, position + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, foregroundRotation.ToDegree()));
                if (gObj.TryGetComponent<IConfigurable>(out IConfigurable configurable))
                {
                    configurable.Configure(tileConfig);
                }
            }

            if (background?.GameObject != null)
                Instantiate(background.GameObject, position + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, backgroundRotation.ToDegree()));
        }

        private void PlaceTile(TileData foreground, TileData background, Vector3Int position)
        {
            if (foreground?.TileMapTileBase != null)
                this.SetForegroundTile(position, foreground.TileMapTileBase);

            if (background?.TileMapTileBase != null)
                this.SetBackgroundTile(position, background.TileMapTileBase);
        }

        private void SetForegroundTile(Vector3Int position, TileBase tile)
        {
            this.Tilemap_Foreground.SetTile(position, tile);
        }

        private void SetBackgroundTile(Vector3Int position, TileBase tile)
        {
            this.Tilemap_Background.SetTile(position, tile);
        }

        #endregion


        private void OnScoreSubmitRequested(string code, float time, WinMenu.WinMenu_OnScoreSubmit callback)
        {
#if YAN_DEBUG
            Debug.Log("yan");
#else
            Debug.Log("not yan");
#endif

            this.LevelManager.LevelStorage.LoadLevelData(code, ScoreLevelLoaded);

            void ScoreLevelLoaded(LevelSave levelSave)
            {
                this.LevelManager.SubmitScore(time, levelSave, ScoreSubmitted);

                void ScoreSubmitted(string code, string remoteId, bool isSteam)
                {
                    // @todo do we need this?
                    AchievementManager.Instance.UnlockAchievement(GameAchievement.LevelScoreSubmitted);
                    AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.LevelScoreSubmitted, $"{code}-{time}");

                    // Update leaderboard.
                    callback.Invoke();
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
                    AchievementManager.Instance.UnlockAchievement(GameAchievement.LevelRated);
                    AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.LevelRatingSubmitted, $"{code}-{rating.ToString()}");
                }
            }
        }

        private void OnShowAchievement(PopupAchievement achievement)
        {

            var popup = Instantiate(this.AchievementBlock, this.ModalCanvas.transform);
            popup.Setup(achievement);

        }

        public override void DoOnEnable()
        {
            WinMenu.OnScoreSubmit += this.OnScoreSubmitRequested;
            WinMenu.OnRatingSubmit += this.OnRatingSubmitRequested;
            Chest.OnChestOpened += this.OnLevelCompleted;
            AchievementManager.OnShowAchievement += OnShowAchievement;
        }

        public override void DoOnDisable()
        {
            WinMenu.OnScoreSubmit -= this.OnScoreSubmitRequested;
            WinMenu.OnRatingSubmit -= this.OnRatingSubmitRequested;
            Chest.OnChestOpened -= this.OnLevelCompleted;
            AchievementManager.OnShowAchievement -= OnShowAchievement;
        }
    }
}
