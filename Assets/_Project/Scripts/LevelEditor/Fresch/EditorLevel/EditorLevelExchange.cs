using Editarrr.Misc;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    /// <summary>
    /// Object/Asset for asynchronous communication to the EditorLevelManager
    /// </summary>
    [CreateAssetMenu(fileName = "Editor Level Exchange", menuName = "Exchange/Editor/new Editor Level Exchange")]
    public class EditorLevelExchange : ScriptableObject
    {
        private const string Documentation = "Settings for how level editor and gameplay should interact with each other.";

        [field: SerializeField, Header("Level Loading"), Info(Documentation)] public bool LoadOnStart { get; private set; }
        [field: SerializeField] public string CodeToLoad { get; private set; }

        public void SetCode(string code)
        {
            CodeToLoad = code;
        }

        public void SetAutoload(bool loadOnStart)
        {
            LoadOnStart = loadOnStart;
        }
    }
}
