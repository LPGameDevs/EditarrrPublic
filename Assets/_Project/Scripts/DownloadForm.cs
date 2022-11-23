using System;
using LevelEditor;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DownloadForm : MonoBehaviour
{
    public static event Action<string> OnLevelDownload;

    public UnityEvent OnSubmitFormComplete;
    public TMP_InputField LevelCodeInput;
    public TMP_Text Title;
    public TMP_Text Description;
    public Transform SubmitButton;

    private Popup _popup;

    private void Awake()
    {
        _popup = GetComponent<Popup>();
    }

    public void OpenPopup()
    {
        Title.text = "DOWNLOAD LEVEL";
        LevelCodeInput.gameObject.SetActive(true);
        SubmitButton.gameObject.SetActive(true);
        Description.gameObject.SetActive(true);
        Description.text = "Enter the code of the level you would like to download.";
        _popup.Open();
    }

    public void ClosePopup()
    {
        LevelCodeInput.gameObject.SetActive(false);
        SubmitButton.gameObject.SetActive(false);
        Description.gameObject.SetActive(false);
        _popup.Close();
    }

    public void SubmitForm()
    {
        OnLevelDownload?.Invoke(LevelCodeInput.text);
        EditorLevelStorage.Instance.DownloadLevel(LevelCodeInput.text);
    }

    private void SubmitFormComplete()
    {
        OnSubmitFormComplete?.Invoke();
    }

    private void OnEnable()
    {
        EditorLevelStorage.OnLevelRefresh += SubmitFormComplete;
    }

    private void OnDisable()
    {
        EditorLevelStorage.OnLevelRefresh -= SubmitFormComplete;
    }
}
