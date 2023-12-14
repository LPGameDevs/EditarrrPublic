using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DownloadByCode : MonoBehaviour
{
    public static event Action<string> OnSearchLevelByCodeRequested;
    public static event Action<string> OnDownloadLevelByCodeRequested;

    [SerializeField] private TMP_InputField codeInput;
    [SerializeField] private Button downloadButton;
    [SerializeField] private Button searchButton;

    public void OnDownloadButtonPressed()
    {
        string code = codeInput.text;
        code = Regex.Replace(code, "[^0-9]", "");
        if (code.Length != 5)
        {
            return;
        }

        OnDownloadLevelByCodeRequested?.Invoke(code);
    }

    public void OnSearchButtonPressed()
    {
        string code = codeInput.text;
        code = Regex.Replace(code, "[^0-9]", "");
        if (code.Length == 0 || code.Length > 5)
        {
            OnSearchLevelByCodeRequested?.Invoke("");
            return;
        }

        OnSearchLevelByCodeRequested?.Invoke(code);

    }
}
