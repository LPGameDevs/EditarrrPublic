using System;
using LevelEditor;
using TMPro;
using UnityEngine;

public class ConfirmSubmitForm : MonoBehaviour
{
    public static event Action<string> OnLevelUpload;
    public TMP_Text Title;
    public TMP_Text Description;
    public Transform SubmitButton;

    private Popup _popup;
    private string _code;

    private void Awake()
    {
        _popup = GetComponent<Popup>();
    }

    private void SetCode(string code)
    {
        _code = code;
    }

    public void OpenPopup(string code)
    {
        SetCode(code);
        Title.text = $"Submit {_code.ToUpper()}";
        Description.gameObject.SetActive(true);
        SubmitButton.gameObject.SetActive(true);

        Description.text = "This will publish your level to the website and you will not be able to make any more changes.";
        _popup.Open();
    }

    public void ClosePopup()
    {
        Description.gameObject.SetActive(false);
        SubmitButton.gameObject.SetActive(false);
        _popup.Close();
    }

    public void SubmitForm()
    {
        EditorLevelStorage.Instance.UploadLevel(_code.ToLower());
        OnLevelUpload?.Invoke(_code.ToLower());
        ClosePopup();
    }

    private void OnEnable()
    {
        EditorLevel.OnEditorLevelUpload += OpenPopup;
    }

    private void OnDisable()
    {
        EditorLevel.OnEditorLevelUpload -= OpenPopup;
    }
}
