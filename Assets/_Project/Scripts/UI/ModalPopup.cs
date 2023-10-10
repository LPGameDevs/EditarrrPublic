using Editarrr.UI;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "NewModalPopup", menuName = "Modals/new Modal Popup")]
    public class ModalPopup : ScriptableObject, IModalPopup
    {
        [SerializeField] protected string TitleText;
        [SerializeField] protected string ContentText;
        [SerializeField] protected string CloseText;
        [SerializeField] protected ModalPopupBlock Prefab;

        public virtual void Open(Transform parent = null)
        {
            if (parent == null)
            {
                parent = FindObjectOfType<Canvas>().transform;
            }
            var popup = Instantiate(Prefab, parent);
            popup.Setup(this);
        }

        public virtual void Close()
        {
            // @todo Store dismissal to not show again later.
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
