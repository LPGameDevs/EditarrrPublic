using UnityEngine;

namespace Editarrr.LevelEditor
{
    /// <summary>
    /// Object/Asset for asynchronous communication to the EditorLevelManager
    /// </summary>
    [CreateAssetMenu(fileName = "Editor Level Exchange", menuName = "Exchange/Editor/new Editor Level Exchange")]
    public class EditorLevelExchange : ScriptableObject
    {
        [field: SerializeField, Header("Level Loading")] public bool LoadOnStart { get; private set; }
        [field: SerializeField] public string CodeToLoad { get; private set; }
    }
}
