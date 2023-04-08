using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Tile Data", menuName = "Data/Tiles/new Tile Data")]
    public class TileData : ScriptableObject
    {
        [field: SerializeField, Header("Game Object Settings")] public GameObject GameObject { get; private set; }
        [field: SerializeField] public Sprite GameObjectSprite { get; private set; }
        [field: SerializeField] public bool CanRotate { get; private set; }

        [field: SerializeField, Header("Tile Map Settings")] public TileBase TileMapTileBase { get; private set; }


        [field: SerializeField, Header("General Settings")] public TileType Type { get; private set; }
        
    }
}
