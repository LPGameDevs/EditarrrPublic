using System;
using LevelEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CorgiExtension
{
    public class EditorLevel : Level
    {
        public static event Action<string> OnEditorLevelUpload;
        public static event Action<string> OnLeaderboardRequest;

        public Text Title;
        public Text Creator;
        public Transform EditButton;
        public Transform UploadButton;
        public Transform DeleteButton;
        private string _code = "";

        public void DeleteLevel()
        {
            EditorLevelStorage.Instance.DeleteLevel(_code);
        }

        public void UploadLevel()
        {
            OnEditorLevelUpload?.Invoke(_code);
        }

        public void GoToEditorLevel()
        {
            CheckCodePreferences();
            LevelManager.Instance.GotoLevel("EditorCreate");
        }

        private void CheckCodePreferences()
        {
            if (_code.Length > 0)
            {
                PlayerPrefs.SetString("EditorCode", _code);
            }
            else
            {
                PlayerPrefs.SetString("EditorCode", "");
            }
        }

        public void GetLevelLeaderboard()
        {
            OnLeaderboardRequest?.Invoke(_code);
            EditorLevelStorage.Instance.GetLevelScores(_code);
        }

        public void GoToReplay()
        {
            CheckCodePreferences();
            LevelManager.Instance.GotoLevel("EditorReplay");
        }

        public void GoToLevel()
        {
            CheckCodePreferences();
            LevelManager.Instance.GotoLevel("EditorTest");
        }

        public void HideEditorTools()
        {
            if (EditButton)
            {
                EditButton.gameObject.SetActive(false);
            }

            if (UploadButton)
            {
                UploadButton.gameObject.SetActive(false);
            }
        }

        public void HideDeleteButton()
        {
            if (DeleteButton)
            {
                DeleteButton.gameObject.SetActive(false);
            }
        }

        public void setTitle(string title)
        {
            Title.text = title;
            _code = title;
        }
    }
}
