using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "CrackedPlankConfigData", menuName = "Data/Tiles/new CrackedPlankConfig Data")]
    public class CrackedPlankConfigData : EditorTileConfigData<CrackedPlankConfig>
    {
        [field: SerializeField] public bool CanRespawn { get; private set; }

        public override CrackedPlankConfig CreateConfig()
        {
            return new CrackedPlankConfig(this.CanRespawn);
        }
    }
}
