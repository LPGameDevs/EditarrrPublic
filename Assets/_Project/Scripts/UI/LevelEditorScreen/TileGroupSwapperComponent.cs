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

            [field: SerializeField, Header("Names")] public string TileGroupSwapperNameLeft { get; private set; } = "TileGroupSwapperLeft";
            [field: SerializeField] public string TileGroupSwapperNameRight { get; private set; } = "TileGroupSwapperRight";

            private Button TileGroupSwapperElementLeft { get; set; }
            private Button TileGroupSwapperElementRight { get; set; }

            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.TileGroupSwapperElementLeft = visualElement.Q<Button>(this.TileGroupSwapperNameLeft);
                this.TileGroupSwapperElementLeft.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.TileGroupSwapperElementLeft.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);

                this.TileGroupSwapperElementRight = visualElement.Q<Button>(this.TileGroupSwapperNameRight);
                this.TileGroupSwapperElementRight.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.TileGroupSwapperElementRight.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);

                this.TileGroupSwapperElementLeft.clicked += () => this.TileGroupSwapperElement_Clicked(true);
                this.TileGroupSwapperElementRight.clicked += () => this.TileGroupSwapperElement_Clicked(false);
            }

            private void TileGroupSwapperElement_Clicked(bool isLeftElement)
            {
                if (isLeftElement)
                    this.EditorTileSelectionManager.PreviousGroup();
                else
                    this.EditorTileSelectionManager.NextGroup();
            }
        }
    }
}
