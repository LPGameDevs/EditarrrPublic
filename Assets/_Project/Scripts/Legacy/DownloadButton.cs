using LevelEditor;
using UnityEngine;

namespace Legacy
{
    public class DownloadButton : MonoBehaviour
    {
        void Start()
        {
            if (!EditorLevelStorage._remoteStorageEnabled)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
