using System;
using UnityEngine;

public class LevelAnalytics : MonoBehaviour
{
    public static event Action<string> OnLevelPlayStart;
    public static event Action<string> OnLevelEditStart;

    public bool IsEditor;

    void Start()
    {
        string levelCode = PlayerPrefs.GetString("EditorCode");

        if (IsEditor)
        {
            OnLevelEditStart?.Invoke(levelCode);
        }
        else
        {
            OnLevelPlayStart?.Invoke(levelCode);
        }
    }
}
