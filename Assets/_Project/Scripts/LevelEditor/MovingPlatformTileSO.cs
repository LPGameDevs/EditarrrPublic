using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelEditor
{
    [CreateAssetMenu()]
    public class MovingPlatformTileSO : EditorTileSO
    {
        public string TileMatch;
        public TileBase TileLeft, TileRight;
        public TileBase TileArrowLeft, TileArrowRight;
        public Color HighlightColor;

        public override bool HasOptions()
        {
            return true;
        }

        public override TileOptions NextOption(TileOptions currentOption)
        {
            if (currentOption == null)
            {
                currentOption = new TileOptions();
            }

            MovingPlatformOptions platformOptions = JsonUtility.FromJson<MovingPlatformOptions>(currentOption.options);

            if (platformOptions == null)
            {
                platformOptions = new MovingPlatformOptions();
            }

            platformOptions.Toggle();
            return new TileOptions(JsonUtility.ToJson(platformOptions));
        }

        public override void Paint(Tilemap tilemap, Vector3Int position, TileOptions options)
        {
            position = GetPlacedTilePosition(tilemap, position) ?? position;

            MovingPlatformOptions platformOptions = options != null ? JsonUtility.FromJson<MovingPlatformOptions>(options.options) : new MovingPlatformOptions();
            TileBase leftTile = platformOptions.direction == 1 ? TileLeft : TileArrowLeft;
            TileBase rightTile = platformOptions.direction == 1 ? TileArrowRight : TileRight;

            Debug.Log(leftTile);
            Debug.Log(rightTile);

            tilemap.SetTile(position + Vector3Int.left, leftTile);
            tilemap.SetTile(position, Tile);
            tilemap.SetTile(position + Vector3Int.right, rightTile);
        }

        public override void UnPaint(Tilemap tilemap, Vector3Int position)
        {
            position = GetPlacedTilePosition(tilemap, position) ?? position;
            tilemap.SetTile(position + Vector3Int.left, null);
            tilemap.SetTile(position, null);
            tilemap.SetTile(position + Vector3Int.right, null);
        }

        public override Vector3Int? GetPlacedTilePosition(Tilemap tilemap, Vector3Int position)
        {
            Vector3Int testPosition = position;
            TileBase tile = tilemap.GetTile(testPosition);

            if (tile != null && tile.name == Tile.name)
            {
                return testPosition;
            }

            testPosition = position + Vector3Int.left;
            tile = tilemap.GetTile(testPosition);
            if (tile != null && tile.name == Tile.name)
            {
                return testPosition;
            }

            testPosition = position + Vector3Int.right;
            tile = tilemap.GetTile(testPosition);
            if (tile != null && tile.name == Tile.name)
            {
                return testPosition;
            }

            return null;
        }

        public override bool canPaint(Tilemap currentTilemap, Vector3Int selectedTile)
        {
            if (GetPlacedTilePosition(currentTilemap, selectedTile) != null)
            {
                // return true;
            }

            var tile = currentTilemap.GetTile(selectedTile);
            if (tile != null && tile.name.Substring(0, TileMatch.Length) == TileMatch)
            {
                return false;
            }

            tile = currentTilemap.GetTile(selectedTile + Vector3Int.left);
            if (tile != null && tile.name.Substring(0, TileMatch.Length) == TileMatch)
            {
                return false;
            }

            tile = currentTilemap.GetTile(selectedTile + Vector3Int.right);
            if (tile != null && tile.name.Substring(0, TileMatch.Length) == TileMatch)
            {
                return false;
            }

            return true;
        }

        public override bool canUnPaint(Tilemap currentTilemap, Vector3Int selectedTile)
        {
            var tile = currentTilemap.GetTile(selectedTile);
            if (tile != null && tile.name.Substring(0, TileMatch.Length) == TileMatch)
            {
                return true;
            }

            return false;
        }

        public override void Highlight(Tilemap tilemap, Vector3Int position, Color color)
        {
            tilemap.SetTile(position + Vector3Int.left, TileLeft);
            tilemap.SetTileFlags(position + Vector3Int.left, TileFlags.None);
            tilemap.SetColor(position + Vector3Int.left, color);

            tilemap.SetTile(position, Tile);
            tilemap.SetTileFlags(position, TileFlags.None);
            tilemap.SetColor(position, color);

            tilemap.SetTile(position + Vector3Int.right, TileRight);
            tilemap.SetTileFlags(position + Vector3Int.right, TileFlags.None);
            tilemap.SetColor(position + Vector3Int.right, color);
        }
    }

    [Serializable]
    public class MovingPlatformOptions
    {
        public int direction = 1;

        public void Toggle()
        {
            direction *= -1;
        }
    }
}
