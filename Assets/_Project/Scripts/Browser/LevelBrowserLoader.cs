using System.Collections.Generic;
using Browser;
using Editarrr.Level;
using Level.Storage;
using Singletons;
using UnityEngine;

public class LevelBrowserLoader : MonoBehaviour
{
    private LevelManager _levelManager;

    [SerializeField] private LevelBrowserLevel _levelPrefab;
    private List<LevelBrowserLevel> _loadedLevels = new List<LevelBrowserLevel>();

    private void Awake()
    {
        // SteamManager.Instance.Init();
    }

    public void SetLevelManager(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    public void DestroyLevels()
    {
        // Remove all existing levels.
        foreach (var level in _loadedLevels)
        {
            Destroy(level.gameObject);
        }
        _loadedLevels = new List<LevelBrowserLevel>();
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

    public void GoToSelection()
    {
        SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
    }

    public void SetLevels(LevelStub[] levels)
    {
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
            if (level.Title.text != code)
            {
                continue;
            }

            string screenshotPath = _levelManager.GetScreenshotPath(code);
            level.SetScreenshot(screenshotPath);
            break;
        }

    }
}
