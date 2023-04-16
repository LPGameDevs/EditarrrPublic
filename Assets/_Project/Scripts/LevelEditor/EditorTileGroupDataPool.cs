using Editarrr.Misc;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Group Data Pool", menuName = "Pool/Editor/new Editor Tile Group Data Pool")]
    public class EditorTileGroupDataPool : ScriptableObject
    {
        private const string Documentation = "A collection of group panels that can be navigated in the level editor.\r\n" +
                                             "Every group can contain different sets of tiles that can be placed.";
        [field: SerializeField, Info(Documentation)] public EditorTileGroupData[] GroupData{ get; private set; }

        public EditorTileGroupData Get(int index)
        {
            return this.GroupData[index];
        }
    }
}
