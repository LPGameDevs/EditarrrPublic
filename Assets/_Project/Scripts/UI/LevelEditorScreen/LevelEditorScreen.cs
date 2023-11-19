using Editarrr.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen : UIElement
    {
        public static Action OnPointerEnter { get; set; }
        public static Action OnPointerLeave { get; set; }
        public static Action<bool> OnInputFocus { get; set; }

        [field: SerializeField, Header("Components")] public TileSelectionComponent TileSelection { get; private set; }
        [field: SerializeField] public TileGroupSwapperComponent TileGroupSwapper { get; private set; }
        [field: SerializeField] public SelectionPreviewComponent SelectionPreview { get; private set; }
        [field: SerializeField] public SaveAndPlayComponent SaveAndPlay { get; private set; }
        [field: SerializeField] public SelectionConfigComponent SelectionConfig { get; private set; }

        private void Awake()
        {
            LevelEditorScreen.OnPointerEnter = null;
            LevelEditorScreen.OnPointerLeave = null;
            LevelEditorScreen.OnInputFocus = null;
        }

        private void Start()
        {
            this.TileSelection.Initialize(this, this.Document.rootVisualElement);
            this.TileGroupSwapper.Initialize(this, this.Document.rootVisualElement);
            this.SelectionPreview.Initialize(this, this.Document.rootVisualElement);
            this.SaveAndPlay.Initialize(this, this.Document.rootVisualElement);
            this.SelectionConfig.Initialize(this, this.Document.rootVisualElement);
        }


        protected static void PointerEnter(PointerEnterEvent pointerEnterEvent)
        {
            LevelEditorScreen.OnPointerEnter?.Invoke();
            AudioManager.Instance.PlayAudioClip(AudioManager.MINOR_CLICK_CLIP_NAME);
        }

        protected static void PointerLeave(PointerLeaveEvent pointerLeaveEvent)
        {
            LevelEditorScreen.OnPointerLeave?.Invoke();
            AudioManager.Instance.PlayAudioClip(AudioManager.MINOR_CLICK_CLIP_NAME);
        }

        protected static void InputFocus(FocusEvent focusEvent)
        {
            // This might be the only HACK to properly proagate the
            // Unfocus Event to our Game Logic as the InputElement regains Focus when confirming with Enter...
            if (focusEvent.relatedTarget == null)
            {
                //Debug.Log("Focus");
                LevelEditorScreen.OnInputFocus?.Invoke(true);
            }
            else
                Debug.Log("Focus ignored");
        }

        protected static void InputBlur(BlurEvent blurEvent)
        {
            Debug.Log("Focus Lost");
            LevelEditorScreen.OnInputFocus?.Invoke(false);
        }
    }
}
