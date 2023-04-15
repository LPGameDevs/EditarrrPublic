using System;
using Editarrr.Level;
using LevelEditor;
using TMPro;
using UnityEngine;

namespace Legacy
{
    public class WinMenu : MonoBehaviour
    {
        public static event Action<string, string> OnScoreSubmit;
        public static event Action OnScoreSubmitted;

        public TMP_Text LevelCode;
        public TMP_Text TimerText;
        public Transform SubmitButton;
        public Transform BackButton;
        public Transform BackEditorButton;

        private string _code;
        private string _user;
        private string _time;

        public void SetCode(string code)
        {
            _code = code;
            LevelCode.text = _code.ToUpper();
        }

        public void SetLevelData(LevelState levelData)
        {
            if (levelData.Creator.Length > 0)
            {
                _user = levelData.Creator;
                LevelCode.text += " by " + _user;
            }

            if (levelData.Published)
            {
                SubmitButton.gameObject.SetActive(true);
                BackButton.gameObject.SetActive(true);
            }
            else
            {
                BackEditorButton.gameObject.SetActive(true);
            }
        }

        private void SetTimeText(string timeText)
        {
            _time = timeText;
            if (TimerText != null)
            {
                TimerText.text = timeText;
            }
        }

        public void SubmitScore()
        {
            OnScoreSubmit?.Invoke(_code, _time);
            EditorLevelStorage.Instance.SubmitLevelScore(_code, _time);
        }

        private void FinishSubmitScore()
        {
            OnScoreSubmitted?.Invoke();
        }

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
