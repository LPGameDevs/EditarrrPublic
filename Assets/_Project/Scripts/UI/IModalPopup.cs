using UnityEngine;

namespace Editarrr.UI
{
    public interface IModalPopup
    {
        public void Open(Transform parent = null, bool alwaysShow = false);
        public void Close();

        public string GetTitleText();
        public string GetContentText();
        public string GetCloseText();
    }
}
