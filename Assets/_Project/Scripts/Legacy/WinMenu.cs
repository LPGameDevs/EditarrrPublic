using System;
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

        private void Start()
        {
            _code = PlayerPrefs.GetString("EditorCode");
            LevelSave levelData = EditorLevelStorage.Instance.GetLevelData(_code);
            if (LevelCode != null)
            {
                LevelCode.text = _code.ToUpper();
            }

            if (levelData.creator.Length > 0)
            {
                _user = levelData.creator;
                LevelCode.text += " by " + _user;
            }

            if (levelData.published)
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

        private void FinishSubmitScore(DatabaseRequestType type, PostRequestData data)
        {
            if (type != DatabaseRequestType.InsertComment)
            {
                return;
            }

            OnScoreSubmitted?.Invoke();
        }

        protected void OnEnable()
        {
            EditorLevelStorage.OnRequestComplete += FinishSubmitScore;
            Timer.OnTimeStop += SetTimeText;
        }

        protected void OnDisable()
        {
            Timer.OnTimeStop -= SetTimeText;
        }
    }
}
