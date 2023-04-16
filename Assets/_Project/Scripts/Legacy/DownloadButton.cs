using Editarrr.Level;
using UnityEngine;

namespace Legacy
{
    public class DownloadButton : MonoBehaviour
    {
        void Start()
        {
            // Hide this button when we cannot download levels.
            if (!LevelManager.RemoteStorageEnabled)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
