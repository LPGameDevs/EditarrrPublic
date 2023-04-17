using Editarrr.Misc;
using Editarrr.Systems;

namespace Editarrr.LevelEditor
{
    public class EditorTileSelectionSystem : SystemComponent<EditorTileSelectionManager>
    {

        private const string Documentation =
            "This system is responsible for Editor Tile Selection.\r\n" +
            "It responds to Input to provide shortcuts for editor tile selection.\r\n";
        [Info(Documentation)]
        public bool _documentation;
    }
}
