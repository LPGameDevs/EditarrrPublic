using Editarrr.UI;
using TMPro;
using UI;
using UnityEngine;

public class ModalPopupBlock : MonoBehaviour
{
    public TMP_Text TitleText, ContentText, CloseText, ConfirmText;

    private IModalPopup _modal;

    public void Setup(IModalPopup modal)
    {
        _modal = modal;
        TitleText.text = modal.GetTitleText();
        ContentText.text = modal.GetContentText();
        CloseText.text = modal.GetCloseText();

        if (_modal is ModalPopupConfirmation confirmation)
        {
            ConfirmText.text = confirmation.GetConfirmText();
        }
    }

    public void Close()
    {
        _modal.Close();
        Destroy(gameObject);
    }

    public void Confirm()
    {
        if (_modal is ModalPopupConfirmation confirmation)
        {
            confirmation.Confirm();
        }
        Close();
    }
}
