using Editarrr.LevelEditor;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen
    {
        [System.Serializable]
        public class SelectionConfigComponent : UIComponent
        {
            [field: SerializeField, Header("Names")] public string ContainerName { get; private set; } = "SelectionConfig";
            [field: SerializeField] public string ValueName { get; private set; } = "Value";
            [field: SerializeField] public string TilePreviewName { get; private set; } = "TilePreview";

            [field: SerializeField, Header("Templates")] private VisualTreeAsset BoolValueTemplate { get; set; }
            [field: SerializeField] private VisualTreeAsset FloatValueTemplate { get; set; }
            [field: SerializeField] private VisualTreeAsset IntValueTemplate { get; set; }

            private VisualElement ContainerElement { get; set; }

            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.ContainerElement = visualElement.Q<VisualElement>(this.ContainerName);

                EditorLevelManager.OnEditorConfigSelected += this.EditorLevelManager_OnEditorConfigSelected;
            }

            private void EditorLevelManager_OnEditorConfigSelected(TileConfig tileConfig, Vector2 tilePosition)
            {
                this.ContainerElement.Clear();

                if (tileConfig == null)
                {
                    this.SetDisplayContainer(false);
                    return;
                }

                this.SetDisplayContainer(true);

                var overlayEnabledConfig = tileConfig as TileConfigOverlayEnabled;
                
                if (overlayEnabledConfig != null)
                    tileConfig.CreateGUIElements(this.Create, tilePosition);
                else
                    tileConfig.CreateGUIElements(this.Create);
            }

            private VisualElement Create<T>(string title, T t)
            {
                TemplateContainer template = null;
                VisualElement toReturn = null;

                if (t is bool boolValue)
                {
                    //Debug.Log(" as bool");
                    template = this.BoolValueTemplate.Instantiate();
                    var element = template.Q<Toggle>(this.ValueName);
                    element.value = boolValue;
                    this.ContainerElement.Add(template);

                    toReturn = element;
                }
                else if (t is int intValue)
                {
                    //Debug.Log(" as int");
                    template = this.IntValueTemplate.Instantiate();
                    var inputElement = template.Q<TextField>(this.ValueName);
                    inputElement.RegisterCallback<FocusEvent>(LevelEditorScreen.InputFocus);
                    inputElement.RegisterCallback<BlurEvent>(LevelEditorScreen.InputBlur);
                    inputElement.value = $"{intValue}";
                    this.ContainerElement.Add(template);

                    toReturn = inputElement;
                }
                else if (t is float floatValue)
                {
                    Debug.LogError("TODO");
                    //TemplateContainer template = this.BoolValueTemplate.Instantiate();
                    //var toggle = template.Q<Toggle>(this.ValueName);
                    //toggle.value = floatValue;
                    //this.ContainerElement.Add(template);

                    //template.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                    //template.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);

                    return null;
                }

                if (template != null)
                {
                    Label label = template.Q<Label>("Title");
                    label.text = title;

                    // Register Mouse Enter/Leave Events
                    template.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                    template.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);
                }

                return toReturn;
            }

            private void SetDisplayContainer(bool value)
            {
                this.ContainerElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}
