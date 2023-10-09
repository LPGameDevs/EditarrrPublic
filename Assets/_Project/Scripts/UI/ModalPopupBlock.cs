using Editarrr.UI;
using TMPro;
using UnityEngine;

public class ModalPopupBlock : MonoBehaviour
{
    public TMP_Text TitleText, ContentText, CloseText;

    private IModalPopup _modal;

    public void Setup(IModalPopup modal)
    {
        _modal = modal;
        TitleText.text = modal.GetTitleText();
        ContentText.text = modal.GetContentText();
        CloseText.text = modal.GetCloseText();
    }

    public void Close()
    {
        _modal.Close();
        Destroy(gameObject);
    }
}
