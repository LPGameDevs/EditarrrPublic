using Singletons;
using SteamIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserStreamForm : MonoBehaviour
{
    public TMP_Text TitleBox;
    public TMP_InputField StreamChannelInput;
    [SerializeField] Toggle _streamerToggle;


    private void Awake()
    {
        // Initialise steam.
        SteamManager.Instance.Init();
        Debug.Log("Application Version : " + Application.version);
    }

    private void Start()
    {
        TitleBox.text = "Who is streaming then?";
        TitleBox.fontSize = 100;
        _streamerToggle.isOn = PreferencesManager.Instance.GetBoolean(PreferencesManager.StreamerKey, 0);
        StreamChannelInput.text = PreferencesManager.Instance.GetStreamerChannel();
    }

    public void SubmitForm()
    {
        // Needs a streamer channel.
        if (StreamChannelInput.text.Length == 0)
        {
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.StartSceneName);
            return;
        }

        string oldChannel = PreferencesManager.Instance.GetStreamerChannel();
        string newChannel = StreamChannelInput.text;

        if (oldChannel != newChannel)
        {
            PreferencesManager.Instance.SetStreamerChannel(newChannel);
        }

        if (_streamerToggle.isOn)
        {
            PreferencesManager.Instance.SetIsStreamer();
        }

        PreferencesManager.Instance.SetOnboarded();
        SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
    }

    public void ToggleStreamer(bool setActive)
    {
        PreferencesManager.Instance.SetBoolean(PreferencesManager.StreamerKey, setActive);
    }
}
