using Editarrr.LevelEditor;
using Mono.Cecil.Cil;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen
    {
        [System.Serializable]
        public class TileSelectionComponent : UIComponent
        {
            [field: SerializeField, Header("Data")] private EditorTileSelectionManager EditorTileSelectionManager { get; set; }
            [field: SerializeField] private EditorLevelManager EditorLevelManager { get; set; }

            [field: SerializeField, Header("Templates")] private VisualTreeAsset TileDataSlotTemplate { get; set; }
            [field: SerializeField] private VisualTreeAsset TiletipAsset { get; set; }

            [field: SerializeField, Header("Names")] public string TileDataSlotContainerName { get; private set; } = "TileSelectionContainer";
            [field: SerializeField] public string TileDataSlotName { get; private set; } = "TileDataSlot";
            [field: SerializeField] public string TileDataSlotImageName { get; private set; } = "Image";
            [field: SerializeField] public string TileDataSlotCountContainerName { get; private set; } = "Count";
            [field: SerializeField] public string TileDataSlotCountContainerLabelName { get; private set; } = "Value";


            [field: SerializeField] private int TileDataSlotCount { get; set; } = 10;
            private VisualElement TileDataSlotContainerElement { get; set; }
            private TileButton[] TileDataSlotElements { get; set; }

            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.TileDataSlotContainerElement = visualElement.Q<VisualElement>(this.TileDataSlotContainerName);
                var tiletipContainer = this.TiletipAsset.Instantiate();
                tiletipContainer.visible = false;

                this.TileDataSlotElements = new TileButton[this.TileDataSlotCount];

                for (int i = 0; i < this.TileDataSlotCount; i++)
                {
                    string name = $"{this.TileDataSlotName}";

                    // Create Visuals and add to Container
                    TemplateContainer template = this.TileDataSlotTemplate.Instantiate();
                    template.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                    template.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);
                    this.TileDataSlotContainerElement.Add(template);

                    Label toolTip = template.Q<Label>("ToolTip");

                    Button button = template.Q<Button>(name);
                    button.userData = i;
                    button.clickable.clickedWithEventInfo += this.TileDataSlotElements_Clicked;

                    VisualElement image = button.Q<VisualElement>(this.TileDataSlotImageName);

                    VisualElement countContainer = template.Q<VisualElement>(this.TileDataSlotCountContainerName);
                    Label countContainerValue = countContainer.Q<Label>(this.TileDataSlotCountContainerLabelName);

                    TileButton tileButton = this.TileDataSlotElements[i] = new TileButton(button, image, countContainer, countContainerValue, toolTip, tiletipContainer);

                    template.RegisterCallback<PointerEnterEvent>(tileButton.ShowToolTip);
                    template.RegisterCallback<PointerLeaveEvent>(tileButton.HideToolTip);
                }

                EditorTileSelectionManager.ActiveGroupChanged += this.EditorTileSelectionManager_ActiveGroupChanged;
                EditorTileSelectionManager.ActiveElementChanged += this.EditorTileSelectionManager_ActiveElementChanged;
                
                EditorLevelManager.OnTileSet += this.EditorLevelManager_OnTileSet;
                EditorLevelManager.OnTileUnset += this.EditorLevelManager_OnTileUnset;

                this.SetActiveGroup(this.EditorTileSelectionManager.ActiveGroup);
                this.SetActiveElement(this.EditorTileSelectionManager.ActiveElement);
            }

            private void EditorTileSelectionManager_ActiveGroupChanged(EditorTileGroupData editorTileGroup)
            {
                this.SetActiveGroup(editorTileGroup);
            }
            
            private void EditorTileSelectionManager_ActiveElementChanged(EditorTileData editorTileData)
            {
                this.SetActiveElement(editorTileData);
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
                    if (editorTileData != null && editorTileData.Length > i)
                    {
                        EditorTileData tileData = editorTileData[i];

                        this.TileDataSlotElements[i].SetData(tileData);
                        if (tileData != null)
                        {
                            int count = this.EditorLevelManager.GetTileCount(tileData);
                            this.TileDataSlotElements[i].UpdateCount(tileData.LevelLimit - count);
                        }
                    }
                    else
                        this.TileDataSlotElements[i].SetData(null);
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

                this.EditorTileSelectionManager.SetActiveElementIndex(index);
            }

            private void EditorLevelManager_OnTileSet(EditorTileData editorTileData, TileType tileType, int inLevel)
            {
                this.UpdateButtonCount(editorTileData, inLevel);
            }

            private void EditorLevelManager_OnTileUnset(EditorTileData editorTileData, TileType tileType, int inLevel)
            {
                this.UpdateButtonCount(editorTileData, inLevel);
            }

            private void UpdateButtonCount(EditorTileData editorTileData, int inLevel)
            {
                for (int i = 0; i < this.TileDataSlotCount; i++)
                {
                    TileButton button = this.TileDataSlotElements[i];

                    if (button.EditorTileData != editorTileData)
                        continue;

                    button.UpdateCount(editorTileData.LevelLimit - inLevel);
                }
            }

            public class TileButton
            {
                public Button Button { get; private set; }
                public VisualElement Image { get; private set; }
                public VisualElement CountContainer { get; private set; }
                public Label Count { get; private set; }
                public Label ToolTip { get; private set; }

                public EditorTileData EditorTileData { get; private set; }
                public TemplateContainer TiletipContainer { get; private set; }
                public Vector2 CurrentPosition { get; private set; }

                public static event Action<EditorTileData> OnTileButtonHover;
                public static event Action OnTileButtonExit;

                public TileButton(Button button, VisualElement image, VisualElement countContainer, Label count, Label toolTip, TemplateContainer tiletipContainer)
                {
                    this.Button = button;
                    this.Image = image;
                    this.CountContainer = countContainer;
                    this.Count = count;
                    this.ToolTip = toolTip;
                    this.TiletipContainer = tiletipContainer;

                    this.SetToolTip(false);
                }

                public void SetData(EditorTileData editorTileData)
                {
                    this.EditorTileData = editorTileData;

                    Sprite sprite = null;
                    bool countContainer = false;
                    string toolTipContent = "";

                    if (editorTileData != null)
                    {
                        sprite = editorTileData.UISprite;
                        countContainer = editorTileData.LevelLimit > 0;
                        toolTipContent = editorTileData.Description;
                    }

                    this.Image.style.backgroundImage = new StyleBackground(sprite);
                    this.CountContainer.style.visibility = new StyleEnum<Visibility>(countContainer ? Visibility.Visible : Visibility.Hidden);
                    this.ToolTip.text = toolTipContent;
                }

                public void UpdateCount(int count)
                {
                    this.Count.text = $"{count}";
                }

                internal void ShowToolTip(PointerEnterEvent evt)
                {
                    OnTileButtonHover?.Invoke(this.EditorTileData);
                }

                internal void HideToolTip(PointerLeaveEvent evt)
                {
                    OnTileButtonExit?.Invoke();
                }

                private void SetToolTip(bool value)
                {
                    if (string.IsNullOrEmpty(this.ToolTip.text))
                    {
                        this.ToolTip.visible = false;
                        return;
                    }
                    this.ToolTip.visible = value;
                }
            }
        }
    }
}
