using Editarrr.LevelEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen
    {
        [System.Serializable]
        public class TileSelectionComponent : UIComponent
        {
            [field: SerializeField, Header("Data")] private EditorTileSelectionManager EditorTileGroupManager { get; set; }

            [field: SerializeField, Header("Templates")] private VisualTreeAsset TileDataSlotTemplate { get; set; }

            [field: SerializeField, Header("Names")] public string TileDataSlotContainerName { get; private set; } = "TileSelectionContainer";
            [field: SerializeField] public string TileDataSlotName { get; private set; } = "TileDataSlot";
            [field: SerializeField] public string TileDataSlotImageName { get; private set; } = "Image";


            [field: SerializeField] private int TileDataSlotCount { get; set; } = 10;
            private VisualElement TileDataSlotContainerElement { get; set; }
            private TileButton[] TileDataSlotElements { get; set; }

            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.TileDataSlotContainerElement = visualElement.Q<VisualElement>(this.TileDataSlotContainerName);

                this.TileDataSlotElements = new TileButton[this.TileDataSlotCount];

                for (int i = 0; i < this.TileDataSlotCount; i++)
                {
                    string name = $"{this.TileDataSlotName}";

                    // Create Visuals and add to Container
                    TemplateContainer template = this.TileDataSlotTemplate.Instantiate();
                    this.TileDataSlotContainerElement.Add(template);

                    Button button = template.Q<Button>(name);
                    button.userData = i;
                    button.clickable.clickedWithEventInfo += this.TileDataSlotElements_Clicked;

                    VisualElement image = button.Q<VisualElement>(this.TileDataSlotImageName);

                    this.TileDataSlotElements[i] = new TileButton(button, image);
                }

                EditorTileSelectionManager.ActiveGroupChanged += this.SetActiveGroup;
                EditorTileSelectionManager.ActiveElementChanged += this.SetActiveElement;
                this.SetActiveGroup(this.EditorTileGroupManager.ActiveGroup);
                this.SetActiveElement(this.EditorTileGroupManager.ActiveElement);
            }

            private void SetActiveGroup(EditorTileGroupData editorTileGroup)
            {
                EditorTileData[] editorTileData = null;

                if (editorTileGroup != null)
                {
                    editorTileData = editorTileGroup.GroupElements;
                }

                for (int i = 0; i < this.TileDataSlotCount; i++)
                {
                    Sprite sprite = null;

                    if (editorTileData != null && editorTileData.Length > i)
                    {
                        EditorTileData tileData = editorTileData[i];

                        if (tileData != null)
                            sprite = tileData.UISprite;
                    }

                    this.TileDataSlotElements[i].Image.style.backgroundImage = new StyleBackground(sprite);
                }
            }

            private void SetActiveElement(EditorTileData editorTileData)
            {
                // Highlights etc??
            }

            private void TileDataSlotElements_Clicked(EventBase eventBase)
            {
                if (!(eventBase.target is Button button))
                    return;

                if (!(button.userData is int index))
                    return;

                this.EditorTileGroupManager.SetActiveElementIndex(index);
            }

            private class TileButton
            {
                public Button Button { get; private set; }
                public VisualElement Image { get; private set; }

                public TileButton(Button button, VisualElement image)
                {
                    this.Button = button;
                    this.Image = image;
                }
            }
        }
    }
}
