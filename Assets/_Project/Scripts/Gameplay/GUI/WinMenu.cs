using System;
using Editarrr.Level;
using Singletons;
using TMPro;
using UnityEngine;

namespace Gameplay.GUI
{
    public class WinMenu : MonoBehaviour
    {
        public static event Action<string, float> OnScoreSubmit;
        public static event Action OnScoreSubmitted;

        public TMP_Text LevelCode;
        public TMP_Text TimerText;
        public Transform ReplayButton;
        public Transform SubmitButton;
        public Transform BackButton;
        public Transform BackEditorButton;
        public bool IsReplay = false;

        private string _code;
        private string _user;
        private float _time;
        private string _timeText;
        private LevelState _levelData;

        private void UpdateContent()
        {
            HideAllButtons();
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

            if (IsReplay)
            {
                ReplayButton.gameObject.SetActive(true);
                ReplayButton.GetComponentInChildren<TMP_Text>().text = "AGAIN";
                SubmitButton.gameObject.SetActive(false);
                BackButton.gameObject.SetActive(true);
            }
            else if (_levelData.Published)
            {
                ReplayButton.gameObject.SetActive(true);
                SubmitButton.gameObject.SetActive(true);
                BackButton.gameObject.SetActive(true);
            }
            else
            {
                ReplayButton.gameObject.SetActive(true);
                BackEditorButton.gameObject.SetActive(true);
            }
        }

        private void HideAllButtons()
        {
            ReplayButton.gameObject.SetActive(false);
            SubmitButton.gameObject.SetActive(false);
            BackButton.gameObject.SetActive(false);
            BackEditorButton.gameObject.SetActive(false);
        }

        public void Show()
        {
            // @todo SFX
            UpdateContent();
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
            SubmitButton.gameObject.SetActive(false);
            OnScoreSubmit?.Invoke(_code, _time);
        }

        private void FinishSubmitScore()
        {
            OnScoreSubmitted?.Invoke();
        }

        #region ButtonTriggers

        public void OnClickReplay()
        {
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.TestLevelSceneName);
        }

        public void OnClickSubmit()
        {
            SubmitScore();
        }

        public void OnClickBack()
        {
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
        }

        public void OnClickBackEditor()
        {
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.CreateLevelSceneName);
        }

        #endregion

        protected void OnEnable()
        {
            Timer.OnTimeStop += SetTimeText;
        }

        protected void OnDisable()
        {
            Timer.OnTimeStop -= SetTimeText;
        }
    }
}
