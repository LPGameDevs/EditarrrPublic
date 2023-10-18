using Editarrr.LevelEditor;
using Editarrr.Misc;
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

            private VisualElement ContainerElement { get; set; }

            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.ContainerElement = visualElement.Q<VisualElement>(this.ContainerName);

                EditorLevelManager.OnEditorConfigSelected += this.EditorLevelManager_OnEditorConfigSelected;
            }

            private void EditorLevelManager_OnEditorConfigSelected(TileConfig tileConfig)
            {
                this.ContainerElement.Clear();

                tileConfig.CreateGUIElements(this.Create);
            }

            private VisualElement Create<T>(T t)
            {
                Debug.Log(t);

                if (t is bool value)
                {
                    Debug.Log(" as bool");
                    TemplateContainer template = this.BoolValueTemplate.Instantiate();
                    var toggle = template.Q<Toggle>(this.ValueName);

                    return toggle;
                }

                return null;
            }


            private class BoolValueElement
            {

            }
        }
    }
}
