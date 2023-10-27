using Editarrr.UI;
using TMPro;
using UI;
using UnityEngine;

public class AchievementPopupBlock : MonoBehaviour
{
    public TMP_Text TitleText;

    private IModalPopup _modal;

    public void Setup(IModalPopup modal)
    {
        _modal = modal;
        TitleText.text = modal.GetTitleText();
    }

    public void Close()
    {
        _modal.Close();
        Destroy(gameObject);
    }
}
