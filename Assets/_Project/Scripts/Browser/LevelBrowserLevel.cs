using System;
using CorgiExtension;
using UnityEngine;

namespace Browser
{
    public class LevelBrowserLevel : EditorLevel
    {
        public static event Action<string> OnBrowserLevelDownload;

        public Transform DownloadButton;

        public void OnDownloadButtonPressed()
        {
            string code = Title.text;
            OnBrowserLevelDownload?.Invoke(code);
        }

        public void SetDownloaded()
        {
            DownloadButton.gameObject.SetActive(false);
        }
    }
}
