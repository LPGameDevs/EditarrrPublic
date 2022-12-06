using System;
using System.IO;
using LevelEditor;
using TMPro;
using UnityEngine;

public class UserNameForm : MonoBehaviour
{
    public static event Action<string> OnNameChosen;

    public const string UserNameStorageKey = "UserName";
    public const string DefaultUserName = "anon";

    public TMP_InputField UserNameInput;

    private void Start()
    {
        string userName = PlayerPrefs.GetString(UserNameStorageKey);
        if (userName.Length > 0 && userName != DefaultUserName)
        {
            UserNameInput.text = userName;
        }
    }

    public void SubmitForm()
    {
        string oldUserName = PlayerPrefs.GetString(UserNameStorageKey, DefaultUserName);
        string newUserName = UserNameInput.text ?? DefaultUserName;

        if (oldUserName != newUserName)
        {
            File.WriteAllText(EditorLevelStorage.LevelStorageEditorLevel, "{}");
        }

        PlayerPrefs.SetString(UserNameStorageKey, newUserName);
        OnNameChosen?.Invoke(UserNameInput.text ?? DefaultUserName);
        // @todo Transition to next scene.
    }
}
