using UnityEngine;

namespace Editarrr.UI
{
    [CreateAssetMenu(fileName = "NewModalPopup", menuName = "Modals/new Modal Popup")]
    public class ModalPopup : ScriptableObject, IModalPopup
    {
        [SerializeField] private string TitleText;
        [SerializeField] private string ContentText;
        [SerializeField] private string CloseText;
        [SerializeField] private ModalPopupBlock Prefab;

        public void Open(Transform parent = null)
        {
            if (parent == null)
            {
                parent = FindObjectOfType<Canvas>().transform;
            }
            var popup = Instantiate(Prefab, parent);
            popup.Setup(this);
        }

        public void Close()
        {
            // @todo Store dismissal to not show again later.
        }

        public string GetTitleText()
        {
            return TitleText;
        }

        public string GetContentText()
        {
            return ContentText;
        }

        public string GetCloseText()
        {
            return CloseText;
        }
    }
}
