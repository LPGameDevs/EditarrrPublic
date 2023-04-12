﻿using Editarrr.Input;
using Editarrr.Level;
using Editarrr.Managers;
using Editarrr.Misc;
using Editarrr.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Level Manager", menuName = "Managers/Editor/new Editor Level Manager")]
    public class EditorLevelManager : ManagerComponent
    {
        public static TileSet OnTileSet { get; set; }
        public static TileSet OnTileUnset { get; set; }

        [field: SerializeField, Header("Settings")] private EditorLevelSettings Settings { get; set; }

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

        private EditorTileState[,] Tiles { get; set; }


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
            this.ClearEvents();

            this.TileLocations = new Dictionary<TileType, List<Int2D>>();

            SpriteRenderer editorGrid = GameObject.Instantiate(this.PrefabPool.EditorGrid);
            editorGrid.transform.position = Vector3.zero;
            editorGrid.size = new Vector2(this.Settings.EditorLevelScaleX, this.Settings.EditorLevelScaleY);

            this.Tiles = new EditorTileState[this.Settings.EditorLevelScaleX, this.Settings.EditorLevelScaleY];

            this.Tilemap.transform.localPosition = new Vector3(this.Settings.EditorLevelScaleX / -2, this.Settings.EditorLevelScaleY / -2, 0);

            this.EditorHoverTile = GameObject.Instantiate(this.PrefabPool.EditorHoverTile);
            this.DisableHoverTile();
        }

        public override void DoStart()
        {

            this.CreateLevelState();

        }


        private void ClearEvents()
        {
            EditorLevelManager.OnTileSet = null;
            EditorLevelManager.OnTileUnset = null;
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
                if (tileData == null)
                    return;

                this.Set(x, y, tileData, this.EditorTileSelection.Rotation);
            }
            else if (this.MouseRightButton.IsPressed)
            {
                this.Unset(x, y);
            }

            this.EnableHoverTile();
            this.EditorHoverTile.transform.position = new Vector3(x - this.Settings.EditorLevelScaleX / 2, y - this.Settings.EditorLevelScaleY / 2, 0);

            this.EditorHoverTile.Set(this.EditorTileSelection.ActiveElement, this.EditorTileSelection.Rotation);
        }

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
            oX = x.Clamp(0, this.Settings.EditorLevelScaleX);
            oY = y.Clamp(0, this.Settings.EditorLevelScaleY);
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

            TileType tileType = tileData.Tile.Type;
            bool updateLocations = !tileData.IsInfinite;

            int count = 0;

            if (updateLocations)
            {
                if (this.TileLocations.ContainsKey(tileType))
                    count = this.TileLocations[tileType].Count;

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

            if (tileData.Tile.CanRotate)
            {
                this.Tilemap.SetTile(
                new TileChangeData(
                    new Vector3Int(x, y, 0),
                    tileData.EditorGridTile,
                    Color.white,
                    Matrix4x4.Rotate(Quaternion.Euler(0, 0, rotation.ToDegree()))), true);

                this.Tiles[x, y] = new EditorTileState(tileData, rotation);
            }
            else
            {
                this.Tilemap.SetTile(new Vector3Int(x, y, 0), tileData.EditorGridTile);
                this.Tiles[x, y] = new EditorTileState(tileData, Rotation.North);
            }

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


        #region Level State

        private void CreateLevelState()
        {
            if (this.Exchange.LoadOnStart)
            {
                this.LevelManager.Load(this.Exchange.CodeToLoad, this.LoadLevelState);
            }
            else
            {
                // Open a new/clean Editor
                this.LevelManager.Create();
            }
        }

        public void LoadLevelState(LevelState levelState)
        {
            int scaleX = levelState.ScaleX;
            int scaleY = levelState.ScaleY;

            for (int y = 0; y < scaleY; y++)
            {
                for (int x = 0; x < scaleX; x++)
                {
                    TileState tileState = levelState.Tiles[x, y];

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

        public void Save()
        {
            this.ScreenshotCamera.orthographicSize = this.SceneCamera.orthographicSize;
            Texture2D screenshot = this.CreateScreenshot(this.ScreenshotCamera);
            this.LevelManager.Save(this.Tiles, screenshot);
        }

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

        #endregion


        public delegate void TileSet(EditorTileData data, TileType tileType, int inLevel);
        public delegate void TileUnset(EditorTileData data, TileType tileType, int inLevel);
    }
}