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



        public override void Paint(Tilemap tilemap, Vector3Int position)
        {
            throw new NotImplementedException();
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
