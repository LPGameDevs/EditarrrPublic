using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "LeverBlockConfigData", menuName = "Data/Tiles/new LeverBlockConfig Data")]
    public class LeverBlockConfigData : EditorTileConfigData<LeverBlockConfig>
    {
        [field: SerializeField] public int Channel { get; private set; }
        [field: SerializeField] public bool Inverted { get; private set; }
        [SerializeField] private List<TileBase> _overlayTiles = new();
        public static List<TileBase> LeverOverlayTiles { get; private set; } = new();

        public override LeverBlockConfig CreateConfig()
        {
            return new LeverBlockConfig(this.Channel, this.Inverted);
        }

        private void OnValidate()
        {
            LeverOverlayTiles = _overlayTiles;
        }

        private void Awake()
        {
            LeverOverlayTiles = _overlayTiles;
        }
    }
}
