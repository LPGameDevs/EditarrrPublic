using System;
using System.Collections.Generic;
using Editarrr.Audio;
using Level.Storage;
using LevelEditor;
using Singletons;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum LevelFilterMetric
{
    None = 0,
    Demo = 1,
    Draft = 2,
    Published = 3,
    Downloaded = 4,
}

/**
 * Responsible for showing levels on the selection screen.
 */
public class LevelSelectionLoader : MonoBehaviour
{
    public static event Action OnFilterChanged;

    public EditorLevel LevelPrefab;
    public EditorLevel DraftPrefab;
    public GameObject LoadingOverlay;

    [SerializeField] UISelectableAugment[] _filterButtons;

    private List<Transform> _loadedLevels = new List<Transform>();
    private LevelFilterMetric CurrentFilter = LevelFilterMetric.None;

    public void DestroyLevels()
    {
        // Remove all existing levels.
        foreach (var level in _loadedLevels)
        {
            Destroy(level.gameObject);
        }
        _loadedLevels = new List<Transform>();
    }

    /**
     * Lookup the saved level data from the filename and create a level prefab.
     */
    public void AddLevelPrefabFromData(LevelStub levelData, string screenshotPath)
    {
        string userId = PreferencesManager.Instance.GetUserId();
        EditorLevel level;

        if (levelData.Published)
        {
            level = Instantiate(LevelPrefab, transform);
        }
        else
        {
            level = Instantiate(DraftPrefab, transform);
        }

        // Set visual information on the level from data.
        level.SetTitle(levelData.Code);
        level.SetCreator(levelData.CreatorName);
        level.SetScreenshot(screenshotPath);
        level.SetScores(levelData.TotalScores);
        level.SetRatings(levelData.TotalRatings);
        level.SetLables(levelData.Labels);

        // Dont allow someone to edit a level if they didnt create it.
        if (levelData.Creator.ToLower() != userId.ToLower())
        {
            level.HideEditorTools();
        }

        if (levelData.IsDistro)
        {
            level.HideDeleteButton();
        }

        _loadedLevels.Add(level.transform);
    }

    public bool LevelFilterApplies(LevelStub level)
    {
        bool filterApplies = false;
        string currentUserId = PreferencesManager.Instance.GetUserId();

        switch (this.CurrentFilter)
        {
            default:
            case LevelFilterMetric.None:
                filterApplies = true;
                break;

            case LevelFilterMetric.Demo:
                if (level.IsDistro)
                {
                    filterApplies = true;
                }
                break;

            case LevelFilterMetric.Draft:
                if (!level.IsDistro && !level.Published)
                {
                    filterApplies = true;
                }
                break;

            case LevelFilterMetric.Published:
                if (!level.IsDistro && level.Published && level.Creator == currentUserId)
                {
                    filterApplies = true;
                }
                break;

            case LevelFilterMetric.Downloaded:
                if (!level.IsDistro && level.Published && level.Creator != currentUserId)
                {
                    filterApplies = true;
                }
                break;
        }

        if (filterApplies)
        {
            Debug.Log(level.Code);
            Debug.Log(currentUserId);
            Debug.Log(level.Creator);
        }

        return filterApplies;
    }

    public void ButtonPressedFilterDemos()
    {
        if (this.CurrentFilter == LevelFilterMetric.Demo)
        {
            SetLevelFilter(LevelFilterMetric.None);
            return;
        }

        Debug.Log("Filter by Demos");
        SetLevelFilter(LevelFilterMetric.Demo);
    }

    public void ButtonPressedFilterDrafts()
    {
        if (this.CurrentFilter == LevelFilterMetric.Draft)
        {
            SetLevelFilter(LevelFilterMetric.None);
            return;
        }

        Debug.Log("Filter by Drafts");
        SetLevelFilter(LevelFilterMetric.Draft);
    }

    public void ButtonPressedFilterPublished()
    {
        if (this.CurrentFilter == LevelFilterMetric.Published)
        {
            SetLevelFilter(LevelFilterMetric.None);
            return;
        }

        Debug.Log("Filter by Published");
        SetLevelFilter(LevelFilterMetric.Published);
    }

    public void ButtonPressedFilterDownloaded()
    {
        if (this.CurrentFilter == LevelFilterMetric.Downloaded)
        {
            SetLevelFilter(LevelFilterMetric.None);
            return;
        }

        Debug.Log("Filter by Downloaded");
        SetLevelFilter(LevelFilterMetric.Downloaded);
    }

    private void SetLevelFilter(LevelFilterMetric filter)
    {
        CurrentFilter = filter;
        foreach(var button in _filterButtons)
            button.LockIn(button.NameTag.Equals(filter.ToString()));

        AudioManager.Instance.PlayAudioClip(AudioManager.SNAP_CLIP_NAME);
        OnFilterChanged?.Invoke();
    }

    public void OpenBrowser()
    {
        SceneTransitionManager.Instance.AddScene(SceneTransitionManager.BrowserSceneName);
        AudioManager.Instance.PlayRandomizedAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME, 0.1f, 0.1f);
    }

    private void OnCloseBrowser()
    {
        SetLevelFilter(LevelFilterMetric.Downloaded);
    }

    private void OnEnable()
    {
        LevelBrowserLoader.OnLevelBrowserClosed += OnCloseBrowser;
    }

    private void OnDisable()
    {
        LevelBrowserLoader.OnLevelBrowserClosed -= OnCloseBrowser;
    }
}
