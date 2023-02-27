using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace Legacy
{
    /**
 * Reference material.
 * @see https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
 */

    public class Timer : MonoBehaviour
    {
        public static event Action OnTimeOut;
        public static event Action<string> OnTimeStop;

        [SerializeField]
        private MMFeedbacks _hideFeedbacks;
        [SerializeField]
        private MMFeedbacks _showFeedbacks;

        public bool IsCountdown = true;

        public float numberOfSeconds = 15;
        private float _secondsRemaining = 0;

        private float _currentTime;

        private TextMeshProUGUI _timeText;
        private MMFeedbacks _timeFeedbacks;
        private bool _hasStarted;
        private bool _isFinished;
        private bool _isFlashing;

        private void Start()
        {
            _timeText = GetComponent<TextMeshProUGUI>();
            _timeFeedbacks = GetComponent<MMFeedbacks>();
        }

        void Update()
        {
            if (_isFinished || !_hasStarted)
            {
                return;
            }

            if (IsCountdown)
            {
                HandleCountdown();
            }
            else
            {
                HandleTimer();
            }

        }

        private void HandleTimer()
        {
            _currentTime += Time.deltaTime;
            DisplayTime(_currentTime);
        }

        private void HandleCountdown()
        {
            if (_secondsRemaining > 0)
            {
                _secondsRemaining -= Time.deltaTime;
            }
            else
            {
                _secondsRemaining = 0;
                _isFinished = true;
                OnTimeOut?.Invoke();
            }
            DisplayTime(_secondsRemaining);

            if (_secondsRemaining <= 6)
            {
                float offset = _secondsRemaining - Mathf.FloorToInt(_secondsRemaining);
                if (!_isFlashing && offset < 0.1)
                {
                    _timeFeedbacks.PlayFeedbacks();
                    _isFlashing = true;
                }
                else if (_isFlashing && offset > 0.9)
                {
                    _isFlashing = false;
                }
            }
        }

        void DisplayTime(float timeToDisplay, bool full = false)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            float milliseconds = Mathf.FloorToInt(timeToDisplay * 100 % 100);

            if (minutes > 0)
            {
                if (full)
                {
                    _timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
                }
                else
                {
                    _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
            }
            else
            {
                _timeText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
            }
        }

        private void HideTimer()
        {
            if (_hideFeedbacks)
            {
                _hideFeedbacks.PlayFeedbacks();
            }

            if (!IsCountdown)
            {
                _isFinished = true;
                DisplayTime(_currentTime, true);
                OnTimeStop?.Invoke(_timeText.text);
            }
        }

        private void ShowTimer()
        {
            _hasStarted = true;
            _isFinished = false;
            _secondsRemaining = numberOfSeconds;
            if (_showFeedbacks)
            {
                _showFeedbacks.PlayFeedbacks();
            }
        }

        public void DebugShowTimer()
        {
            ShowTimer();
        }

        private void OnEnable()
        {
            LevelWin.OnLevelWin += HideTimer;
            // @todo reset on level start.
        }

        private void OnDisable()
        {
            LevelWin.OnLevelWin -= HideTimer;
        }
    }
}
