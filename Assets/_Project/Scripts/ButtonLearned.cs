using MoreMountains.Feedbacks;
using UnityEngine;

public class ButtonLearned : MonoBehaviour
{
    private MMFeedbacks _feedbacks;

    void Start()
    {
        _feedbacks = GetComponent<MMFeedbacks>();
        if (PlayerPrefs.GetInt("HelpButtonLearned", 0) == 1)
        {
            _feedbacks.StopFeedbacks();
            _feedbacks.enabled = false;
        }
    }

    public void ButtonPressed()
    {
        PlayerPrefs.SetInt("HelpButtonLearned", 1);
        _feedbacks.StopFeedbacks();
        _feedbacks.enabled = false;
    }
}
