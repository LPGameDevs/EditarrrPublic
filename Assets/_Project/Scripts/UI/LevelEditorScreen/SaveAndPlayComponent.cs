using System;
using Editarrr.LevelEditor;
using Singletons;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen
    {
        [System.Serializable]
        public class SaveAndPlayComponent : UIComponent
        {
            public static event Action OnInvalidLevelRequest;

            [field: SerializeField, Header("Manager")] private EditorLevelManager EditorLevelManager { get; set; }

            [field: SerializeField, Header("Names")] public string ContainerName { get; private set; } = "SaveAndPlay";
            [field: SerializeField] public string PlayButtonName { get; private set; } = "Play";
            [field: SerializeField] public string SaveButtonName { get; private set; } = "Save";
            [field: SerializeField] public string CloseButtonName { get; private set; } = "Save";


            private VisualElement ContainerElement { get; set; }
            private Button PlayButtonElement { get; set; }
            private Button SaveButtonElement { get; set; }
            private Button CloseButtonElement { get; set; }


            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.ContainerElement = visualElement.Q<VisualElement>(this.ContainerName);

                this.PlayButtonElement = this.ContainerElement.Q<Button>(this.PlayButtonName);
                this.PlayButtonElement.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.PlayButtonElement.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);

                this.SaveButtonElement = this.ContainerElement.Q<Button>(this.SaveButtonName);
                this.SaveButtonElement.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.SaveButtonElement.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);

                this.CloseButtonElement = this.ContainerElement.Q<Button>(this.CloseButtonName);
                this.CloseButtonElement.RegisterCallback<PointerEnterEvent>(LevelEditorScreen.PointerEnter);
                this.CloseButtonElement.RegisterCallback<PointerLeaveEvent>(LevelEditorScreen.PointerLeave);


                this.PlayButtonElement.clicked += this.PlayButtonElement_Clicked;
                this.SaveButtonElement.clicked += this.SaveButtonElement_Clicked;
                this.CloseButtonElement.clicked += this.CloseButtonElement_Clicked;
            }

            private void PlayButtonElement_Clicked()
            {
                this.EditorLevelManager.SaveLevelState(false);

                if (!this.EditorLevelManager.IsLevelValid())
                {
                    OnInvalidLevelRequest?.Invoke();
                    return;
                }
                
                this.EditorLevelManager.SaveLevelState();
                SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.TestLevelSceneName);
            }

            private void SaveButtonElement_Clicked()
            {
                bool upload = this.EditorLevelManager.IsLevelValid();
                
                this.EditorLevelManager.SaveLevelState(upload);
                SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
            }

            private void CloseButtonElement_Clicked()
            {
                SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
            }
        }
    }
}
