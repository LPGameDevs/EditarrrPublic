using Singletons;
using SteamIntegration;
using System.Text.RegularExpressions;
using System;
using TMPro;
using UnityEngine;
using Editarrr.Audio;

/**
 * This class allows a user to choose a username.
 */
public class UserNameForm : MonoBehaviour
{
    public TMP_InputField UserNameInput;

    [SerializeField] Color _neutralColor, _errorColor, _charLimitColor;

    private string _cachedNameText = "";
    const int CHAR_LIMIT = 20;


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

    public void ValidateInput(string newText)
    {
        if(newText.Length > CHAR_LIMIT)
        {
            UserNameInput.text = _cachedNameText;
            UserNameInput.targetGraphic.CrossFadeColor(_charLimitColor, 0.1f, true, true);
            AudioManager.Instance.PlayAudioClip(AudioManager.DENIED_CLIP_NAME);
            return;
        }

        //Filter out all chars except alphanumerics and spaces, and allow only single spaces
        string adjustedNewText = CleanInput(newText);

        //Invalid char or double space entered
        if (newText != adjustedNewText)
        {
            UserNameInput.text = adjustedNewText;
            UserNameInput.targetGraphic.CrossFadeColor(_errorColor, 0.1f, true, true);
            AudioManager.Instance.PlayAudioClip(AudioManager.DENIED_CLIP_NAME);
        }
        //Valid input
        else
        {
            UserNameInput.targetGraphic.CrossFadeColor(_neutralColor, 0.1f, true, true);
            if(adjustedNewText.Length > _cachedNameText.Length)
                AudioManager.Instance.PlayAudioClip(AudioManager.EDITOR_PLACE_CLIP_NAME);
            else
                AudioManager.Instance.PlayAudioClip(AudioManager.EDITOR_REMOVE_CLIP_NAME);
        }

        _cachedNameText = adjustedNewText;
    }

    private string CleanInput(string input)
    {
        string cleanedInput;
        try
        {
            cleanedInput = Regex.Replace(input, @"\s+", " ");
            cleanedInput = Regex.Replace(cleanedInput, @"[^a-zA-Z0-9\s]", "",
                                 RegexOptions.None, TimeSpan.FromSeconds(1.5));
        }
        catch (RegexMatchTimeoutException exc)
        {
            cleanedInput = "ERROR";
            Debug.Log("UserName Regex timed out | " + exc.Source);
        }

        return cleanedInput;
    }

    public void TrimInput(string input) => UserNameInput.text = input.Trim();

    public void SubmitForm()
    {
        string oldUserName = PreferencesManager.Instance.GetUserName();
        //Clean input when submitting to block manual registry edits
        string newUserName = CleanInput(UserNameInput.text).Trim() ?? PreferencesManager.DefaultUserName;
        if(newUserName.Length > CHAR_LIMIT)
            newUserName = newUserName.Substring(0, CHAR_LIMIT - 1);

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
