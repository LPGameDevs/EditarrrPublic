using UnityEngine;

namespace Editarrr.UI
{
    public interface IModalPopup
    {
        public void Open(Transform parent = null);
        public void Close();

        public string GetTitleText();
        public string GetContentText();
        public string GetCloseText();
    }
}
