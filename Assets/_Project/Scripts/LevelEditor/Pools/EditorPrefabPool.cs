using Editarrr.Misc;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Prefab Pool", menuName = "Pool/Editor/new Editor Prefab Pool")]
    public class EditorPrefabPool : ScriptableObject
    {
        private const string Documentation =
            "This pool contains all prefabs required by the EditorLevelManager.";

        [field: SerializeField, Header("Editor"), Info(Documentation)] public SpriteRenderer EditorGrid { get; private set; }
        [field: SerializeField] public EditorHoverTile EditorHoverTile { get; private set; }
    }
}
