using System;
using CorgiExtension;
using LevelEditor;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    private EditorLevelSelector _selector;

    private void Awake()
    {
        _selector = GetComponent<EditorLevelSelector>();
    }

    public void ButtonClicked()
    {
        string code = PlayerPrefs.GetString("EditorCode");
        var level = EditorLevelStorage.Instance.GetLevelData(code);

        if (level.published)
        {
            _selector.LevelName = "EditorSelection";
        }

        _selector.GoToLevel();
    }
}
