using Editarrr.Audio;
using Misc;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : UnityPersistentSingleton<SettingsManager>
{
    public static bool ScreenShakeEnabled {  get; private set; }
    public static bool ScreenFlashEnabled { get; private set; }

    [SerializeField] MixerVolume _musicSlider, _sfxSlider;
    [SerializeField] Toggle _screenShakeToggle, _screenFlashToggle;
    [SerializeField] GameObject _settingsOverlay;
    [SerializeField] GameObject _settingsButton;

    private void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 1;


        _musicSlider.Initialize();
        _sfxSlider.Initialize();

        _screenShakeToggle.isOn = ScreenShakeEnabled = PreferencesManager.Instance.GetBoolean(PreferencesManager.ScreenShakeKey);
        _screenFlashToggle.isOn = ScreenFlashEnabled = PreferencesManager.Instance.GetBoolean(PreferencesManager.ScreenFlashKey);
    }

    private void OnEnable()
    {
        SceneTransitionManager.OnSceneChanged += ToggleSettingsButton;
    }

    private void ToggleSettingsButton(string sceneName)
    {
        _settingsButton.SetActive(!sceneName.Equals(SceneTransitionManager.TestLevelSceneName));
    }

    public void OpenSettingsMenu()
    {
        AudioManager.Instance.PlayAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME);
        _settingsOverlay.SetActive(true);
    }

    public void CloseSettingsMenu() 
    {
        AudioManager.Instance.PlayAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME);
        _settingsOverlay.SetActive(false);
    }

    public void InitializeToggle(Toggle toggleElement, string settingName)
    {
        toggleElement.isOn = PreferencesManager.Instance.GetBoolean(settingName);
    }

    public void ToggleScreenShake(bool setActive) => PreferencesManager.Instance.SetBoolean(PreferencesManager.ScreenShakeKey, setActive);

    public void ToggleScreenFlash(bool setActive) => PreferencesManager.Instance.SetBoolean(PreferencesManager.ScreenFlashKey, setActive);

}
