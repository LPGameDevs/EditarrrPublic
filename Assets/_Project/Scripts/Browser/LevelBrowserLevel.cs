using System;
using System.IO;
using LevelEditor;
using UnityEngine;

namespace Browser
{
    public class LevelBrowserLevel : EditorLevel
    {
        public static event Action<string> OnBrowserLevelDownloadScreenshot;
        public static event Action<string> OnBrowserLevelDownload;

        public Transform DownloadButton;

        public void OnDownloadButtonPressed()
        {
            string remote = RemoteId.text;

            if (remote.Length == 0)
            {
                remote = Title.text;
            }
            OnBrowserLevelDownload?.Invoke(remote);
        }

        public void SetDownloaded()
        {
            DownloadButton.gameObject.SetActive(false);
        }

        public override void SetScreenshot(string path, bool retry = false)
        {
            if (!File.Exists(path))
            {
                if (retry)
                {
                    OnBrowserLevelDownloadScreenshot?.Invoke(Title.text);
                }
                return;
            }

            base.SetScreenshot(path, retry);
        }
    }
}
