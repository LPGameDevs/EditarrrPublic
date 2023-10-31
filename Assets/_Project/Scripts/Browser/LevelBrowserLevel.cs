using System;
using System.IO;
using Editarrr.Audio;
using LevelEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Browser
{
    public class LevelBrowserLevel : EditorLevel
    {
        public static event Action<string> OnBrowserLevelDownloadScreenshot;
        public static event Action<string> OnBrowserLevelDownload;

        public Button DownloadButton;

        public void OnDownloadButtonPressed()
        {
            string remote = RemoteId.text;

            if (remote.Length == 0)
            {
                remote = Code;
            }
            DownloadButton.interactable = false;
            OnBrowserLevelDownload?.Invoke(remote);
            AudioManager.Instance.PlayRandomizedAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME, 0.1f, 0.1f);
        }

        public void SetDownloaded()
        {
            DownloadButton.interactable = false;
        }

        public override void SetScreenshot(string path, bool retry = false)
        {
            if (!File.Exists(path))
            {
                if (retry)
                {
                    OnBrowserLevelDownloadScreenshot?.Invoke(Code);
                }
                return;
            }

            base.SetScreenshot(path, retry);
        }
    }
}
