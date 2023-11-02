using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button _backEditorButton;

    internal void ToggleEditorButton(bool levelPublished)
    {
        _backEditorButton.interactable = !levelPublished;
    }
}
