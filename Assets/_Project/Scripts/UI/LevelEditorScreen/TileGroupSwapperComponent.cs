using Editarrr.LevelEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen
    {
        [System.Serializable]
        public class TileGroupSwapperComponent : UIComponent
        {
            [field: SerializeField, Header("Data")] private EditorTileSelectionManager EditorTileSelectionManager { get; set; }

            [field: SerializeField, Header("Names")] public string TileGroupSwapperName { get; private set; } = "TileGroupSwapper";

            private Button TileGroupSwapperElement { get; set; }

            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.TileGroupSwapperElement = visualElement.Q<Button>(this.TileGroupSwapperName);
                this.TileGroupSwapperElement.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.TileGroupSwapperElement.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);

                this.TileGroupSwapperElement.clicked += this.TileGroupSwapperElement_Clicked;

                EditorTileSelectionManager.ActiveGroupChanged += this.SetActiveGroup;
                this.SetActiveGroup(this.EditorTileSelectionManager.ActiveGroup);
            }

            private void SetActiveGroup(EditorTileGroupData editorTileGroup)
            {
                this.TileGroupSwapperElement.text = $"{this.EditorTileSelectionManager.ActiveGroupIndex + 1}";
            }

            private void TileGroupSwapperElement_Clicked()
            {
                this.EditorTileSelectionManager.NextGroup();
            }
        }
    }
}
