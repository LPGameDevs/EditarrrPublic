using System;
using Editarrr.Audio;
using Misc;
using Singletons;
using TMPro;
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
    [SerializeField] TMP_InputField _streamerChannel;
    [SerializeField] TMP_InputField _targetFps;

    private void Start()
    {
        Application.targetFrameRate = PreferencesManager.Instance.GetFps();
        QualitySettings.vSyncCount = 1;


        _musicSlider.Initialize();
        _sfxSlider.Initialize();

        _screenShakeToggle.isOn = ScreenShakeEnabled = PreferencesManager.Instance.GetBoolean(PreferencesManager.ScreenShakeKey);
        _screenFlashToggle.isOn = ScreenFlashEnabled = PreferencesManager.Instance.GetBoolean(PreferencesManager.ScreenFlashKey);
        _streamerChannel.text = PreferencesManager.Instance.GetStreamerChannel();
        _targetFps.text = PreferencesManager.Instance.GetFps().ToString();
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
        string currentSavedChannel = PreferencesManager.Instance.GetStreamerChannel();
        if (_streamerChannel.text != currentSavedChannel)
        {
            PreferencesManager.Instance.SetStreamerChannel(_streamerChannel.text);
        }

        int currentSavedFps = PreferencesManager.Instance.GetFps();
        if (_targetFps.text != currentSavedFps.ToString())
        {
            int newFps = Int32.Parse(_targetFps.text);
            newFps = Mathf.Min(120, newFps);
            newFps = Mathf.Max(30, newFps);
            _targetFps.text = newFps.ToString();
            PreferencesManager.Instance.SetFps(newFps);
            Application.targetFrameRate = newFps;
        }

        AudioManager.Instance.PlayAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME);
        _settingsOverlay.SetActive(false);
    }

    public void ToggleScreenShake(bool setActive) => PreferencesManager.Instance.SetBoolean(PreferencesManager.ScreenShakeKey, setActive);

    public void ToggleScreenFlash(bool setActive) => PreferencesManager.Instance.SetBoolean(PreferencesManager.ScreenFlashKey, setActive);

}
