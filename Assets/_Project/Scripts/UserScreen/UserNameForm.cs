using System;
using Singletons;
using SteamIntegration;
using TMPro;
using UnityEngine;

/**
 * This class allows a user to choose a username.
 */
public class UserNameForm : MonoBehaviour
{
    public const string UserIdStorageKey = "UserId";
    public const string UserNameStorageKey = "UserName";
    public const string DefaultUserName = "anon";

    public TMP_InputField UserNameInput;

    private void Awake()
    {
        // Initialise steam.
        SteamManager.Instance.Init();
    }

    private void Start()
    {
        // If we already have saved a username then use that.
        string userName = PlayerPrefs.GetString(UserNameStorageKey);
        if (userName.Length > 0 && userName != DefaultUserName)
        {
            UserNameInput.text = userName;
        }

        string userId = PlayerPrefs.GetString(UserIdStorageKey);
        if (userId.Length == 0)
        {
            userId = new Guid().ToString();
            PlayerPrefs.SetString(UserIdStorageKey, userId);
        }
    }

    public void SubmitForm()
    {
        string oldUserName = PlayerPrefs.GetString(UserNameStorageKey, DefaultUserName);
        string newUserName = UserNameInput.text ?? DefaultUserName;

        // If the username changes then we need to reset the level data.
        // This is easier than trying to migrate the data.
        if (oldUserName != newUserName)
        {
            // Reset any previous level data.
            // @todo If there is any temporary level data unsaved, remove it.
        }

        // We store the username in player prefs. Its not sensitive data so this is fine.
        PlayerPrefs.SetString(UserNameStorageKey, newUserName);

        SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
    }
}
