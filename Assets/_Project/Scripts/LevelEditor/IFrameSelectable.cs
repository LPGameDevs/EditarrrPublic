using LevelEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IFrameSelectable
{
    int getCurrentItemCount();
    Sprite getItemFrameImage();

    bool showCount();

    TileBase getTile();

    void Paint(Tilemap tilemap, Vector3Int position, TileOptions options);
    void UnPaint(Tilemap tilemap, Vector3Int position);

    TilemapPainterLayer getLayer();

    bool isInfinite();

    Transform getPrefab();
    void reduceItemCount();
    void increaseItemCount();
    string getName();
    bool canPaint(Tilemap currentTilemap, Vector3Int selectedTile);
    bool canUnPaint(Tilemap currentTilemap, Vector3Int selectedTile);
    Vector3Int? GetPlacedTilePosition(Tilemap tilemap, Vector3Int selectedTile);
    bool HasOptions();
    TileOptions NextOption(TileOptions currentOption);
    void Highlight(Tilemap currentTilemap, Vector3Int selectedTile, Color color);
}
