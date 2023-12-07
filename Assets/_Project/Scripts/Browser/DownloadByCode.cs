using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DownloadByCode : MonoBehaviour
{
    public static event Action<string> OnDownloadLevelByCodeRequested;
    
    [SerializeField] private TMP_Text codeInput;
    [SerializeField] private Button downloadButton;

    public void OnDownloadButtonPressed()
    {
        string code = codeInput.text;
        if (code.Length != 5)
        {
            return;
        } 
        
        OnDownloadLevelByCodeRequested?.Invoke(code);
        
    }
}
