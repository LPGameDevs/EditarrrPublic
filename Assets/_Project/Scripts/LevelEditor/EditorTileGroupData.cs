using Editarrr.Misc;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Group Data", menuName = "Data/Editor/new Editor Tile Group Data")]
    public class EditorTileGroupData : ScriptableObject
    {
        private const string Documentation = "A tile group is used to configure a particular panel of the Level Editor." +
                                             "All the tiles referenced here will be visible together and be selectable.";

        [field: SerializeField, Info(Documentation)] public EditorTileData[] GroupElements { get; private set; }
    }
}
