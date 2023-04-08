using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Group Data Pool", menuName = "Pool/Editor/new Editor Tile Group Data Pool")]
    public class EditorTileGroupDataPool : ScriptableObject
    {
        [field: SerializeField] public EditorTileGroupData[] GroupData{ get; private set; }

        public EditorTileGroupData Get(int index)
        {
            return this.GroupData[index];
        }
    }
}
