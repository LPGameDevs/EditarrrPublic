using System.Collections;
using System.Collections.Generic;
using LevelEditor;
using UnityEngine;

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
