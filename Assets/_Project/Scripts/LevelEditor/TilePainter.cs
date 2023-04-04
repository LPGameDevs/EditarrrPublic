using Editarrr.Input;
using Singletons;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Yanniboi;

namespace LevelEditor
{
    public enum TilemapPainterLayer
    {
        Background = 0,
        Platform = 10,
        Elements = 20,
        Damage = 30,
    }

    public class TilePainter : MonoBehaviour
    {
        public static event Action<IFrameSelectable> OnTilePlaced, OnTileRemoved;

        public TileBase Tile;
        public Tilemap TilemapBackground, TilemapPlatform, TilemapDamage, TilemapWalls, TilemapElements, TilemapHover;
        public bool IsEditorScene = true;

        private IFrameSelectable _currentTileSO;
        private Tilemap _currentTilemap;
        private bool _backToSelection = false;

        /* Input */
        private bool _isPainting;
        private bool _isMouseLeftDown;
        private bool _isMouseRightDown;
        private bool _isMouseLeftClick;
        private bool _isMouseRightClick;


        private Camera _camera;
        private LevelSave _currentLevelSave;
        private bool _resetLevel = true;
        private Vector3 _cameraAtStart;
        private string _levelCode;
        private Vector3Int _previousHoverTilePosition;

        // Temp
        public Vector3 minPosition = new Vector3(-15, -9, 0);
        public Vector3 maxPosition = new Vector3(15, -9, 0);
        public TileBase BackgroundTile, WallTile, PlatformTile, SpikeTile, SpawnTile, WinTile;
        public EditorTileSO BackgroundSO, SpikesSO, WallSO, PlayerSpawnSO, PlayerWinSO, EnemySO, EnemyFollowSO;
        // public EditorTileSO MovingPlatformSO, MovingPlatformDeathSO;

        public EditorTileSO PlayerSpawn;
        private bool _isHoverTilesEnabled;


        #region Input
        [field: SerializeField, Header("Input")] private InputValue MousePosition { get; set; }
        [field: SerializeField] private InputValue MouseLeftButton { get; set; }
        [field: SerializeField] private InputValue MouseMiddleButton { get; set; }
        [field: SerializeField] private InputValue MouseRightButton { get; set; }

        [field: SerializeField] private InputValue SelectTile { get; set; }
        #endregion

        #region LevelSetup

        private void RegisterEditorSOs()
        {
            EditorItemManager.Instance.EditorTiles = new[]
            {
                // @todo fix moving platforms.
                // WallSO, SpikesSO, PlayerSpawnSO, PlayerWinSO, EnemySO, EnemyFollowSO, MovingPlatformSO, MovingPlatformDeathSO
                this.WallSO, this.SpikesSO, this.PlayerSpawnSO, this.PlayerWinSO, this.EnemySO, this.EnemyFollowSO
            };
        }

        private void RemoveAllTiles()
        {
            this.TilemapBackground.ClearAllTiles();
            this.TilemapDamage.ClearAllTiles();
            this.TilemapPlatform.ClearAllTiles();
            this.TilemapElements.ClearAllTiles();
        }

        private void PaintDefaultTiles()
        {
            this.Tile = this.WallTile;
            this._currentTilemap = this.TilemapWalls;
            this._currentTileSO = this.WallSO;
            for (int x = 0; x <= (this.maxPosition.x - this.minPosition.x); x++)
            {
                for (int y = 0; y <= (this.maxPosition.y - this.minPosition.y); y++)
                {
                    Vector3 point = new Vector3(x, y, 0) + this.minPosition;
                    Vector3Int selectedTile = this._currentTilemap.WorldToCell(point);
                    this.PaintTile(selectedTile, true, false);
                }
            }
        }

        private void PaintTilesFromFile()
        {
            this._currentTileSO = this.BackgroundSO;
            this._currentTilemap = this.TilemapBackground;
            foreach (var groundTile in this._currentLevelSave.groundTiles)
            {
                this.PaintTile(groundTile, false, false);
            }

            this._currentTileSO = this.WallSO;
            foreach (var wallTile in this._currentLevelSave.wallTiles)
            {
                this._currentTilemap = this.TilemapBackground;
                this.PaintTile(wallTile, false, false);

                if (!this.IsEditorScene)
                {
                    this._currentTilemap = this.TilemapWalls;
                    this.PaintTile(wallTile, false, false);
                }
            }

            this._currentTileSO = this.WallSO;
            this._currentTilemap = this.TilemapPlatform;
            foreach (var platformTile in this._currentLevelSave.platformTiles)
            {
                this.PaintTile(platformTile, false, false);
            }

            this._currentTileSO = this.SpikesSO;
            this._currentTilemap = this.TilemapDamage;
            foreach (var spikeTile in this._currentLevelSave.spikeTiles)
            {
                this.PaintTile(spikeTile, false, false);
            }

            if (this.IsEditorScene)
            {
                // Player spawn.
                if (this._currentLevelSave.playerSpawn != Vector3Int.zero)
                {
                    this._currentTileSO = this.PlayerSpawnSO;
                    this._currentTilemap = this.TilemapElements;
                    this.PaintTile(this._currentLevelSave.playerSpawn, false, true);
                }

                // Player win.
                if (this._currentLevelSave.playerWin != Vector3Int.zero)
                {
                    this._currentTileSO = this.PlayerWinSO;
                    this._currentTilemap = this.TilemapElements;
                    this.PaintTile(this._currentLevelSave.playerWin, false, true);
                }

                // Enemies.
                foreach (var enemyTile in this._currentLevelSave.enemyTiles)
                {
                    this._currentTileSO = this.EnemySO;
                    this._currentTilemap = this.TilemapElements;
                    this.PaintTile(enemyTile, false, true);
                }

                foreach (var enemyTile in this._currentLevelSave.enemyFollowTiles)
                {
                    this._currentTileSO = this.EnemyFollowSO;
                    this._currentTilemap = this.TilemapElements;
                    this.PaintTile(enemyTile, false, true);
                }

                // @todo fix moving platforms
                // foreach (var movingPlatformTile in _currentLevelSave.movingPlatformTiles)
                // {
                //     _currentTileSO = MovingPlatformSO;
                //     _currentTilemap = TilemapPlatform;
                //     PaintTile(movingPlatformTile, false, true);
                // }
                // foreach (var movingPlatformDeathTile in _currentLevelSave.movingPlatformDeathTiles)
                // {
                //     _currentTileSO = MovingPlatformDeathSO;
                //     _currentTilemap = TilemapPlatform;
                //     PaintTile(movingPlatformDeathTile, false, true);
                // }
            }

            if (!this.IsEditorScene)
            {
                if (this._currentLevelSave.playerSpawn != Vector3Int.zero && this.PlayerSpawnSO.prefab != null)
                {
                    Instantiate(this.PlayerSpawnSO.prefab, (Vector3)this._currentLevelSave.playerSpawn + new Vector3(0.5f, 0.5f, 0),
                        Quaternion.identity);
                }

                if (this._currentLevelSave.playerWin != Vector3Int.zero && this.PlayerWinSO.prefab != null)
                {
                    Instantiate(this.PlayerWinSO.prefab, (Vector3)this._currentLevelSave.playerWin + new Vector3(0.5f, 0.5f, 0),
                        Quaternion.identity);
                }

                if (this.EnemySO.prefab != null)
                {
                    foreach (var enemyTile in this._currentLevelSave.enemyTiles)
                    {
                        // Instantiate(EnemySO.prefab, (Vector3) enemyTile, Quaternion.identity);
                        Instantiate(this.EnemySO.prefab, (Vector3)enemyTile + new Vector3(0.5f, 0.5f, 0),
                            Quaternion.identity);
                    }
                }

                if (this.EnemyFollowSO.prefab != null)
                {
                    foreach (var enemyTile in this._currentLevelSave.enemyFollowTiles)
                    {
                        Instantiate(this.EnemyFollowSO.prefab, (Vector3)enemyTile + new Vector3(0.5f, 0.5f, 0),
                            Quaternion.identity);
                    }
                }

                // @todo fix moving platforms
                // if (MovingPlatformSO.prefab != null)
                // {
                //     foreach (var movingPlatformTile in _currentLevelSave.movingPlatformTiles)
                //     {
                //         Transform platform = Instantiate(MovingPlatformSO.prefab, (Vector3) movingPlatformTile + new Vector3(0.5f, 0.5f, 0),
                //             Quaternion.identity);
                //         MovingPlatform movingPlatform = platform.GetComponent<MovingPlatform>();
                //         TileOptions options = GetCurrentOptions(movingPlatformTile) ?? new TileOptions();
                //         MovingPlatformOptions platformOptions = JsonUtility.FromJson<MovingPlatformOptions>(options.options) ?? new MovingPlatformOptions();
                //         movingPlatform.PathElements.ForEach(x => x.PathElementPosition.x *= platformOptions.direction);
                //     }
                // }
                //
                // if (MovingPlatformDeathSO.prefab != null)
                // {
                //     foreach (var movingPlatformDeathTile in _currentLevelSave.movingPlatformDeathTiles)
                //     {
                //         Transform platform = Instantiate(MovingPlatformDeathSO.prefab, (Vector3) movingPlatformDeathTile + new Vector3(0.5f, 0.5f, 0),
                //             Quaternion.identity);
                //         MovingPlatform movingPlatform = platform.GetComponent<MovingPlatform>();
                //         TileOptions options = GetCurrentOptions(movingPlatformDeathTile) ?? new TileOptions();
                //         MovingPlatformOptions platformOptions = JsonUtility.FromJson<MovingPlatformOptions>(options.options) ?? new MovingPlatformOptions();
                //         movingPlatform.PathElements.ForEach(x => x.PathElementPosition.x *= platformOptions.direction);
                //     }
                // }
            }

            // Reset current tile and UI for start of level.
            this.SwapTile(this.WallSO);
            OnTilePlaced?.Invoke(this._currentTileSO);
        }

        #endregion

        #region DataManagement

        /**
         * Used when saving a new level.
         */
        public void SaveAndReset()
        {
            this.SaveLevel();
            this.ResetEditorSave();
        }

        public void PrepareTestLevel()
        {
            if (this._levelCode.Length > 0)
            {
                this.SaveLevel();
            }
            else
            {
                this.SaveIfNotLevel();
            }
        }

        /**
         * Update or create a new level with the current changes.
         */
        public void SaveLevel()
        {
            string data = JsonUtility.ToJson(this._currentLevelSave);
            EditorLevelStorage.Instance.SaveLevel(this._levelCode, data, true);
        }

        /**
         * Update current editor changes without creating a saved level.
         */
        public void SaveIfNotLevel()
        {
            // @todo!! Everything breaks if you are testing a saved level :P

            // Only update current if we are not in a level.
            if (this._levelCode.Length > 0)
            {
                return;
            }

            string data = JsonUtility.ToJson(this._currentLevelSave);
            File.WriteAllText(EditorLevelStorage.LevelStorageEditorLevel, data);
        }

        /**
         * Reset level back to starter template.
         */
        public void ResetLevel()
        {
            this.ResetEditorSave();
            this.RemoveAllTiles();
            this.PaintDefaultTiles();
            EditorItemManager.Instance.ResetTraps();
            this._camera.transform.position = this._cameraAtStart;
        }

        /**
         * Clear out the unsaved Editor changes.
         */
        public void ResetEditorSave()
        {
            string data = JsonUtility.ToJson("");
            File.WriteAllText(EditorLevelStorage.LevelStorageEditorLevel, data);
            this._currentLevelSave = JsonUtility.FromJson<LevelSave>(data);
        }

        #endregion

        #region Painting

        private bool CanPaintHoverTile()
        {
            return this._isHoverTilesEnabled;
        }

        private void PaintHoverTile(Vector3Int selectedTile)
        {
            if (!this.CanPaintHoverTile())
            {
                return;
            }

            this.ClearHoverTile();

            this._previousHoverTilePosition = this._currentTileSO.GetPlacedTilePosition(this._currentTilemap, selectedTile) ?? selectedTile;
            if (this._currentTileSO.canPaint(this._currentTilemap, this._previousHoverTilePosition))
            {
                this._currentTileSO.Paint(this.TilemapHover, this._previousHoverTilePosition, null);
            }
            else if (this._currentTileSO.HasOptions() && this._currentTilemap.GetTile(this._previousHoverTilePosition) != null)
            {
                this._currentTileSO.Highlight(this.TilemapHover, this._previousHoverTilePosition, Color.red);
            }
        }

        private void ClearHoverTile()
        {
            if (!this.CanPaintHoverTile())
            {
                return;
            }

            if (!this.IsEditorScene || this._currentTileSO == null)
            {
                return;
            }

            this._currentTileSO.UnPaint(this.TilemapHover, this._previousHoverTilePosition);
        }

        private void PaintTile(Vector3Int selectedTile, bool store = true, bool track = true)
        {
            TileOptions options = null;
            if (this._currentTileSO.HasOptions())
            {
                options = this.GetCurrentOptions(selectedTile);
            }

            this.PaintTile(selectedTile, store, track, options);
        }


        private void PaintTile(Vector3Int selectedTile, bool store, bool track, TileOptions options)
        {
            if (this._currentTileSO == null)
            {
                return;
            }

            if (track && !this._currentTileSO.isInfinite() && this._currentTileSO.getCurrentItemCount() < 1)
            {
                return;
            }

            if (!this._currentTileSO.canPaint(this._currentTilemap, selectedTile))
            {
                return;
            }

            if (track)
            {
                this._currentTileSO.reduceItemCount();
            }

            if (store)
            {
                this._currentLevelSave.StorePlacedTile(this._currentTileSO, selectedTile, options);
            }

            OnTilePlaced?.Invoke(this._currentTileSO);
            this._currentTileSO.Paint(this._currentTilemap, selectedTile, options);
        }

        private void UnPaintTile(Vector3Int selectedTile, bool store = true, bool track = true)
        {
            TileOptions options = null;
            if (this._currentTileSO.HasOptions())
            {
                options = this.GetCurrentOptions(selectedTile);
            }

            this.UnPaintTile(selectedTile, store, track, options);
        }

        private void UnPaintTile(Vector3Int selectedTile, bool track, bool store, TileOptions options)
        {
            if (this._currentTileSO == null)
            {
                return;
            }

            if (!this._currentTileSO.canUnPaint(this._currentTilemap, selectedTile))
            {
                return;
            }

            if (track)
            {
                this._currentTileSO.increaseItemCount();
            }

            if (store)
            {
                Vector3Int storedPosition = this._currentTileSO.GetPlacedTilePosition(this._currentTilemap, selectedTile) ?? selectedTile;
                this._currentLevelSave.RemovePlacedTile(this._currentTileSO, storedPosition, options);
            }

            OnTileRemoved?.Invoke(this._currentTileSO);
            this._currentTileSO.UnPaint(this._currentTilemap, selectedTile);
        }

        private TileOptions GetCurrentOptions(Vector3Int selectedTile)
        {
            var position = this._currentTileSO.GetPlacedTilePosition(this._currentTilemap, selectedTile);
            if (position == null)
            {
                position = selectedTile;
            }

            return this._currentLevelSave.GetTileOptions((Vector3Int)position);
        }

        private void ToggleOptions(Vector3Int selectedTile)
        {
            if (!this._currentTileSO.HasOptions())
            {
                return;
            }

            TileOptions options = this.GetCurrentOptions(selectedTile);
            options = this._currentTileSO.NextOption(options);

            var position = this._currentTileSO.GetPlacedTilePosition(this._currentTilemap, selectedTile);

            if (position == null)
            {
                return;
            }

            selectedTile = (Vector3Int)position;
            this._currentLevelSave.StorePlacedTile(this._currentTileSO, selectedTile, options);
            this._currentTileSO.Paint(this._currentTilemap, selectedTile, options);
        }

        private Tilemap GetMapFromTile(IFrameSelectable tile)
        {
            switch (tile.getLayer())
            {
                default:
                case TilemapPainterLayer.Background:
                    return this.TilemapBackground;
                case TilemapPainterLayer.Damage:
                    return this.TilemapDamage;
                case TilemapPainterLayer.Platform:
                    return this.TilemapPlatform;
                case TilemapPainterLayer.Elements:
                    return this.TilemapElements;
            }
        }

        private void SwapTile(IFrameSelectable tileFrame)
        {
            this.ClearHoverTile();
            IFrameSelectable tileSO = tileFrame;
            this.Tile = tileSO.getTile();
            this._currentTileSO = tileSO;
            this._currentTilemap = this.GetMapFromTile(tileSO);
        }

        #endregion

        #region UnityHooks

        private void Awake()
        {
            // General setup
            this._camera = Camera.main;
            this._currentLevelSave = new LevelSave();
            this._isHoverTilesEnabled = this.TilemapHover.IsNull("Hover Tilemap is not set so hover tiles are disabled."); ;

            string data = File.ReadAllText(EditorLevelStorage.LevelStorageEditorLevel);
            this._levelCode = PlayerPrefs.GetString("EditorCode");
            if (this._levelCode.Length > 0)
            {
                string fileName = EditorLevelStorage.LevelStoragePath + this._levelCode + ".json";
                if (File.Exists(fileName))
                {
                    data = File.ReadAllText(fileName);
                }
                else if (File.Exists(EditorLevelStorage.DistroLevelStoragePath + this._levelCode + ".json"))
                {
                    data = File.ReadAllText(EditorLevelStorage.DistroLevelStoragePath + this._levelCode + ".json");
                }
            }

            if (data.Length > 2)
            {
                this._currentLevelSave = JsonUtility.FromJson<LevelSave>(data);
                this._resetLevel = false;
            }

            string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);
            if (this._currentLevelSave.creator.Length == 0)
            {
                this._currentLevelSave.creator = userName;
            }

            if (this._currentLevelSave.creator.Length > 0 && this._currentLevelSave.creator.ToLower() != userName.ToLower())
            {
                Debug.Log("Not allowed to edit a level by: " + this._currentLevelSave.creator);
                this._backToSelection = true;
            }

            if (this.IsEditorScene)
            {
                if (this._backToSelection)
                {
                    LevelManager.Instance.GotoLevel(LevelManager.LevelSelectionSceneName);
                }

                this.RegisterEditorSOs();
                this._cameraAtStart = this._camera.transform.position;
            }
        }

        private void Start()
        {
            if (this._resetLevel)
            {
                this.PaintDefaultTiles();
            }
            else
            {
                this.PaintTilesFromFile();
            }
        }

        void Update()
        {
            if (!this.IsEditorScene)
            {
                return;
            }

            if (this._currentTilemap == null)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                this.ClearHoverTile();
                return;
            }

            Vector3 point = this._camera.ScreenToWorldPoint(this.MousePosition.Read<Vector2>());
            Vector3Int selectedTile = this._currentTilemap.WorldToCell(point);
            if (this._isMouseLeftClick || this._isMouseRightClick)
            {
                this._isPainting = true;

                if (this._isMouseLeftClick)
                {
                    this.ToggleOptions(selectedTile);
                }
            }
            else if (this._isPainting && this._isMouseLeftDown)
            {
                this.PaintTile(selectedTile);
            }
            else if (this._isPainting && this._isMouseRightDown)
            {
                this.UnPaintTile(selectedTile);
            }
            else
            {
                this._isPainting = false;
                if (selectedTile != this._previousHoverTilePosition)
                {
                    this.PaintHoverTile(selectedTile);
                }

            }

            this.HandleKeyInput(point);
            this.HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            this._isMouseLeftClick = false;
            this._isMouseRightClick = false;
            if (this.MouseLeftButton.WasPressed)
            {
                this._isMouseLeftClick = true;
            }
            else if (this.MouseLeftButton.IsPressed)
            {
                this._isMouseLeftDown = true;
            }
            else if (this.MouseLeftButton.WasReleased)
            {
                this._isMouseLeftDown = false;
            }

            if (this.MouseRightButton.WasPressed)
            {
                this._isMouseRightClick = true;

            }
            else if (this.MouseRightButton.IsPressed)
            {
                this._isMouseRightDown = true;

            }
            else if (this.MouseRightButton.WasReleased)
            {
                this._isMouseRightDown = false;
            }
        }

        private void HandleKeyInput(Vector3 position)
        {
            if (this.SelectTile.WasPressed || this.MouseMiddleButton.WasPressed) //Input.GetKeyDown(KeyCode.LeftShift))
            {
                // public Tilemap TilemapBackground, TilemapPlatform, TilemapDamage, TilemapWalls, TilemapElements, TilemapHover;

                Vector3Int selectedTile = this._currentTilemap.WorldToCell(position);

                if (this.CheckForTile(selectedTile, this.TilemapElements))
                {
                    TileBase tile = this.TilemapElements.GetTile(selectedTile);
                    this.SwapTileUntilFound(tile);
                }
                else if (this.CheckForTile(selectedTile, this.TilemapWalls))
                {
                    TileBase tile = this.TilemapWalls.GetTile(selectedTile);
                    this.SwapTileUntilFound(tile);
                }
                else if (this.CheckForTile(selectedTile, this.TilemapPlatform))
                {
                    TileBase tile = this.TilemapPlatform.GetTile(selectedTile);
                    this.SwapTileUntilFound(tile);
                }
                else if (this.CheckForTile(selectedTile, this.TilemapDamage))
                {
                    TileBase tile = this.TilemapDamage.GetTile(selectedTile);
                    this.SwapTileUntilFound(tile);
                }
                else if (this.CheckForTile(selectedTile, this.TilemapBackground))
                {
                    TileBase tile = this.TilemapBackground.GetTile(selectedTile);
                    this.SwapTileUntilFound(tile);
                }
            }
        }

        private void SwapTileUntilFound(TileBase tile)
        {
            while (this.Tile != tile)
            {
                EditorItemManager.Instance.NextTrap();
            }
        }

        private bool CheckForTile(Vector3Int position, Tilemap tilemapElements)
        {
            return tilemapElements.HasTile(position);
        }

        private void OnEnable()
        {
            EditorItemManager.OnTileSelected += this.SwapTile;
        }

        private void OnDisable()
        {
            EditorItemManager.OnTileSelected -= this.SwapTile;
        }

        #endregion
    }
}
