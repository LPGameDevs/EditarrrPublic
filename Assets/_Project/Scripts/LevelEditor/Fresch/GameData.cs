using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Data/Game/new Game Data", order = 0)]
    public class GameData : ScriptableObject
    {
        [field: SerializeField, Header("Manager")] public EditorTileSelectionManager EditorTileGroupManager { get; private set; }

    }
}
