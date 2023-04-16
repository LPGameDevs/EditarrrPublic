using Editarrr.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Tile Data", menuName = "Data/Tiles/new Tile Data")]
    public class TileData : ScriptableObject
    {
        private const string Documentation =
            "Tile data is information relevant to something that can be placed in the game.\r\n" +
            "What type of object it is, what to spawn, and any custom behaviour/setup required for it to function.";

        [field: SerializeField, Header("Game Object Settings"), Info(Documentation)] public GameObject GameObject { get; private set; }

        [field: SerializeField, Header("Tile Map Settings")] public TileBase TileMapTileBase { get; private set; }


        [field: SerializeField, Header("General Settings")] public TileType Type { get; private set; }
        [field: SerializeField] public bool CanRotate { get; private set; }

    }
}
