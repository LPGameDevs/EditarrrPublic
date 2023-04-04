using System;
using System.IO;
using Editarrr.Input;
using Singletons;
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
        [field: SerializeField] private InputValue MousePosition { get; set; }
        [field: SerializeField] private InputValue MouseLeftButton { get; set; }
        [field: SerializeField] private InputValue MouseRightButton { get; set; }
        #endregion

        #region LevelSetup

        private void RegisterEditorSOs()
        {
            EditorItemManager.Instance.EditorTiles = new[]
            {
                // @todo fix moving platforms.
                // WallSO, SpikesSO, PlayerSpawnSO, PlayerWinSO, EnemySO, EnemyFollowSO, MovingPlatformSO, MovingPlatformDeathSO
                WallSO, SpikesSO, PlayerSpawnSO, PlayerWinSO, EnemySO, EnemyFollowSO
            };
        }

        private void RemoveAllTiles()
        {
            TilemapBackground.ClearAllTiles();
            TilemapDamage.ClearAllTiles();
            TilemapPlatform.ClearAllTiles();
            TilemapElements.ClearAllTiles();
        }

        private void PaintDefaultTiles()
        {
            Tile = WallTile;
            _currentTilemap = TilemapWalls;
            _currentTileSO = WallSO;
            for (int x = 0; x <= (maxPosition.x - minPosition.x); x++)
            {
                for (int y = 0; y <= (maxPosition.y - minPosition.y); y++)
                {
                    Vector3 point = new Vector3(x, y, 0) + minPosition;
                    Vector3Int selectedTile = _currentTilemap.WorldToCell(point);
                    PaintTile(selectedTile, true, false);
                }
            }
        }

        private void PaintTilesFromFile()
        {
            _currentTileSO = BackgroundSO;
            _currentTilemap = TilemapBackground;
            foreach (var groundTile in _currentLevelSave.groundTiles)
            {
                PaintTile(groundTile, false, false);
            }

            _currentTileSO = WallSO;
            foreach (var wallTile in _currentLevelSave.wallTiles)
            {
                _currentTilemap = TilemapBackground;
                PaintTile(wallTile, false, false);

                if (!IsEditorScene)
                {
                    _currentTilemap = TilemapWalls;
                    PaintTile(wallTile, false, false);
                }
            }

            _currentTileSO = WallSO;
            _currentTilemap = TilemapPlatform;
            foreach (var platformTile in _currentLevelSave.platformTiles)
            {
                PaintTile(platformTile, false, false);
            }

            _currentTileSO = SpikesSO;
            _currentTilemap = TilemapDamage;
            foreach (var spikeTile in _currentLevelSave.spikeTiles)
            {
                PaintTile(spikeTile, false, false);
            }

            if (IsEditorScene)
            {
                // Player spawn.
                if (_currentLevelSave.playerSpawn != Vector3Int.zero)
                {
                    _currentTileSO = PlayerSpawnSO;
                    _currentTilemap = TilemapElements;
                    PaintTile(_currentLevelSave.playerSpawn, false, true);
                }

                // Player win.
                if (_currentLevelSave.playerWin != Vector3Int.zero)
                {
                    _currentTileSO = PlayerWinSO;
                    _currentTilemap = TilemapElements;
                    PaintTile(_currentLevelSave.playerWin, false, true);
                }

                // Enemies.
                foreach (var enemyTile in _currentLevelSave.enemyTiles)
                {
                    _currentTileSO = EnemySO;
                    _currentTilemap = TilemapElements;
                    PaintTile(enemyTile, false, true);
                }

                foreach (var enemyTile in _currentLevelSave.enemyFollowTiles)
                {
                    _currentTileSO = EnemyFollowSO;
                    _currentTilemap = TilemapElements;
                    PaintTile(enemyTile, false, true);
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

            if (!IsEditorScene)
            {
                if (_currentLevelSave.playerSpawn != Vector3Int.zero && PlayerSpawnSO.prefab != null)
                {
                    Instantiate(PlayerSpawnSO.prefab, (Vector3) _currentLevelSave.playerSpawn + new Vector3(0.5f, 0.5f, 0),
                        Quaternion.identity);
                }

                if (_currentLevelSave.playerWin != Vector3Int.zero && PlayerWinSO.prefab != null)
                {
                    Instantiate(PlayerWinSO.prefab, (Vector3) _currentLevelSave.playerWin + new Vector3(0.5f, 0.5f, 0),
                        Quaternion.identity);
                }

                if (EnemySO.prefab != null)
                {
                    foreach (var enemyTile in _currentLevelSave.enemyTiles)
                    {
                        // Instantiate(EnemySO.prefab, (Vector3) enemyTile, Quaternion.identity);
                        Instantiate(EnemySO.prefab, (Vector3) enemyTile + new Vector3(0.5f, 0.5f, 0),
                            Quaternion.identity);
                    }
                }

                if (EnemyFollowSO.prefab != null)
                {
                    foreach (var enemyTile in _currentLevelSave.enemyFollowTiles)
                    {
                        Instantiate(EnemyFollowSO.prefab, (Vector3) enemyTile + new Vector3(0.5f, 0.5f, 0),
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
            SwapTile(WallSO);
            OnTilePlaced?.Invoke(_currentTileSO);
        }

        #endregion

        #region DataManagement

        /**
         * Used when saving a new level.
         */
        public void SaveAndReset()
        {
            SaveLevel();
            ResetEditorSave();
        }

        public void PrepareTestLevel()
        {
            if (_levelCode.Length > 0)
            {
                SaveLevel();
            }
            else
            {
                SaveIfNotLevel();
            }
        }

        /**
         * Update or create a new level with the current changes.
         */
        public void SaveLevel()
        {
            string data = JsonUtility.ToJson(_currentLevelSave);
            EditorLevelStorage.Instance.SaveLevel(_levelCode, data, true);
        }

        /**
         * Update current editor changes without creating a saved level.
         */
        public void SaveIfNotLevel()
        {
            // @todo!! Everything breaks if you are testing a saved level :P

            // Only update current if we are not in a level.
            if (_levelCode.Length > 0)
            {
                return;
            }

            string data = JsonUtility.ToJson(_currentLevelSave);
            File.WriteAllText(EditorLevelStorage.LevelStorageEditorLevel, data);
        }

        /**
         * Reset level back to starter template.
         */
        public void ResetLevel()
        {
            ResetEditorSave();
            RemoveAllTiles();
            PaintDefaultTiles();
            EditorItemManager.Instance.ResetTraps();
            _camera.transform.position = _cameraAtStart;
        }

        /**
         * Clear out the unsaved Editor changes.
         */
        public void ResetEditorSave()
        {
            string data = JsonUtility.ToJson("");
            File.WriteAllText(EditorLevelStorage.LevelStorageEditorLevel, data);
            _currentLevelSave = JsonUtility.FromJson<LevelSave>(data);
        }

        #endregion

        #region Painting

        private bool CanPaintHoverTile()
        {
            return _isHoverTilesEnabled;
        }

        private void PaintHoverTile(Vector3Int selectedTile)
        {
            if (!CanPaintHoverTile())
            {
                return;
            }

            ClearHoverTile();

            _previousHoverTilePosition = _currentTileSO.GetPlacedTilePosition(_currentTilemap, selectedTile) ?? selectedTile;
            if (_currentTileSO.canPaint(_currentTilemap, _previousHoverTilePosition))
            {
                _currentTileSO.Paint(TilemapHover, _previousHoverTilePosition, null);
            }
            else if (_currentTileSO.HasOptions() && _currentTilemap.GetTile(_previousHoverTilePosition) != null) {
                _currentTileSO.Highlight(TilemapHover, _previousHoverTilePosition, Color.red);
            }
        }

        private void ClearHoverTile()
        {
            if (!CanPaintHoverTile())
            {
                return;
            }

            if (!IsEditorScene || _currentTileSO == null)
            {
                return;
            }

            _currentTileSO.UnPaint(TilemapHover, _previousHoverTilePosition);
        }

        private void PaintTile(Vector3Int selectedTile, bool store = true, bool track = true)
        {
            TileOptions options = null;
            if (_currentTileSO.HasOptions())
            {
                options = GetCurrentOptions(selectedTile);
            }

            PaintTile(selectedTile, store, track, options);
        }


        private void PaintTile(Vector3Int selectedTile, bool store, bool track, TileOptions options)
        {
            if (_currentTileSO == null)
            {
                return;
            }

            if (track && !_currentTileSO.isInfinite() && _currentTileSO.getCurrentItemCount() < 1)
            {
                return;
            }

            if (!_currentTileSO.canPaint(_currentTilemap, selectedTile))
            {
                return;
            }

            if (track)
            {
                _currentTileSO.reduceItemCount();
            }

            if (store)
            {
                _currentLevelSave.StorePlacedTile(_currentTileSO, selectedTile, options);
            }

            OnTilePlaced?.Invoke(_currentTileSO);
            _currentTileSO.Paint(_currentTilemap, selectedTile, options);
        }

        private void UnPaintTile(Vector3Int selectedTile, bool store = true, bool track = true)
        {
            TileOptions options = null;
            if (_currentTileSO.HasOptions())
            {
                options = GetCurrentOptions(selectedTile);
            }

            UnPaintTile(selectedTile, store, track, options);
        }

        private void UnPaintTile(Vector3Int selectedTile, bool track, bool store, TileOptions options)
        {
            if (_currentTileSO == null)
            {
                return;
            }

            if (!_currentTileSO.canUnPaint(_currentTilemap, selectedTile))
            {
                return;
            }

            if (track)
            {
                _currentTileSO.increaseItemCount();
            }

            if (store)
            {
                Vector3Int storedPosition = _currentTileSO.GetPlacedTilePosition(_currentTilemap, selectedTile) ?? selectedTile;
                _currentLevelSave.RemovePlacedTile(_currentTileSO, storedPosition, options);
            }

            OnTileRemoved?.Invoke(_currentTileSO);
            _currentTileSO.UnPaint(_currentTilemap, selectedTile);
        }

        private TileOptions GetCurrentOptions(Vector3Int selectedTile)
        {
            var position = _currentTileSO.GetPlacedTilePosition(_currentTilemap, selectedTile);
            if (position == null)
            {
                position = selectedTile;
            }

            return _currentLevelSave.GetTileOptions((Vector3Int) position);
        }

        private void ToggleOptions(Vector3Int selectedTile)
        {
            if (!_currentTileSO.HasOptions())
            {
                return;
            }

            TileOptions options = GetCurrentOptions(selectedTile);
            options = _currentTileSO.NextOption(options);

            var position = _currentTileSO.GetPlacedTilePosition(_currentTilemap, selectedTile);

            if (position == null)
            {
                return;
            }

            selectedTile = (Vector3Int) position;
            _currentLevelSave.StorePlacedTile(_currentTileSO, selectedTile, options);
            _currentTileSO.Paint(_currentTilemap, selectedTile, options);
        }

        private Tilemap GetMapFromTile(IFrameSelectable tile)
        {
            switch (tile.getLayer())
            {
                default:
                case TilemapPainterLayer.Background:
                    return TilemapBackground;
                case TilemapPainterLayer.Damage:
                    return TilemapDamage;
                case TilemapPainterLayer.Platform:
                    return TilemapPlatform;
                case TilemapPainterLayer.Elements:
                    return TilemapElements;
            }
        }

        private void SwapTile(IFrameSelectable tileFrame)
        {
            ClearHoverTile();
            IFrameSelectable tileSO = tileFrame;
            Tile = tileSO.getTile();
            _currentTileSO = tileSO;
            _currentTilemap = GetMapFromTile(tileSO);
        }

        #endregion

        #region UnityHooks

        private void Awake()
        {
            // General setup
            _camera = Camera.main;
            _currentLevelSave = new LevelSave();
            _isHoverTilesEnabled = TilemapHover.IsNull("Hover Tilemap is not set so hover tiles are disabled.");;

            string data = File.ReadAllText(EditorLevelStorage.LevelStorageEditorLevel);
            _levelCode = PlayerPrefs.GetString("EditorCode");
            if (_levelCode.Length > 0)
            {
                string fileName = EditorLevelStorage.LevelStoragePath + _levelCode + ".json";
                if (File.Exists(fileName))
                {
                    data = File.ReadAllText(fileName);
                }
                else if (File.Exists(EditorLevelStorage.DistroLevelStoragePath + _levelCode + ".json"))
                {
                    data = File.ReadAllText(EditorLevelStorage.DistroLevelStoragePath + _levelCode + ".json");
                }
            }

            if (data.Length > 2)
            {
                _currentLevelSave = JsonUtility.FromJson<LevelSave>(data);
                _resetLevel = false;
            }

            string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);
            if (_currentLevelSave.creator.Length == 0)
            {
                _currentLevelSave.creator = userName;
            }

            if (_currentLevelSave.creator.Length > 0 && _currentLevelSave.creator.ToLower() != userName.ToLower())
            {
                Debug.Log("Not allowed to edit a level by: " + _currentLevelSave.creator);
                _backToSelection = true;
            }

            if (IsEditorScene)
            {
                if (_backToSelection)
                {
                    LevelManager.Instance.GotoLevel(LevelManager.LevelSelectionSceneName);
                }

                RegisterEditorSOs();
                _cameraAtStart = _camera.transform.position;
            }
        }

        private void Start()
        {
            if (_resetLevel)
            {
                PaintDefaultTiles();
            }
            else
            {
                PaintTilesFromFile();
            }
        }

        void Update()
        {
            if (!IsEditorScene)
            {
                return;
            }

            if (_currentTilemap == null)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                ClearHoverTile();
                return;
            }

            Vector3 point = _camera.ScreenToWorldPoint(MousePosition.Read<Vector2>());
            Vector3Int selectedTile = _currentTilemap.WorldToCell(point);
            if (_isMouseLeftClick || _isMouseRightClick)
            {
                _isPainting = true;

                if (_isMouseLeftClick)
                {
                    ToggleOptions(selectedTile);
                }
            }
            else if (_isPainting && _isMouseLeftDown)
            {
                PaintTile(selectedTile);
            }
            else if (_isPainting && _isMouseRightDown)
            {
                UnPaintTile(selectedTile);
            }
            else
            {
                _isPainting = false;
                if (selectedTile != _previousHoverTilePosition)
                {
                    PaintHoverTile(selectedTile);
                }

            }

            HandleKeyInput(point);
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            _isMouseLeftClick = false;
            _isMouseRightClick = false;
            if (MouseLeftButton.WasPressed)
            {
                _isMouseLeftClick = true;
            }
            else if (MouseLeftButton.IsPressed)
            {
                _isMouseLeftDown = true;
            }
            else if (MouseLeftButton.WasReleased)
            {
                _isMouseLeftDown = false;
            }

            if (MouseRightButton.WasPressed)
            {
                _isMouseRightClick = true;

            }
            else if (MouseRightButton.IsPressed)
            {
                _isMouseRightDown = true;

            }
            else if (MouseRightButton.WasReleased)
            {
                _isMouseRightDown = false;
            }
        }

        private void HandleKeyInput(Vector3 position)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {

        // public Tilemap TilemapBackground, TilemapPlatform, TilemapDamage, TilemapWalls, TilemapElements, TilemapHover;


                Vector3Int selectedTile = _currentTilemap.WorldToCell(position);

                if (CheckForTile(selectedTile, TilemapElements))
                {
                    TileBase tile = TilemapElements.GetTile(selectedTile);
                    SwapTileUntilFound(tile);
                }
                else if (CheckForTile(selectedTile, TilemapWalls))
                {
                    TileBase tile = TilemapWalls.GetTile(selectedTile);
                    SwapTileUntilFound(tile);
                }
                else if (CheckForTile(selectedTile, TilemapPlatform))
                {
                    TileBase tile = TilemapPlatform.GetTile(selectedTile);
                    SwapTileUntilFound(tile);
                }
                else if (CheckForTile(selectedTile, TilemapDamage))
                {
                    TileBase tile = TilemapDamage.GetTile(selectedTile);
                    SwapTileUntilFound(tile);
                }
                else if (CheckForTile(selectedTile, TilemapBackground))
                {
                    TileBase tile = TilemapBackground.GetTile(selectedTile);
                    SwapTileUntilFound(tile);
                }

            }
        }

        private void SwapTileUntilFound(TileBase tile)
        {
            while (Tile != tile)
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
            EditorItemManager.OnTileSelected += SwapTile;
        }

        private void OnDisable()
        {
            EditorItemManager.OnTileSelected -= SwapTile;
        }

        #endregion
    }
}
