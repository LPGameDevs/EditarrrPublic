using System;
using Gameplay;
using Gameplay.GUI;
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
        public static event Action<string> OnTimeStop;

        [SerializeField] private MMFeedbacks _hideFeedbacks;
        [SerializeField] private MMFeedbacks _showFeedbacks;

        private float _currentTime;

        private TextMeshProUGUI _timeText;
        private bool _hasStarted;
        private bool _isFinished;
        private bool _isFlashing;
        private bool _isPaused;

        private void Start()
        {
            _timeText = GetComponentInChildren<TextMeshProUGUI>();
            ShowTimer();
        }

        void Update()
        {
            if (_isPaused)
            {
                return;
            }

            if (_isFinished || !_hasStarted)
            {
                return;
            }

            HandleTimer();
        }

        private void HandleTimer()
        {
            _currentTime += Time.deltaTime;
            DisplayTime(_currentTime);
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

            _isFinished = true;
            DisplayTime(_currentTime, true);
            OnTimeStop?.Invoke(_timeText.text);
        }

        private void ShowTimer()
        {
            _hasStarted = true;
            _isFinished = false;
            if (_showFeedbacks)
            {
                _showFeedbacks.PlayFeedbacks();
            }
        }

        private void PauseTimer(bool isPaused)
        {
            _isPaused = isPaused;
        }

        private void OnEnable()
        {
            Chest.OnChestOpened += HideTimer;
            GameplayGuiManager.OnGamePauseChanged += PauseTimer;
        }

        private void OnDisable()
        {
            Chest.OnChestOpened -= HideTimer;
            GameplayGuiManager.OnGamePauseChanged -= PauseTimer;
        }
    }
}
