using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Group Data", menuName = "Data/Editor/new Editor Tile Group Data")]
    public class EditorTileGroupData : ScriptableObject
    {
        [field: SerializeField] public EditorTileData[] GroupElements { get; private set; }
    }
}
