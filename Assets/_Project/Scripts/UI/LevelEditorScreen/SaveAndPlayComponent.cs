using Editarrr.LevelEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen
    {
        [System.Serializable]
        public class SaveAndPlayComponent : UIComponent
        {
            [field: SerializeField, Header("Names")] public string ContainerName { get; private set; } = "SaveAndPlay";
            [field: SerializeField] public string PlayButtonName { get; private set; } = "Play";
            [field: SerializeField] public string SaveButtonName { get; private set; } = "Save";


            private VisualElement ContainerElement { get; set; }
            private Button PlayButtonElement { get; set; }
            private Button SaveButtonElement { get; set; }


            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.ContainerElement = visualElement.Q<VisualElement>(this.ContainerName);

                this.PlayButtonElement = this.ContainerElement.Q<Button>(this.PlayButtonName);
                this.PlayButtonElement.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.PlayButtonElement.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);

                this.SaveButtonElement = this.ContainerElement.Q<Button>(this.SaveButtonName);
                this.SaveButtonElement.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.SaveButtonElement.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);

                this.PlayButtonElement.clicked += this.PlayButtonElement_Clicked;
                this.SaveButtonElement.clicked += this.SaveButtonElement_Clicked;
            }

            private void PlayButtonElement_Clicked()
            {
#warning // TODO Add Code to test Level here
            }

            private void SaveButtonElement_Clicked()
            {
#warning // TODO Add Code to Save Level here
            }
        }
    }
}
