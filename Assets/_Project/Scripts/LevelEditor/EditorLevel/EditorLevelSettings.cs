using Editarrr.Misc;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Level Settings", menuName = "Settings/Editor/new Editor Level Settings")]
    public class EditorLevelSettings : ScriptableObject
    {
        private const string Documentation = "This one probably doesnt need explaining.\r\n" +
                                             "Settings for the level editor...";

        [field: SerializeField, Info(Documentation)] public int EditorLevelScaleX { get; private set; } = 100;
        [field: SerializeField] public int EditorLevelScaleY { get; private set; } = 50;

        [field: SerializeField] public int AutoExpansionThreshold { get; private set; } = 4;
        [field: SerializeField] public int AutoExpansionValue { get; private set; } = 8;
    }
}
