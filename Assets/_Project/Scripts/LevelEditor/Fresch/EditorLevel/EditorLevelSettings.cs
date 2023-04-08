using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Level Settings", menuName = "Settings/Editor/new Editor Level Settings")]
    public class EditorLevelSettings : ScriptableObject
    {
        [field: SerializeField] public int EditorLevelScaleX { get; private set; } = 100;
        [field: SerializeField] public int EditorLevelScaleY { get; private set; } = 50;
    }
}
