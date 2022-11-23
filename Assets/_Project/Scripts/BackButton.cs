using System;
using _Project.Scripts.CorgiExtension;
using LevelEditor;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    private TOLevelSelector _selector;

    private void Awake()
    {
        _selector = GetComponent<TOLevelSelector>();
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
