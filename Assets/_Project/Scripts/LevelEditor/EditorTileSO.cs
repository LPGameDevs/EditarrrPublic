using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelEditor
{
    public enum TilemapPainterLayer
    {
        Background = 0,
        Platform = 10,
        Elements = 20,
        Damage = 30,
    }

    [CreateAssetMenu()]
    public class EditorTileSO : ScriptableObject, ISerializationCallbackReceiver
    {

        public Transform prefab;

        public Sprite itemFrameImage;
        public int initialItemCount;
        [HideInInspector]
        public int currentItemCount;


        public TileBase Tile;
        public TilemapPainterLayer Layer;
        public bool Infinite = false;

        public int getCurrentItemCount()
        {
            return currentItemCount;
        }

        public void reduceItemCount()
        {
            currentItemCount--;
        }

        public void increaseItemCount()
        {
            currentItemCount++;
        }

        public Sprite getItemFrameImage()
        {
            return itemFrameImage;
        }

        public bool showCount()
        {
            return !Infinite;
        }

        public bool isInfinite()
        {
            return Infinite;
        }

        public Transform getPrefab()
        {
            return prefab;
        }

        public string getName()
        {
            return name;
        }

        public virtual bool HasOptions()
        {
            return false;
        }

        public TilemapPainterLayer getLayer()
        {
            return Layer;
        }

        public TileBase getTile()
        {
            return Tile;
        }

        public virtual Vector3Int? GetPlacedTilePosition(Tilemap tilemap, Vector3Int position)
        {
            return position;
        }


        public virtual bool canPaint(Tilemap currentTilemap, Vector3Int selectedTile)
        {
            var tile = currentTilemap.GetTile(selectedTile);
            if (tile && tile.name == Tile.name)
            {
                return false;
            }

            return true;
        }

        public virtual bool canUnPaint(Tilemap currentTilemap, Vector3Int selectedTile)
        {
            var tile = currentTilemap.GetTile(selectedTile);
            if (tile == null || tile.name != Tile.name)
            {
                return false;
            }

            return true;
        }

        public virtual void Paint(Tilemap tilemap, Vector3Int position)
        {
            tilemap.SetTile(position, Tile);
        }

        public virtual void UnPaint(Tilemap tilemap, Vector3Int position)
        {
            tilemap.SetTile(position, null);
        }

        public virtual void Highlight(Tilemap currentTilemap, Vector3Int selectedTile, Color color)
        {
            return;
        }

        public void OnAfterDeserialize()
        {
            currentItemCount = initialItemCount;
        }

        public void OnBeforeSerialize()
        {
            // Noting to do here.
        }
    }
}
