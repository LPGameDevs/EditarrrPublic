using Editarrr.LevelEditor;
using Editarrr.Misc;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen
    {
        [System.Serializable]
        public class SelectionPreviewComponent : UIComponent
        {
            [field: SerializeField, Header("Data")] private EditorTileSelectionManager EditorTileGroupManager { get; set; }

            [field: SerializeField, Header("Names")] public string ContainerName { get; private set; } = "SelectionPreview";
            [field: SerializeField] public string RotateButtonName { get; private set; } = "Rotate";
            [field: SerializeField] public string TilePreviewName { get; private set; } = "TilePreview";


            private VisualElement ContainerElement { get; set; }
            private Button RotateButtonElement { get; set; }
            private VisualElement TilePreviewElement { get; set; }

            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.ContainerElement = visualElement.Q<VisualElement>(this.ContainerName);

                this.RotateButtonElement = this.ContainerElement.Q<Button>(this.RotateButtonName);
                this.RotateButtonElement.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.RotateButtonElement.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);
                this.TilePreviewElement = this.ContainerElement.Q<VisualElement>(this.TilePreviewName);

                this.RotateButtonElement.clicked += this.RotateButtonElement_Clicked;

                EditorTileSelectionManager.ActiveElementChanged += this.SetActiveElement;
                this.UpdatePreview();
            }

            private void RotateButtonElement_Clicked()
            {
                this.EditorTileGroupManager.NextRotation();
                this.UpdatePreview();
            }

            private void UpdatePreview()
            {
                this.SetActiveElement(this.EditorTileGroupManager.ActiveElement);
            }

            private void SetActiveElement(EditorTileData editorTileData)
            {
                Sprite sprite = null;
                float rotate = 0;

                if (editorTileData != null)
                {
                    sprite = editorTileData.UISprite;

                    if (editorTileData.Tile.CanRotate)
                        rotate = this.EditorTileGroupManager.Rotation.ToDegree();
                }



                this.TilePreviewElement.style.backgroundImage = new StyleBackground(sprite);
                this.TilePreviewElement.style.rotate = new StyleRotate(new Rotate(-rotate));
            }
        }
    }
}
