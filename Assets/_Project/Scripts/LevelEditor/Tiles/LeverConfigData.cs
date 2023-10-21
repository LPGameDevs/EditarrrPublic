using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "LeverConfigData", menuName = "Data/Tiles/new LeverConfig Data")]
    public class LeverConfigData : EditorTileConfigData<LeverConfig>
    {
        [field: SerializeField] public int Channel { get; private set; }

        public override LeverConfig CreateConfig()
        {
            return new LeverConfig(this.Channel);
        }
    }
}
