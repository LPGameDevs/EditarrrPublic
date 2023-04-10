using Editarrr.LevelEditor;
using UnityEngine;

namespace Editarrr.Game
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Data/Game/new Game Data", order = 0)]
    public class GameData : ScriptableObject
    {
        [field: SerializeField, Header("Manager")] public EditorTileSelectionManager EditorTileGroupManager { get; private set; }


        [field: SerializeField, Header("Pools")] public EditorPrefabPool EditorPrefabs { get; private set; }
    }
}
