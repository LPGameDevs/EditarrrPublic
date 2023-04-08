using Editarrr.Input;
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
        [field: SerializeField, Header("Settings")] public EditorLevelSettings Settings { get; private set; }

        [field: SerializeField, Header("Pools")] public EditorPrefabPool PrefabPool { get; private set; }

        [field: SerializeField, Header("Managers")] public EditorTileSelectionManager EditorTileSelection { get; private set; }


        #region Input
        [field: SerializeField, Header("Input")] private InputValue MousePosition { get; set; }
        [field: SerializeField] private InputValue MouseLeftButton { get; set; }
        [field: SerializeField] private InputValue MouseRightButton { get; set; }
        #endregion

        Dictionary<TileType, List<Int2D>> TileLocations { get; set; }


        private EditorTileState[,] Tiles { get; set; }

        private LevelEditorStage Stage { get; set; }



        // From System
        private Camera SceneCamera { get; set; }
        private Tilemap Tilemap { get; set; }




        public void SetSceneCamera(Camera camera)
        {
            this.SceneCamera = camera;
        }

        public void SetTilemap(Tilemap tilemap)
        {
            this.Tilemap = tilemap;
        }

        public override void DoAwake()
        {
            this.TileLocations = new Dictionary<TileType, List<Int2D>>();

            SpriteRenderer editorGrid = GameObject.Instantiate(this.PrefabPool.EditorGrid);
            editorGrid.transform.position = Vector3.zero;
            editorGrid.size = new Vector2(this.Settings.EditorLevelScaleX, this.Settings.EditorLevelScaleY);

            this.Tiles = new EditorTileState[this.Settings.EditorLevelScaleX, this.Settings.EditorLevelScaleY];

            this.Tilemap.transform.localPosition = new Vector3(this.Settings.EditorLevelScaleX / -2, this.Settings.EditorLevelScaleY / -2, 0);
        }

        public override void DoUpdate()
        {
            if (this.MouseLeftButton.IsPressed && !this.EditorTileSelection.IsUIHover)
            {
                this.ClampPosition(this.GetCursorTilePosition(), out int x, out int y);

                this.Set(x, y, this.EditorTileSelection.ActiveElement);
            }
            else if (this.MouseRightButton.IsPressed)
            {
                this.ClampPosition(this.GetCursorTilePosition(), out int x, out int y);

                this.Unset(x, y);
            }
        }

        private void ClampPosition(Vector3Int position, out int x, out int y)
        {
            x = position.x.Clamp(0, this.Settings.EditorLevelScaleX);
            y = position.y.Clamp(0, this.Settings.EditorLevelScaleY);
        }

        private Vector3Int GetCursorTilePosition()
        {
            Vector3 point = this.SceneCamera.ScreenToWorldPoint(MousePosition.Read<Vector2>());
            Vector3Int tilePosition = this.Tilemap.WorldToCell(point);

            //tilePosition.x += this.Settings.EditorLevelScaleX / 2;
            //tilePosition.y += this.Settings.EditorLevelScaleY / 2;

            return tilePosition;
        }

        private void UpdateInput()
        {

        }

        public void Set(int x, int y, EditorTileData tileData)
        {
            TileType tileType = tileData.Tile.Type;
            bool updateLocations = !tileData.IsInfinite;

            if (updateLocations)
            {
                int count = 0;

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
                    Matrix4x4.Rotate(Quaternion.Euler(0, 0, this.EditorTileSelection.Rotation.ToDegree()))), true);

                this.Tiles[x, y] = new EditorTileState(tileData, this.EditorTileSelection.Rotation);
            }
            else
            {
                this.Tilemap.SetTile(new Vector3Int(x, y, 0), tileData.EditorGridTile);
                this.Tiles[x, y] = new EditorTileState(tileData, Rotation.North);
            }

            if (updateLocations)
            {
                // Update Location
                if (!this.TileLocations.ContainsKey(tileType))
                    this.TileLocations.Add(tileType, new List<Int2D>());

                this.TileLocations[tileType].Add(new Int2D(x, y));
            }
        }

        public void Unset(int x, int y)
        {
            EditorTileState current = this.Tiles[x, y];

            if (current == null)
                return;

            if (!current.TileData.IsInfinite)
            {
                TileType tileType = current.TileData.Tile.Type;

                if (this.TileLocations.ContainsKey(tileType))
                {
                    Int2D remove = new Int2D(x, y);

                    Debug.Log($"Remove: {this.TileLocations[tileType].Count}");
                    this.TileLocations[tileType].Remove(remove);
                    Debug.Log($"Removed: {this.TileLocations[tileType].Count}");
                }
            }

            this.Tilemap.SetTile(new Vector3Int(x, y, 0), null);
            this.Tiles[x, y] = null;

            // if(current.TileData.Tile.)
        }

        private enum LevelEditorStage
        {
            None,
            Paint,
            Remove
        }
    }
}
