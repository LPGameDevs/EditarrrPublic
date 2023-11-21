using System;
using System.Collections.Generic;
using System.IO;
using Editarrr.Audio;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class EditorLevel : MonoBehaviour
    {
        public static event Action<string> OnEditorLevelSelected;
        public static event Action OnEditorLevelPlayRequest;
        public static event Action<string> OnEditorLevelUpload;
        public static event Action<string> OnEditorLevelDelete;
        public static event Action<string> OnLeaderboardRequest;

        public TMP_Text Title;
        public TMP_Text Creator;
        public TMP_Text RemoteId;
        public TMP_Text Scores;
        public TMP_Text Ratings;
        public RawImage ScreenshotImage;
        public Transform EditButton;
        public Transform UploadButton;
        public Transform DeleteButton;
        public string Code { get; protected set; } = "";
        public List<string> Labels { get; set; } = new List<string>();

        public void HideLevel() => gameObject.SetActive(false);
        public void ShowLevel() => gameObject.SetActive(true);

        public void DeleteLevel()
        {
            // @todo Remove this
            // EditorLevelStorage.Instance.DeleteLevel(_code);

            OnEditorLevelDelete?.Invoke(Code);
        }

        public void UploadLevel()
        {
            OnEditorLevelUpload?.Invoke(Code);
            //AudioManager.Instance.PlayAudioClip(AudioManager.AFFIRMATIVE_CLIP_NAME);
        }

        public void GoToEditorLevel()
        {
            CheckCodePreferences();
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.CreateLevelSceneName);
        }

        public void GoToEditorLevel(int type)
        {
            PreferencesManager.Instance.SetUserTypeTag((UserTagType)type);
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
            AudioManager.Instance.PlayAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME);
        }

        public void GoToReplay()
        {
            CheckCodePreferences();
            SceneTransitionManager.Instance.GoToScene("EditorReplay");
        }

        public void GoToLevel()
        {
            CheckCodePreferences();
            OnEditorLevelPlayRequest?.Invoke();
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
            Title.text = "Lvl: " + code;
            Code = code.ToLower();
        }

        public void SetCreator(string creator)
        {
            Creator.text = "By: " + creator;
        }

        public void SetScores(int totalScores)
        {
            if (Scores == null)
                return;

            Scores.text = $"Total scores: {totalScores}";
        }

        public void SetRatings(int totalRatings)
        {
            if (Ratings == null)
                return;

            Ratings.text = $"Total ratings: {totalRatings}";
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
            //else if (ScreenshotImage)
            //{
            //    ScreenshotImage.color = Color.black;
            //}
        }

        public void SetLables(List<string> incomingLabels)
        {
            Labels = incomingLabels;
        }
    }
}
