using Singletons;
using SteamIntegration;
using TMPro;
using UnityEngine;

/**
 * This class allows a user to choose a username.
 */
public class UserNameForm : MonoBehaviour
{
    public TMP_InputField UserNameInput;

    private void Awake()
    {
        // Initialise steam.
        SteamManager.Instance.Init();
        Debug.Log("Application Version : " + Application.version);
    }

    private void Start()
    {
        // If we already have saved a username then use that.
        string userName = PreferencesManager.Instance.GetUserName();

        if (userName.Length > 0 && userName != PreferencesManager.DefaultUserName)
        {
            UserNameInput.text = userName;
        }

        // Initialise a new user id if we dont have one.
        PreferencesManager.Instance.GetUserId();
    }

    public void SubmitForm()
    {
        string oldUserName = PreferencesManager.Instance.GetUserName();
        string newUserName = UserNameInput.text ?? PreferencesManager.DefaultUserName;

        // If the username changes then we need to reset the level data.
        // This is easier than trying to migrate the data.
        if (oldUserName != newUserName)
        {
            // Reset any previous level data.
            // @todo If there is any temporary level data unsaved, remove it.
        }

        // We store the username in player prefs. Its not sensitive data so this is fine.
        PreferencesManager.Instance.SetUserName(newUserName);

        string session = PreferencesManager.Instance.StartNewSession();
        AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.UserSessionStarted, session);

        SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
    }
}
