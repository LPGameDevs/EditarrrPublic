using Editarrr.Audio;
using Gameplay.GUI;
using Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RatingMenu : MonoBehaviour
{
    [SerializeField] AudioClip[] _ratingSounds;
    [SerializeField] Button[] _ratingButtons;
    [SerializeField] Image[] _coinImages;
    [SerializeField] Button _submitButton;
    [SerializeField] TMPro.TMP_Text _currentRatingText;
    [SerializeField] Sprite _inactiveSprite;
    [SerializeField] WinMenu _winMenu;


    public int CurrentUserRating { get => _currentUserRating; }
  
    private float _baseTextSize;
    private int _currentUserRating = 0, _maxRating = 3;
    private const string PARAMETER_NAME_ACTIVE = "active";
    private const string PARAMETER_NAME_PRESSED = "Pressed";

    private void Awake()
    {
        _baseTextSize = _currentRatingText.fontSize;
    }

    private void OnEnable()
    {
        _submitButton.interactable = false;
    }

    public void OpenMenu(string levelCode)
    {
        gameObject.SetActive(true);
        UpdateRating(PreferencesManager.Instance.GetLevelRating(levelCode));
    }

    public void UpdateRating(int rating)
    {
        rating = Mathf.Clamp(rating, 0, _maxRating);

        for(int i=0; i<_ratingButtons.Length; i++)
        {
            //_ratingButtons[i].OnDeselect(new UnityEngine.EventSystems.BaseEventData(FindObjectOfType<EventSystem>()));
            if(rating-1 != i)
                _ratingButtons[i].animator.SetTrigger(PARAMETER_NAME_PRESSED);

            if (rating-1 >= i)
                _ratingButtons[i].animator.SetBool(PARAMETER_NAME_ACTIVE, true);
            else
            {
                _ratingButtons[i].animator.SetBool(PARAMETER_NAME_ACTIVE, false);
                _coinImages[i].sprite = _inactiveSprite;
            }
        }

        if (rating > 0)
        {
            AudioManager.Instance.PlayAudioClip(_ratingSounds[rating - 1]);

            if (_currentUserRating == 0)
            {
                _submitButton.interactable = true;
                _currentRatingText.color = new Color(_currentRatingText.color.r, _currentRatingText.color.g, _currentRatingText.color.b, 1f);
            }
        }

        _currentUserRating = rating;
        _currentRatingText.fontSize = _baseTextSize + ((_currentUserRating + 1) * 10f);
        _currentRatingText.text = $"{_currentUserRating} / {_ratingButtons.Length}";
    }

    public void SubmitRating()
    {
        AudioManager.Instance.PlayAudioClip(AudioManager.KEY_PICKUP_CLIP_NAME);
        _winMenu.SubmitRating(_currentUserRating);
        gameObject.SetActive(false);
    }
}
