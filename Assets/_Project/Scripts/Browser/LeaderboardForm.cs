using Editarrr.Audio;
using Editarrr.Input;
using Level.Storage;
using Singletons;
using System;
using TMPro;
using UnityEngine;

namespace Browser
{
    public class LeaderboardForm : MonoBehaviour
    {
        public static event Action<string, RemoteScoreStorage_AllScoresLoadedCallback> OnLeaderboardRefreshRequested;

        public Transform Rows;
        public LeaderboardRow RowPrefab;
        public GameObject LoadingOverlay;

        [field: SerializeField, Tooltip("Pause input")] private InputValue PauseInput { get; set; }
        [field: SerializeField] private TMP_Text Code { get; set; }

        private string _code;

        private void Update()
        {
            if(PauseInput.WasPressed)
                CloseButtonPressed();
        }

        public void SetCode(string code)
        {
            _code = code;
            Code.text = code;
        }

        public void SetScores(ScoreStub[] scoreStubs)
        {
            foreach (Transform child in Rows) {
                Destroy(child.gameObject);
            }

            int i = 0;
            foreach (ScoreStub scoreStub in scoreStubs)
            {
                i++;
                if (i > 5) break;

                if (i == 1 && scoreStub.Creator == PreferencesManager.Instance.GetUserId() /*&& ToDo: not level creator*/)
                    AchievementManager.Instance.UnlockAchievement(GameAchievement.LevelLeaderboardTopped);

                LeaderboardRow row = Instantiate(RowPrefab, Rows);
                row.SetRow(_code, i.ToString(), scoreStub.Score, scoreStub.CreatorName);
            }

            LoadingOverlay.SetActive(false);
        }

        public void CloseButtonPressed()
        {
            foreach (Transform child in Rows) {
                Destroy(child.gameObject);
            }
            gameObject.SetActive(false);
            LoadingOverlay.SetActive(true);
            AudioManager.Instance.PlayAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME);
        }

        public void RefreshButtonPressed()
        {
            LoadingOverlay.SetActive(true);
            OnLeaderboardRefreshRequested?.Invoke(this._code, LeaderboardScoresLoaded);
            AudioManager.Instance.PlayAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME);


            void LeaderboardScoresLoaded(ScoreStub[] scoreStubs)
            {
                this.SetScores(scoreStubs);
                LoadingOverlay.SetActive(false);
            }
        }
    }
}
