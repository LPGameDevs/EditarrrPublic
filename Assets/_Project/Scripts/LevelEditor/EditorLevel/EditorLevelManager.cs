using System;
using System.Collections.Generic;
using Editarrr.Input;
using Editarrr.Level;
using Editarrr.Managers;
using Editarrr.Misc;
using Editarrr.Utilities;
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
        private Tilemap Tilemap { get; set; }

        private EditorHoverTile EditorHoverTile { get; set; }

        public void SetSceneCamera(Camera camera)
        {
            this.SceneCamera = camera;
        }

        public void SetScreenshotCamera(Camera camera)
        {
            this.ScreenshotCamera = camera;
        }

        public void SetTilemap(Tilemap tilemap)
        {
            this.Tilemap = tilemap;
        }


        public override void DoAwake()
        {
            LevelManager.DoAwake();

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

            if (this.MouseLeftButton.WasPressed)
            {
                EditorTileState state = this.Get(x, y);

                if (state != null &&
                    state.TileData == tileData &&
                    state.Rotation == this.EditorTileSelection.Rotation)
                {
                    // Use same Rotation in if ???
                    // Change Options/Variations/Show Options Modify UI....
                }
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
                this.Unset(x, y);
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
            Vector3 point = this.SceneCamera.ScreenToWorldPoint(MousePosition.Read<Vector2>());
            Vector3Int tilePosition = this.Tilemap.WorldToCell(point);

            //tilePosition.x += this.Settings.EditorLevelScaleX / 2;
            //tilePosition.y += this.Settings.EditorLevelScaleY / 2;

            return tilePosition;
        }

        public void Set(int x, int y, EditorTileData tileData, Rotation rotation)
        {
            if (tileData == null)
            {
                this.Unset(x, y);
                return;
            }

            EditorTileState current = this.Tiles[x, y];
            TileType tileType = tileData.Tile.Type;

            if (current?.TileData.Tile.Type == tileData.Tile.Type)
                return;

            bool updateLocations = !tileData.IsInfinite;

            int count = 0;

            if (updateLocations)
            {
                if (this.TileLocations.ContainsKey(tileType))
                {
                    List<Int2D> locations;
                    if (TileLocations.TryGetValue(tileType, out locations))
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

            this.Unset(x, y);

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

        private void SetTile(int x, int y, EditorTileData tileData, Rotation rotation)
        {
            if (tileData.Tile.CanRotate)
            {
                this.Tiles[x, y] = new EditorTileState(tileData, rotation);
                this.Tilemap.SetTile(
                    new TileChangeData(
                        new Vector3Int(x, y, 0),
                        tileData.EditorGridTile,
                        Color.white,
                        Matrix4x4.Rotate(Quaternion.Euler(0, 0, rotation.ToDegree()))), true);
            }
            else
            {
                this.Tilemap.SetTile(new Vector3Int(x, y, 0), tileData.EditorGridTile);
                this.Tiles[x, y] = new EditorTileState(tileData, Rotation.North);
            }
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

        public void Unset(int x, int y)
        {
            EditorTileState current = this.Tiles[x, y];

            if (current == null)
                return;

            TileType tileType = current.TileData.Tile.Type;
            int count = 0;

            if (!current.TileData.IsInfinite)
            {
                if (this.TileLocations.ContainsKey(tileType))
                {
                    Int2D remove = new Int2D(x, y);

                    this.TileLocations[tileType].Remove(remove);
                    count = this.TileLocations[tileType].Count;
                }
            }

            this.Tilemap.SetTile(new Vector3Int(x, y, 0), null);
            this.Tiles[x, y] = null;

            EditorLevelManager.OnTileUnset?.Invoke(current.TileData, tileType, count);
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

                        editorTileData = this.EditorTileDataPool.Get(tileState.Type);

                        this.Set(x, y, editorTileData, tileState.Rotation);
                    }
                }
            }
        }

        public void SaveLevelState()
        {
            this.ScreenshotCamera.orthographicSize = this.SceneCamera.orthographicSize;
            Texture2D screenshot = CreateScreenshot(this.ScreenshotCamera);
            this.LevelState.SetScale(this.ScaleX, this.ScaleY);
            this.LevelState.SetTiles(this.Tiles);
            this.LevelManager.SaveState(LevelState);
            this.LevelManager.SaveScreenshot(LevelState.Code, screenshot);

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

        private void SetScale(int x, int y)
        {
            this.ScaleX = x;
            this.ScaleY = y;

            Debug.Log($"{this.ScaleX}/{this.ScaleY}");

            this.EditorGrid.size = new Vector2(this.ScaleX, this.ScaleY);
            this.Tilemap.transform.localPosition = new Vector3(
                this.ScaleX / -2,
                this.ScaleY / -2, 0);

            OnEditorLevelScaleChanged?.Invoke(this.ScaleX, this.ScaleY);
        }
        #endregion
    }
}
