using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Prefab Pool", menuName = "Pool/Editor/new Editor Prefab Pool")]
    public class EditorPrefabPool : ScriptableObject
    {
        [field: SerializeField, Header("Editor")] public SpriteRenderer EditorGrid { get; private set; }
        [field: SerializeField] public EditorHoverTile EditorHoverTile { get; private set; }
    }
}
