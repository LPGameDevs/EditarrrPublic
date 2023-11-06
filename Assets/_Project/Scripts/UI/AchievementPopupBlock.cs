using TMPro;
using UI;
using UnityEngine;

public class AchievementPopupBlock : MonoBehaviour
{
    public TMP_Text TitleText;
    public float DestroyTime = 2f;

    private PopupAchievement _achievement;

    private float _spawnTime = 0;

    public void Setup(PopupAchievement achievement)
    {
        _achievement = achievement;
        TitleText.text = achievement.GetTitleText();

        _spawnTime = Time.unscaledTime;
    }

    private void Update()
    {
        if (_spawnTime == 0)
        {
            return;
        }

        if (Time.unscaledTime > _spawnTime + DestroyTime)
        {
            Close();
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
