using System;
using Browser;
using Editarrr.Audio;
using Editarrr.Level;
using Level.Storage;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.GUI
{
    public class WinMenu : MonoBehaviour
    {
        public static event Action<string, RemoteScoreStorage_AllScoresLoadedCallback> OnLeaderboardRequested;
        public static event Action<string, float> OnScoreSubmit;
        public static event Action<string, int> OnRatingSubmit;

        public TMP_Text LevelCode;
        public TMP_Text TimerText;
        public Button HomeButton, BackEditorButton, ReplayButton, SubmitButton;
        public bool IsReplay = false;

        [SerializeField] Animator _animator;
        [SerializeField] GameObject _leaderBoard;
        [SerializeField] RatingMenu _ratingMenu;


        const string VICTORY_TRIGGER_NAME = "Victory";

        private string _code;
        private string _user;
        private float _time;
        private string _timeText;
        private LevelState _levelData;

        private void UpdateContent()
        {
            DisableAllButtons();
            LevelCode.text = _code.ToUpper();

            if (_levelData == null)
            {
                return;
            }

            if (_levelData.CreatorName.Length > 0)
            {
                _user = _levelData.CreatorName;
                LevelCode.text += " by " + _user;
            }

            // Home is always a valid option.
            HomeButton.interactable = true;

            if (IsReplay)
            {
                ReplayButton.interactable = true;
                SubmitButton.interactable = false;
            }
            else if (_levelData.Published)
            {
                ReplayButton.interactable = true;
                SubmitButton.interactable = true;
            }
            else
            {
                ReplayButton.interactable = true;
                BackEditorButton.interactable = true;
                SubmitButton.interactable = false;
            }
        }

        private void DisableAllButtons()
        {
            ReplayButton.interactable = false;
            SubmitButton.interactable = false;
            HomeButton.interactable = false;
            BackEditorButton.interactable = false;
        }

        public void Show()
        {
            UpdateContent();
            _animator.SetTrigger(VICTORY_TRIGGER_NAME);

            // @todo only show this if its not the players own level.
            if (_user != PreferencesManager.Instance.GetUserName())
            {
                AchievementManager.Instance.UnlockAchievement(GameAchievement.LevelCompleted);
            }
        }

        public void SetCode(string code)
        {
            _code = code;

            UpdateContent();
        }

        public void SetLevelData(LevelState levelData)
        {
            _levelData = levelData;

            UpdateContent();
        }

        private void SetTimeText(float time, string timeText)
        {
            _time = time;
            _timeText = timeText;
            if (TimerText != null)
            {
                TimerText.text = timeText;
            }
        }

        public void SubmitScore()
        {
            Debug.Log("Submitting score");
            OnScoreSubmit?.Invoke(_code, _time);
            SubmitButton.onClick.RemoveAllListeners();
            SubmitButton.onClick.AddListener(OpenScoreboard);
            OpenScoreboard();
        }

        public void OpenScoreboard()
        {
            _leaderBoard.SetActive(true);

            var leaderboard = _leaderBoard.GetComponent<LeaderboardForm>();
            leaderboard.SetCode(this._code);

            OnLeaderboardRequested?.Invoke(this._code, LeaderboardScoresLoaded);

            void LeaderboardScoresLoaded(ScoreStub[] scoreStubs)
            {
                leaderboard.SetScores(scoreStubs);
            }

            AudioManager.Instance.PlayAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME);
        }

        public void OpenRatingMenu()
        {
            _ratingMenu.OpenMenu(_code);
            AudioManager.Instance.PlayAudioClip(AudioManager.AFFIRMATIVE_CLIP_NAME);
        }

        public void CloseRatingMenu()
        {
            _ratingMenu.UpdateRating(0);
            _ratingMenu.gameObject.SetActive(false);
            AudioManager.Instance.PlayAudioClip(AudioManager.NEGATIVE_CLIP_NAME);
        }

        #region ButtonTriggers

        public void OnClickReplay()
        {
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.TestLevelSceneName);
        }

        public void OnClickSubmitScore()
        {
            if(PreferencesManager.Instance.HasLevelRating(_code))
                SubmitScore();
            else
                OpenRatingMenu();
        }

        public void OnClickBack()
        {
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
        }

        public void OnClickBackEditor()
        {
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.CreateLevelSceneName);
        }

        public void SubmitRating(int rating)
        {
            OnRatingSubmit?.Invoke(_code, rating);
            OpenScoreboard();
        }
        #endregion

        protected void OnEnable()
        {
            Timer.OnTimeStop += SetTimeText;
            SubmitButton.onClick.AddListener(OnClickSubmitScore);
        }

        protected void OnDisable()
        {
            Timer.OnTimeStop -= SetTimeText;
            SubmitButton.onClick.RemoveAllListeners();
        }

        public void DeactivateWinMenuAnimator() => _animator.enabled = false;
    }
}
