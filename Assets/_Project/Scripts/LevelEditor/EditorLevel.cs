using System;
using System.IO;
using LevelEditor;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace CorgiExtension
{
    public class EditorLevel : MonoBehaviour
    {
        public static event Action<string> OnEditorLevelSelected;
        public static event Action<string> OnEditorLevelUpload;
        public static event Action<string> OnEditorLevelDelete;
        public static event Action<string> OnLeaderboardRequest;

        public Text Title;
        public Text Creator;
        public RawImage ScreenshotImage;
        public Transform EditButton;
        public Transform UploadButton;
        public Transform DeleteButton;
        private string _code = "";

        public void DeleteLevel()
        {
            // @todo Remove this
            // EditorLevelStorage.Instance.DeleteLevel(_code);

            OnEditorLevelDelete?.Invoke(_code);
        }

        public void UploadLevel()
        {
            OnEditorLevelUpload?.Invoke(_code);
        }

        public void GoToEditorLevel()
        {
            CheckCodePreferences();
            SceneManager.Instance.GoToScene(SceneManager.CreateLevelSceneName);
        }

        private void CheckCodePreferences()
        {
            string newCode = "";
            if (_code.Length > 0)
            {
                newCode = _code;
            }

            OnEditorLevelSelected?.Invoke(newCode);
        }

        public void GetLevelLeaderboard()
        {
            OnLeaderboardRequest?.Invoke(_code);
        }

        public void GoToReplay()
        {
            CheckCodePreferences();
            SceneManager.Instance.GoToScene("EditorReplay");
        }

        public void GoToLevel()
        {
            CheckCodePreferences();
            SceneManager.Instance.GoToScene(SceneManager.TestLevelSceneName);
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

        public void SetTitle(string title)
        {
            Title.text = title.ToUpper();
            _code = title.ToLower();
        }

        public void SetCreator(string creator)
        {
            Creator.text = creator.ToUpper();
        }

        public void SetScreenshot(string path)
        {
            if (ScreenshotImage && File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);

                ScreenshotImage.texture = tex;
                ScreenshotImage.color = Color.white;
            }
        }
    }
}