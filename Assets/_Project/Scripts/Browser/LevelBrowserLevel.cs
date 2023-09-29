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
    }
}
