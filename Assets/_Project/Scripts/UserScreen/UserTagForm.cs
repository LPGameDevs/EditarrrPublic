using Singletons;
using SteamIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum UserTagType
{
    None = 0,
    GDFG = 1,
    Stream = 2,
}

public class UserTagForm : MonoBehaviour
{
    public TMP_Text TitleBox;
    public TMP_InputField UserNameInput;
    public Transform NextStep;
    private UserTagType _currentTag = UserTagType.None;
    public Button GdfgButton, StreamButton, FunButton;

    private void Awake()
    {
        // Initialise steam.
        SteamManager.Instance.Init();
        Debug.Log("Application Version : " + Application.version);
    }

    private void Start()
    {
        TitleBox.text = "What are you here for?";
        TitleBox.fontSize = 100;
    }

    public void SubmitForm()
    {
        PreferencesManager.Instance.SetUserTypeTag(_currentTag);

        bool isStreaming = this._currentTag == UserTagType.Stream;

        if (!isStreaming)
        {
            PreferencesManager.Instance.SetOnboarded();
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
            return;
        }

        // Move on to streaming.
        NextStep.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void ClearAllButtons()
    {
        var colours = FunButton.colors;
        colours.normalColor = Color.white;

        FunButton.colors = colours;
        GdfgButton.colors = colours;
        StreamButton.colors = colours;
    }

    public void SelectFun()
    {
        ClearAllButtons();

        _currentTag = UserTagType.None;

        var colours = FunButton.colors;
        colours.normalColor = colours.selectedColor;
        FunButton.colors = colours;
    }



    public void SelectGdfg()
    {
        ClearAllButtons();

        _currentTag = UserTagType.GDFG;

        var colours = GdfgButton.colors;
        colours.normalColor = colours.selectedColor;
        GdfgButton.colors = colours;
    }

    public void SelectStreamer()
    {
        ClearAllButtons();

        _currentTag = UserTagType.Stream;

        var colours = StreamButton.colors;
        colours.normalColor = colours.selectedColor;
        StreamButton.colors = colours;
    }
}
