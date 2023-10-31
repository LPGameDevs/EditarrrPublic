using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "LeverBlockConfigData", menuName = "Data/Tiles/new LeverBlockConfig Data")]
    public class LeverBlockConfigData : EditorTileConfigData<LeverBlockConfig>
    {
        [field: SerializeField] public int Channel { get; private set; }
        [field: SerializeField] public bool Inverted { get; private set; }

        public override LeverBlockConfig CreateConfig()
        {
            return new LeverBlockConfig(this.Channel, this.Inverted);
        }
    }
}
