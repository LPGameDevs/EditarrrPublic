using Editarrr.UI;
using Singletons;
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

        public virtual void Open(Transform parent = null, bool alwaysShow = false)
        {
            if (!alwaysShow && PreferencesManager.Instance.IsModalEventTracked(this.name, ModalPopupAction.Close))
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

            PreferencesManager.Instance.SetModalEventTracked(this.name, ModalPopupAction.Open);
        }

        public virtual void Close()
        {
            PreferencesManager.Instance.SetModalEventTracked(this.name, ModalPopupAction.Close);
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
