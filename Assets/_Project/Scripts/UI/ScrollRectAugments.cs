using Editarrr.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAugments : MonoBehaviour
{
    [SerializeField] Vector2 _scrollTickDistance;

    ScrollRect _scrollRect;
    Vector2 _checkpointPos, _checkpointNeg;
    bool _checkpointPassed;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();

        _checkpointPos = _scrollTickDistance * 0.5f;
        _checkpointNeg = _scrollTickDistance * -0.5f;
    }

    public void OnValueChanged(Vector2 newPercentilePosition)
    {
        if (_scrollTickDistance.x != 0)
        {
            while (_scrollRect.content.anchoredPosition.x > _checkpointPos.x)
                UpdateCheckpoints(true);
            while (_scrollRect.content.anchoredPosition.x < _checkpointNeg.x)
                UpdateCheckpoints(false);
        }

        if (_scrollTickDistance.y != 0)
        {
            while (_scrollRect.content.anchoredPosition.y > _checkpointPos.y)
                UpdateCheckpoints(true);
            while (_scrollRect.content.anchoredPosition.y < _checkpointNeg.y)
                UpdateCheckpoints(false);
        }

        if (_checkpointPassed)
            PlayScrollTickEffects();
    }

    private void UpdateCheckpoints(bool increase)
    {
        if(increase)
        {
            _checkpointNeg = _checkpointPos;
            _checkpointPos += _scrollTickDistance;
        }
        else
        {
            _checkpointPos = _checkpointNeg;
            _checkpointNeg -= _scrollTickDistance;
        }

        if (!_checkpointPassed) _checkpointPassed = true;
    }

    private void PlayScrollTickEffects()
    {
        _checkpointPassed = false;
        AudioManager.Instance.PlayRandomizedAudioClip(AudioManager.SCROLL_TICK_CLIP_NAME, 0.1f, 0.05f);
    }
}
