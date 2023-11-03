using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "MovingPlatformConfigData", menuName = "Data/Tiles/new MovingPlatformConfig Data")]
    public class MovingPlatformConfigData : EditorTileConfigData<MovingPlatformConfig>
    {
        [field: SerializeField] public int Distance { get; private set; }
        [field: SerializeField] public bool MoveRight { get; private set; }

        public override MovingPlatformConfig CreateConfig()
        {
            return new MovingPlatformConfig(this.Distance, this.MoveRight);
        }
    }
}
