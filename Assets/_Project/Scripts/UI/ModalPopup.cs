using Editarrr.UI;
using UnityEngine;

namespace UI
{
    public enum ModalPopupAction
    {
        Open = 0,
        Close = 1,
        Confirm = 2,
    }

    [CreateAssetMenu(fileName = "NewModalPopup", menuName = "Modals/new Modal Popup")]
    public class ModalPopup : ScriptableObject, IModalPopup
    {
        [SerializeField] protected string TitleText;
        [SerializeField] protected string ContentText;
        [SerializeField] protected string CloseText;
        [SerializeField] protected ModalPopupBlock Prefab;

        protected virtual void TrackEvent(string name, ModalPopupAction action)
        {
            PlayerPrefs.SetInt($"ModalPopupTrack-{name}-{action.ToString()}", 1);
        }

        protected virtual bool HasTrackedEvent(string name, ModalPopupAction action)
        {
            int hasTracked = PlayerPrefs.GetInt($"ModalPopupTrack-{name}-{action.ToString()}", 0);
            return hasTracked == 1;
        }

        public virtual void Open(Transform parent = null)
        {
            if (HasTrackedEvent(this.name, ModalPopupAction.Close))
            {
                this.Close();
                return;
            }

            if (parent == null)
            {
                parent = FindObjectOfType<Canvas>().transform;
            }
            var popup = Instantiate(Prefab, parent);
            popup.Setup(this);

            this.TrackEvent(this.name, ModalPopupAction.Open);
        }

        public virtual void Close()
        {
           this.TrackEvent(this.name, ModalPopupAction.Close);
        }

        public virtual string GetTitleText()
        {
            return TitleText;
        }

        public virtual string GetContentText()
        {
            return ContentText;
        }

        public virtual string GetCloseText()
        {
            return CloseText;
        }
    }
}
