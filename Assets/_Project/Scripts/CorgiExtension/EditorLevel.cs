using System;
using System.Collections;
using LevelEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CorgiExtension
{
    public class EditorLevel : Level
    {
        public static event Action<string> OnEditorLevelUpload;
        public static event Action<string> OnLeaderboardRequest;

        public float FadeOutDuration = 1f;
        public Text Title;
        public Text Creator;
        public Transform EditButton;
        public Transform UploadButton;
        public Transform DeleteButton;
        public string SceneName;


        public void DeleteLevel()
        {
            EditorLevelStorage.Instance.DeleteLevel(Title.text.ToLower());
        }

        public void UploadLevel()
        {
            OnEditorLevelUpload?.Invoke(Title.text.ToLower());
        }

        public void GoToEditorLevel()
        {
            CheckCodePreferences();
            SceneName = "EditorCreate";

            // @todo fade in.

            // if the user presses the "Jump" button, we start the first level.
            StartCoroutine (LoadLevel ());
        }

        private void CheckCodePreferences()
        {
            if (SceneName.Length == 0)
            {
                PlayerPrefs.SetString("EditorCode", Title.text.ToLower());
            }
            else
            {
                PlayerPrefs.SetString("EditorCode", "");
            }
        }

        public void GetLevelLeaderboard()
        {
            string code = Title.text.ToLower();
            OnLeaderboardRequest?.Invoke(code);
            EditorLevelStorage.Instance.GetLevelScores(code);
        }

        public void GoToReplay()
        {
            CheckCodePreferences();
            SceneName = "EditorReplay";
            // @todo fade in.
            // if the user presses the "Jump" button, we start the first level.
            StartCoroutine (LoadLevel ());
        }

        public void GoToLevel()
        {
            CheckCodePreferences();
            SceneName = "EditorTest";
            // @todo fade in.
            // if the user presses the "Jump" button, we start the first level.
            StartCoroutine (LoadLevel ());
        }

        protected virtual IEnumerator LoadLevel()
        {
            yield return new WaitForSeconds (FadeOutDuration);

            // @todo load level
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
    }
}
