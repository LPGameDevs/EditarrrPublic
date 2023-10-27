using Editarrr.UI;
using TMPro;
using UI;
using UnityEngine;

public class AchievementPopupBlock : MonoBehaviour
{
    public TMP_Text TitleText;

    private PopupAchievement _achievement;

    public void Setup(PopupAchievement achievement)
    {
        _achievement = achievement;
        TitleText.text = achievement.GetTitleText();

        Invoke(nameof(Close), 2f);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
