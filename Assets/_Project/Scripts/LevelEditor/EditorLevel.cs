using System;
using System.IO;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class EditorLevel : MonoBehaviour
    {
        public static event Action<string> OnEditorLevelSelected;
        public static event Action<string> OnEditorLevelUpload;
        public static event Action<string> OnEditorLevelDelete;
        public static event Action<string> OnLeaderboardRequest;

        public Text Title;
        public Text Creator;
        public Text RemoteId;
        public RawImage ScreenshotImage;
        public Transform EditButton;
        public Transform UploadButton;
        public Transform DeleteButton;
        public string Code { get; protected set; } = "";

        public void DeleteLevel()
        {
            // @todo Remove this
            // EditorLevelStorage.Instance.DeleteLevel(_code);

            OnEditorLevelDelete?.Invoke(Code);
        }

        public void UploadLevel()
        {
            OnEditorLevelUpload?.Invoke(Code);
        }

        public void GoToEditorLevel()
        {
            CheckCodePreferences();
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.CreateLevelSceneName);
        }

        private void CheckCodePreferences()
        {
            string newCode = "";
            if (Code.Length > 0)
            {
                newCode = Code;
            }

            OnEditorLevelSelected?.Invoke(newCode);
        }

        public void GetLevelLeaderboard()
        {
            OnLeaderboardRequest?.Invoke(Code);
        }

        public void GoToReplay()
        {
            CheckCodePreferences();
            SceneTransitionManager.Instance.GoToScene("EditorReplay");
        }

        public void GoToLevel()
        {
            CheckCodePreferences();
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.TestLevelSceneName);
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

        public void SetTitle(string code)
        {
            Title.text = "lvl: " + code.ToUpper();
            Code = code.ToLower();
        }

        public void SetCreator(string creator)
        {
            Creator.text = "by: " + creator.ToUpper();
        }

        public void SetRemoteId(string remoteId)
        {
            if (RemoteId == null)
            {
                return;
            }
            RemoteId.text = remoteId;
        }

        public virtual void SetScreenshot(string path, bool retry = false)
        {
            if (ScreenshotImage && File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);

                ScreenshotImage.texture = tex;
                ScreenshotImage.color = Color.white;
            }
            else if (ScreenshotImage)
            {
                ScreenshotImage.color = Color.black;
            }
        }
    }
}
