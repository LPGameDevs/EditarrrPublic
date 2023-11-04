using System;
using System.Collections.Generic;
using Browser;
using Editarrr.Audio;
using Editarrr.Level;
using Editarrr.Input;
using Level.Storage;
using Singletons;
using UnityEngine;

public class LevelBrowserLoader : MonoBehaviour
{
    [SerializeField] private LevelBrowserLevel _levelPrefab;
    [SerializeField] private GameObject _loadingOverlay;
    [field: SerializeField, Tooltip("Pause input")] private InputValue PauseInput { get; set; }

    private LevelManager _levelManager;
    private List<LevelBrowserLevel> _loadedLevels = new List<LevelBrowserLevel>();

    private void Awake()
    {
        // SteamManager.Instance.Init();
    }

    private void Start()
    {
        StartLoading();
    }

    private void Update()
    {
        if(PauseInput.WasPressed)
            CloseBrowser();
    }

    public void SetLevelManager(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private void StartLoading()
    {
        _loadingOverlay.SetActive(true);
    }

    private void StopLoading()
    {
        _loadingOverlay.SetActive(false);
    }

    public void DestroyLevels()
    {
        // Remove all existing levels.
        foreach (var level in _loadedLevels)
        {
            Destroy(level.gameObject);
        }
        _loadedLevels = new List<LevelBrowserLevel>();
        StartLoading();
    }

    public void AddLevelPrefabFromData(LevelStub levelStub)
    {
        string userId = PreferencesManager.Instance.GetUserId();
        LevelBrowserLevel level;

        level = Instantiate(_levelPrefab, transform);

        // Set visual information on the level from data.
        level.SetTitle(levelStub.Code);
        level.SetCreator(levelStub.CreatorName);
        level.SetRemoteId(levelStub.RemoteId);

        string screenshotPath = _levelManager.GetScreenshotPath(levelStub.Code);
        level.SetScreenshot(screenshotPath, true);

        // level.SetScreenshot(screenshotPath);

        if (this._levelManager.LevelExists(levelStub.Code))
        {
            level.SetDownloaded();
        }

        // Dont allow someone to edit a level if they didnt create it.
        if (levelStub.Creator.ToLower() != userId.ToLower())
        {
            level.HideEditorTools();
        }

        _loadedLevels.Add(level);
    }

    public void CloseBrowser()
    {
        SceneTransitionManager.Instance.RemoveScene(SceneTransitionManager.BrowserSceneName);
        AudioManager.Instance.PlayAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME);
    }

    public void SetLevels(LevelStub[] levels)
    {
        StopLoading();
        foreach (var level in levels)
        {
            AddLevelPrefabFromData(level);
        }
        // The scroll content needs to be resized to fit all the levels.
        RectTransform currentTransform = GetComponent<RectTransform>();
        var contentHeight = _loadedLevels.Count * (_levelPrefab.GetComponent<RectTransform>().rect.height + 25);
        currentTransform.sizeDelta = new Vector2(currentTransform.sizeDelta.x, contentHeight);

        // Scroll to the top of the content by subtracting the offset.
        // 680 is roughly the height of the scroll view.
        var contentOffset = contentHeight > 680 ? (contentHeight - 680) / 2 : 0;
        currentTransform.anchoredPosition = new Vector2(currentTransform.anchoredPosition.x, -contentOffset);
    }

    public void ReloadScreenshot(string code)
    {
        foreach (var level in _loadedLevels)
        {
            if (level.Code != code)
            {
                continue;
            }

            string screenshotPath = _levelManager.GetScreenshotPath(code);
            level.SetScreenshot(screenshotPath);
            break;
        }

    }
}
