using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HelperText : MonoBehaviour
{
    public TMP_Text Text;
    [TextArea(10,5)]
    public string[] TextList;
    public UnityEvent CloseHelp;

    private int _currentTextIndex = 0;

    private void Start()
    {
        Text.text = TextList[0].ToUpper();
    }

    public void ShowNext()
    {
        _currentTextIndex++;

        if (TextList.Length > _currentTextIndex)
        {
            Text.text = TextList[_currentTextIndex].ToUpper();
        }
        else
        {
            _currentTextIndex = 0;
            Text.text = TextList[0].ToUpper();
            CloseHelp?.Invoke();
        }
    }
}
