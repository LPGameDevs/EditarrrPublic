using Editarrr.Input;
using Editarrr.Level;
using Editarrr.Managers;
using Editarrr.Misc;
using Editarrr.Utilities;
using System;
using System.Collections.Generic;
using Editarrr.UI.LevelEditor;
using LevelEditor;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Level Manager", menuName = "Managers/Editor/new Editor Level Manager")]
    public class EditorLevelManager : ManagerComponent
    {
        public static TileSet OnTileSet { get; set; }
        public delegate void TileSet(EditorTileData data, TileType tileType, int inLevel);

        public static TileUnset OnTileUnset { get; set; }
        public delegate void TileUnset(EditorTileData data, TileType tileType, int inLevel);

        public static EditorLevelScaleChanged OnEditorLevelScaleChanged { get; set; }
        public delegate void EditorLevelScaleChanged(int x, int y);

        public static EditorConfigSelected OnEditorConfigSelected { get; set; }
        public delegate void EditorConfigSelected(TileConfig tileConfig);


        private const string Documentation =
            "This component manages input, placement and storage of the level editor.\r\n" +
            "Camera movement, tilemaps, loading, saving and screenshots all happen here.\r\n";

        [Info(Documentation)]
        [Tooltip("This does nothing...")]
        public bool _documentation;

        [field: SerializeField, Header("Settings")] public EditorLevelSettings Settings { get; private set; }

        [field: SerializeField, Header("Exchange")] private EditorLevelExchange Exchange { get; set; }

        [field: SerializeField, Header("Pools")] private EditorPrefabPool PrefabPool { get; set; }
        [field: SerializeField] private EditorTileDataPool EditorTileDataPool { get; set; }

        [field: SerializeField, Header("Managers")] private EditorTileSelectionManager EditorTileSelection { get; set; }
        [field: SerializeField] private LevelManager LevelManager { get; set; }

        #region Input
        [field: SerializeField, Header("Input")] private InputValue MousePosition { get; set; }
        [field: SerializeField] private InputValue MouseLeftButton { get; set; }
        [field: SerializeField] private InputValue MouseRightButton { get; set; }
        [field: SerializeField] private InputValue Input_CloneTile { get; set; }
        [field: SerializeField] private InputValue Input_OpenConfig { get; set; }
        #endregion

        Dictionary<TileType, List<Int2D>> TileLocations { get; set; }

        private LevelState LevelState { get; set; }
        private EditorTileState[,] Tiles { get; set; }

        private int ScaleX { get; set; }
        private int ScaleY { get; set; }
        // This is used to prevent automatically placing tiles on expanded Area
        // Might get solved by moving the Camera uppon expanding, but not yet >> QoL!
        bool IsExpanding { get; set; }

        SpriteRenderer EditorGrid { get; set; }


        // From System
        private Camera SceneCamera { get; set; }
        private Camera ScreenshotCamera { get; set; }
        private Tilemap Tilemap_Foreground { get; set; }
        private Tilemap Tilemap_Background { get; set; }
        private Canvas ModalCanvas { get; set; }
        private ModalPopup StartModal { get; set; }
        private ModalPopup InvalidModal { get; set; }



        private EditorHoverTile EditorHoverTile { get; set; }


        public void SetSceneCamera(Camera camera)
        {
            this.SceneCamera = camera;
        }

        public void SetScreenshotCamera(Camera camera)
        {
            this.ScreenshotCamera = camera;
        }

        public void SetTilemap_Foreground(Tilemap tilemap)
        {
            this.Tilemap_Foreground = tilemap;
        }

        public void SetTilemap_Background(Tilemap tilemap)
        {
            this.Tilemap_Background = tilemap;
        }

        public void SetCanvas(Canvas modalCanvas)
        {
            this.ModalCanvas = modalCanvas;
        }

        public void SetStartModal(ModalPopup startModal)
        {
            this.StartModal = startModal;
        }

        public void SetInvalidModal(ModalPopup invalidModal)
        {
            this.InvalidModal = invalidModal;
        }

        public override void DoAwake()
        {
            this.LevelManager.DoAwake();

            this.TileLocations = new Dictionary<TileType, List<Int2D>>();

            this.EditorGrid = Instantiate(this.PrefabPool.EditorGrid);
            this.EditorGrid.transform.position = Vector3.zero;

            this.EditorHoverTile = Instantiate(this.PrefabPool.EditorHoverTile);

            this.DisableHoverTile();
        }

        public override void DoStart()
        {
            if (this.Exchange.LoadOnStart)
            {
                this.LoadLevelState();
            }
            else
            {
                this.CreateLevelState();
            }


            EditorTileSelectionManager.OnTileSelect += this.EditorTileSelectionManager_OnTileSelect;
            EditorTileSelectionManager.ActiveElementChanged += this.EditorTileSelectionManager_ActiveElementChanged;
        }

        public override void DoUpdate()
        {
            if (this.EditorTileSelection.IsUIHover)
            {
                this.DisableHoverTile();
                return;
            }

            this.ClampPosition(this.GetCursorTileMapPosition(), out int x, out int y);

            EditorTileData tileData = this.EditorTileSelection.ActiveElement;

            if (this.Input_CloneTile.WasPressed)
            {
                EditorTileState atCursor = this.Tiles[x, y];
                if (atCursor != null)
                {
                    bool background = atCursor.Foreground == tileData || atCursor.Foreground == null;
                    background = background && atCursor.Background != null;

                    EditorTileData toClone = background ? atCursor.Background : atCursor.Foreground;
                    Rotation rotation = background ? atCursor.BackgroundRotation : atCursor.ForegroundRotation;

                    this.EditorTileSelection.SetActiveElement(toClone, rotation);
                }
            }

            if (tileData != null)
            {
                if (tileData.IsBackground)
                {
                    this.Tilemap_Foreground.color = Color.white * .5f;
                }
                else
                {
                    this.Tilemap_Foreground.color = Color.white;
                }
            }

            if (this.Input_OpenConfig.WasPressed)
            {
                EditorTileState state = this.Get(x, y);
                if (state != null && state.Config != null)
                {
                    this.NotifyConfig(state.Config);
                }
            }

            if (this.MouseLeftButton.WasPressed)
            {
                

                //if (state != null &&
                //    state.Foreground == tileData &&
                //    // state.ForegroundRotation == this.EditorTileSelection.Rotation && Might result in some weird and unclear situations...
                //    state.Config != null)
                //{
                //    this.NotifyConfig(state.Config);
                //}

                //if (state != null &&
                //    state.Foreground == tileData &&
                //    state.Rotation == this.EditorTileSelection.Rotation)
                //{
                //    // Use same Rotation in if ???
                //    // Change Options/Variations/Show Options Modify UI....
                //}
            }
            else if (this.MouseLeftButton.IsPressed)
            {
                if (tileData == null || this.IsExpanding)
                    return;

                this.Set(x, y, tileData, this.EditorTileSelection.Rotation);
                this.TryExpandSize(x, y);
            }
            else if (this.MouseRightButton.IsPressed)
            {
                if (this.MouseRightButton.WasPressed)
                {
                    this.NotifyConfig(null);
                }

                this.Unset(x, y, tileData);
            }

            if (!this.MouseLeftButton.IsPressed)
                this.IsExpanding = false;

            this.EnableHoverTile();
            this.EditorHoverTile.transform.position = new Vector3(
                x - this.ScaleX / 2,
                y - this.ScaleY / 2, 0);

            this.EditorHoverTile.Set(this.EditorTileSelection.ActiveElement, this.EditorTileSelection.Rotation);
        }

        #region Tile Operations

        private void EnableHoverTile()
        {
            this.EditorHoverTile.SetActive(true);
        }

        private void DisableHoverTile()
        {
            this.EditorHoverTile.SetActive(false);
        }

        private void ClampPosition(Vector3Int position, out int x, out int y)
        {
            this.ClampPosition(position.x, position.y, out x, out y);
        }

        private void ClampPosition(int x, int y, out int oX, out int oY)
        {
            oX = x.Clamp(0, this.ScaleX - 1);
            oY = y.Clamp(0, this.ScaleY - 1);
        }

        private Vector3Int GetCursorTileMapPosition()
        {
            Vector3 point = this.SceneCamera.ScreenToWorldPoint(this.MousePosition.Read<Vector2>());
            Vector3Int tilePosition = this.Tilemap_Foreground.WorldToCell(point);

            //tilePosition.x += this.Settings.EditorLevelScaleX / 2;
            //tilePosition.y += this.Settings.EditorLevelScaleY / 2;

            return tilePosition;
        }

        public void Set(int x, int y, EditorTileData tileData, Rotation rotation)
        {
            if (tileData == null)
            {
                this.Unset(x, y, tileData);
                return;
            }

            EditorTileState current = this.Tiles[x, y];

            EditorTileData currentTileData = tileData.IsBackground ? current?.Background : current?.Foreground;

            if (currentTileData?.Tile.Type == tileData.Tile.Type)
                return;

            bool updateLocations = !tileData.IsInfinite;

            int count = 0;
            TileType tileType = tileData.Tile.Type;

            if (updateLocations)
            {
                if (this.TileLocations.ContainsKey(tileType))
                {
                    List<Int2D> locations;
                    if (this.TileLocations.TryGetValue(tileType, out locations))
                    {
                        foreach (Int2D location in locations)
                            if (location.X == x && location.Y == y)
                                return;
                    }

                    count = this.TileLocations[tileType].Count;
                }


                if (count <= 0)
                {
                    // Do Nothing
                }
                //else if (tileData.LevelLimit == 1)
                //{
                //    // Only 1 allowed, replace..?
                //    Int2D location = this.TileLocations[tileType][0];
                //    this.Unset(location.X, location.Y);
                //}
                else if (count >= tileData.LevelLimit)
                {
                    return;
                }
            }

            this.Unset(x, y, tileData);

            this.SetTile(x, y, tileData, rotation);

            //if (tileData.Tile.CanRotate)
            //{

            //}
            //else
            //{
            //    this.Tilemap.SetTile(new Vector3Int(x, y, 0), tileData.EditorGridTile);
            //    this.Tiles[x, y] = new EditorTileState(tileData, Rotation.North);
            //}

            count = 0;

            if (updateLocations)
            {
                // Update Location
                if (!this.TileLocations.ContainsKey(tileType))
                    this.TileLocations.Add(tileType, new List<Int2D>());

                this.TileLocations[tileType].Add(new Int2D(x, y));
                count = this.TileLocations[tileType].Count;
            }

            EditorLevelManager.OnTileSet?.Invoke(tileData, tileType, count);
        }

        private void SetTile(int x, int y, EditorTileData tileData, Rotation tileRotation)
        {
            Tilemap tilemap;
            EditorTileState currentState = this.Tiles[x, y];

            if (!tileData.Tile.CanRotate)
                tileRotation = Rotation.North;

            if (currentState == null)
            {
                this.Tiles[x, y] = currentState = new EditorTileState();
            }

            if (tileData.IsBackground)
            {
                tilemap = this.Tilemap_Background;
                currentState.SetBackground(tileData);
                currentState.SetBackgroundRotation(tileRotation);
            }
            else
            {
                tilemap = this.Tilemap_Foreground;
                currentState.SetForeground(tileData);
                currentState.SetForegroundRotation(tileRotation);
                this.SetConfig(currentState, tileData.Config);
            }

            if (tileData.Tile.CanRotate && tileRotation != Rotation.North)
            {
                tilemap.SetTile(
                    new TileChangeData(
                        new Vector3Int(x, y, 0),
                        tileData.EditorGridTile,
                        Color.white,
                        Matrix4x4.Rotate(Quaternion.Euler(0, 0, tileRotation.ToDegree()))), true);
            }
            else
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tileData.EditorGridTile);
            }

            this.NotifyConfig(null);
        }

        private void SetConfig(int x, int y, TileConfig config)
        {
            this.ClampPosition(x, y, out int fX, out int fY);

            // Do not auto adjust coordinates... Something might be off!
            if (fX != x || fY != y)
                return;

            EditorTileState state = this.Tiles[x, y];
            this.SetConfig(state, config);
        }

        private void SetConfig(EditorTileState currentState, EditorTileConfigData editorTileConfigData)
        {
            this.SetConfig(currentState, editorTileConfigData?.CreateTileConfig());
        }

        private void SetConfig(EditorTileState currentState, TileConfig config)
        {
            currentState.SetConfig(config);
        }

        private void TryExpandSize(int x, int y)
        {
            int expandCheck = this.Settings.AutoExpansionThreshold; // Number of units to expand by in each direction
            int expandBy = this.Settings.AutoExpansionValue; // Number of units to expand by in each direction

            int currentWidth = this.ScaleX;
            int currentHeight = this.ScaleY;

            bool expandLeft = false;
            bool expandRight = false;
            bool expandTop = false;
            bool expandBottom = false;

            // Check if expansion is needed based on the (x, y) coordinate
            if (x < expandCheck)
            {
                // Maybe later...
                // expandLeft = true;
            }
            else if (x >= currentWidth - expandCheck)
            {
                expandRight = true;
            }

            if (y >= currentHeight - expandCheck)
            {
                expandTop = true;
            }
            else if (y < expandCheck)
            {
                // Maybe later...
                // expandBottom = true;
            }

            if (expandLeft || expandRight || expandTop || expandBottom)
            {
                // Debug.Log($"Expand Reason: L={expandLeft}; R={expandRight}; T={expandTop}; B={expandBottom};");

                int expandByX = (expandRight ? (this.Settings.AutoExpansionScaleX - currentWidth).Min(expandBy) : 0);
                int expandByY = (expandTop ? (this.Settings.AutoExpansionScaleY - currentHeight).Min(expandBy) : 0);

                int newWidth = currentWidth + expandByX;
                int newHeight = currentHeight + expandByY;

                while (newWidth * newHeight > this.Settings.AutoExpansionSize)
                {
                    if (expandByX > 1)
                    {
                        expandByX /= 2;
                        newWidth = currentWidth + expandByX;
                    }
                    if (expandByY > 1)
                    {
                        expandByY /= 2;
                        newHeight = currentHeight + expandByY;
                    }

                    if (expandByX <= 1 && expandByY <= 1)
                    {
                        // Debug.Log("Can not expand: Step size too small!");
                        return;
                    }
                }

                if (expandByX <= 0 && expandByY <= 0)
                {
                    // Debug.Log("Can not expand: Scale reached!");
                    return;
                }

                //int newWidth = currentWidth + (expandLeft ? expandBy : 0) + (expandRight ? expandBy : 0);
                //int newHeight = currentHeight + (expandTop ? expandBy : 0) + (expandBottom ? expandBy : 0);

                // Debug.Log($"Expand Change: CW={currentWidth}; NW={newWidth}; CH={currentHeight}; NH={newHeight};");

                this.IsExpanding = true;

                // Create a new larger array
                EditorTileState[,] tileStates = new EditorTileState[newWidth, newHeight];

                // Copy the elements from the old array to the new one
                for (int i = 0; i < currentWidth; i++)
                {
                    for (int j = 0; j < currentHeight; j++)
                    {
                        //tileStates[i + (expandLeft ? expandBy : 0), j + (expandBottom ? expandBy : 0)] = this.Tiles[i, j];
                        tileStates[i, j] = this.Tiles[i, j];
                    }
                }

                Debug.Log("Expand Tiles");

                this.Tiles = tileStates;

                this.SetScale(newWidth, newHeight);
            }
        }

        public void Unset(int x, int y, EditorTileData tileData)
        {
            EditorTileState current = this.Tiles[x, y];

            if (current == null)
                return;

            EditorTileData currentTileData = null;
            Tilemap tilemap = null;
            bool setNull = false;

            if (tileData.IsBackground)
            {
                currentTileData = current.Background;
                tilemap = this.Tilemap_Background;
                setNull = current.Foreground == null;

                current.SetBackground(null);
            }
            else
            {
                currentTileData = current.Foreground;
                tilemap = this.Tilemap_Foreground;
                setNull = current.Background == null;

                current.SetForeground(null);
                current.SetForegroundRotation(Rotation.North);
                current.SetConfig(null);
            }

            // No tile data at spot, return
            if (currentTileData == null)
                return;

            TileType tileType = currentTileData.Tile.Type;
            int count = 0;

            if (!currentTileData.IsInfinite)
            {
                if (this.TileLocations.ContainsKey(tileType))
                {
                    Int2D remove = new Int2D(x, y);

                    this.TileLocations[tileType].Remove(remove);
                    count = this.TileLocations[tileType].Count;
                }
            }

            tilemap.SetTile(new Vector3Int(x, y, 0), null);

            if (setNull)
            {
                this.Tiles[x, y] = null;
            }

            EditorLevelManager.OnTileUnset?.Invoke(currentTileData, tileType, count);
        }

        public EditorTileState Get(int x, int y)
        {
            this.ClampPosition(x, y, out x, out y);

            return this.Tiles[x, y];
        }

        public int GetTileCount(EditorTileData editorTileData)
        {
            if (editorTileData == null) return 0;

            TileType key = editorTileData.Tile.Type;

            if (this.TileLocations.ContainsKey(key))
            {
                return this.TileLocations[key].Count;
            }

            return 0;
        }

        private void EditorTileSelectionManager_OnTileSelect()
        {
            // var editorTileData = this.EditorTileSelection.ActiveElement;
            this.NotifyConfig(null);
        }

        private void EditorTileSelectionManager_ActiveElementChanged(EditorTileData obj)
        {
            this.NotifyConfig(null);
        }

        private void NotifyConfig(TileConfig config)
        {
            OnEditorConfigSelected?.Invoke(config);
        }

        #endregion

        #region Level State Operations

        private void CreateLevelState()
        {
            // Open a new/clean Editor
            this.SetLevelState(this.LevelManager.Create());

            // Trigger an event to update the level code in the Exchange.
            string code = this.LevelState.Code;
            this.Exchange.SetCode(code);
            this.Exchange.SetAutoload(code.Length > 0);
            this.StartModal.Open(this.ModalCanvas.transform);
        }

        public void LoadLevelState()
        {
            this.LevelManager.Load(this.Exchange.CodeToLoad, OnLevelStateLoaded);

            void OnLevelStateLoaded(LevelState levelState)
            {
                this.SetLevelState(levelState);

                for (int y = 0; y < this.ScaleY; y++)
                {
                    for (int x = 0; x < this.ScaleX; x++)
                    {
                        TileState tileState = this.LevelState.Tiles[x, y];

                        EditorTileData editorTileData = null;

                        if (tileState == null)
                        {
                            this.Set(x, y, null, Rotation.North);
                            continue;
                        }

                        editorTileData = this.EditorTileDataPool.Get(tileState.Foreground);
                        this.Set(x, y, editorTileData, tileState.ForegroundRotation);

                        if (tileState.Config != null)
                            this.SetConfig(x, y, tileState.Config);

                        editorTileData = this.EditorTileDataPool.Get(tileState.Background);
                        if (editorTileData == null)
                            continue;
                        this.Set(x, y, editorTileData, tileState.BackgroundRotation);
                    }
                }
            }
        }

        public void SaveLevelState(bool uploadToRemote = true)
        {
            this.ScreenshotCamera.orthographicSize = this.SceneCamera.orthographicSize;
            Texture2D screenshot = CreateScreenshot(this.ScreenshotCamera);

            this.LevelState.SetScale(this.ScaleX, this.ScaleY);
            this.LevelState.SetTiles(this.Tiles);
            this.LevelManager.SaveState(this.LevelState, uploadToRemote);
            this.LevelManager.SaveScreenshot(this.LevelState.Code, screenshot);

            Texture2D CreateScreenshot(Camera cam)
            {
                RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
                cam.targetTexture = screenTexture;
                RenderTexture.active = screenTexture;
                cam.Render();
                Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height);
                screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                RenderTexture.active = null;


                return screenshotTexture;
            }
        }

        private void SetLevelState(LevelState levelState)
        {
            this.LevelState = levelState;

            this.SetScale(this.LevelState.ScaleX, this.LevelState.ScaleY);

            this.Tiles = new EditorTileState[this.ScaleX, this.ScaleY];
        }

        public bool IsLevelValid()
        {
            return this.LevelState.IsLevelValid();
        }

        private void SetScale(int x, int y)
        {
            this.ScaleX = x;
            this.ScaleY = y;

            Debug.Log($"{this.ScaleX}/{this.ScaleY}");

            this.EditorGrid.size = new Vector2(this.ScaleX, this.ScaleY);
            this.Tilemap_Foreground.transform.localPosition = new Vector3(
                this.ScaleX / -2,
                this.ScaleY / -2, 0);

            this.Tilemap_Background.transform.localPosition = new Vector3(
                this.ScaleX / -2,
                this.ScaleY / -2, 0);

            OnEditorLevelScaleChanged?.Invoke(this.ScaleX, this.ScaleY);
        }
        #endregion

        private void ShowInvalidLevelModal()
        {
            this.InvalidModal.Open(this.ModalCanvas.transform, true);
        }

        public override void DoOnEnable()
        {
            LevelEditorScreen.SaveAndPlayComponent.OnInvalidLevelRequest += ShowInvalidLevelModal;
        }

        public override void DoOnDisable()
        {
            LevelEditorScreen.SaveAndPlayComponent.OnInvalidLevelRequest -= ShowInvalidLevelModal;
        }
    }
}
