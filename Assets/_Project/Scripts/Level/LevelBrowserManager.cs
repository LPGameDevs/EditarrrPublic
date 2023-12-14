using System;
using Browser;
using Editarrr.Level;
using Editarrr.Managers;
using Editarrr.Misc;
using Level.Storage;
using Singletons;
using System.Linq;
using UnityEngine;
using static SortingSelector;

[CreateAssetMenu(fileName = "LevelBrowserManager", menuName = "Managers/Level/new Level Browser Manager")]
public class LevelBrowserManager : ManagerComponent
{
    private const string Documentation = "This handles loading levels for level browser.";

    public static event Action<LevelStub[]> OnRemoteLevelsLoaded;

    [field: SerializeField, Info(Documentation), Header("Managers")] private LevelManager LevelManager { get; set; }

    // From system
    private LevelBrowserLoader _levelLoader { get; set; }
    private RemoteLevelLoadQuery LevelQuery { get; set; }
    private string NextCursor, PreviousCursor = "";
    BrowserPager.PagerRequestResultHasMore PagerRequestResultHasMoreCallback { get; set; }

    SortingState _currentSortingState = SortingState.Inactive;
    string _sortingCriterionName = "";


    public void SetLevelLoader(LevelBrowserLoader levelLoader)
    {
        _levelLoader = levelLoader;
        _levelLoader.SetLevelManager(LevelManager);
    }

    public override void DoAwake()
    {
        LevelManager.DoAwake();
        this.PreviousCursor = "";
        this.LevelQuery = new RemoteLevelLoadQuery()
        {
            limit = 50,
            cursor = "",
            code = "",
            sort = "",
            direction = SortDirection.Ascending,
        };
    }

    public override void DoStart()
    {
        LevelManager.DoStart();
        this.DestroyAndRefreshLevels();
    }

    private void DestroyAndRefreshLevels()
    {
        _levelLoader.DestroyLevels();

        LevelManager.LoadAll(SetLevelsAfterLoad, this.LevelQuery);

        void SetLevelsAfterLoad(LevelStub[] levels, string cursor = "")
        {
            IOrderedEnumerable<LevelStub> orderedlevels = null;
            if (SortingState.Ascending == _currentSortingState)
                orderedlevels = levels.OrderBy(x => typeof(LevelStub).GetProperty(_sortingCriterionName).GetValue(x));
            else if (SortingState.Descending == _currentSortingState)
                orderedlevels = levels.OrderByDescending(x => typeof(LevelStub).GetProperty(_sortingCriterionName).GetValue(x));
            else
                orderedlevels = levels.OrderBy(x => x.Code);

            this._levelLoader.SetLevels(orderedlevels.ToArray());
            this.NextCursor = cursor;
            this.PagerRequestResultHasMoreCallback?.Invoke(levels.Length == this.LevelQuery.limit);
            this.PagerRequestResultHasMoreCallback = null;

            OnRemoteLevelsLoaded?.Invoke(levels);
        }
    }

    private void OnLevelDownloadRequested(string code)
    {
        Debug.Log("Download requested for level " + code + ".");
        LevelManager.Download(code, OnLevelDownloadComplete);

        // Update display - this is only necessary with long downloads.
        // DestroyAndRefreshLevels();
        AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.LevelDownload, code);
    }

    private void OnLevelSearchRequested(string code)
    {
        var remoteLevelLoadQuery = this.LevelQuery;
        remoteLevelLoadQuery.code = code;
        this.LevelQuery = remoteLevelLoadQuery;
        DestroyAndRefreshLevels();
    }

    private void OnLevelSortRequested(string sort, SortDirection direction = SortDirection.Ascending)
    {
        var remoteLevelLoadQuery = this.LevelQuery;
        remoteLevelLoadQuery.sort = sort;
        remoteLevelLoadQuery.direction = direction;
        this.LevelQuery = remoteLevelLoadQuery;
        DestroyAndRefreshLevels();
    }

    private void OnLevelScreenshotDownloadRequested(string code)
    {
        LevelManager.DownloadScreenshot(code, OnLevelScreenshotDownloadComplete);

        void OnLevelScreenshotDownloadComplete()
        {
            _levelLoader.ReloadScreenshot(code);
        }
    }

    private void OnLevelDownloadStarted(LevelStub level)
    {
        Debug.Log("Download started for level " + level.Code + ".");

        // Update display - this is only necessary with long downloads.
        // DestroyAndRefreshLevels();
    }

    private void OnLevelDownloadComplete(LevelSave level)
    {
        // save level to local storage.
        LevelManager.SaveDownloadedLevel(level);

        Debug.Log("Download finished for level " + level.Code + ".");

        // Update display.
        //DestroyAndRefreshLevels();
    }

#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
    private void OnSteamLevelDownloadComplete(Steamworks.Ugc.Item item)
    {
        // @todo Make sure we also store the steam id with the level
        // as it didnt exist at time of upload.

        // save level to local storage.
        LevelManager.CopyLevelFromSteamDirectory(item);

        // Update display.
        //DestroyAndRefreshLevels();
    }
#endif

    private void OnBrowserPagerUpdateRequested(int update, BrowserPager.PagerRequestResultHasMore callback)
    {
        this.PagerRequestResultHasMoreCallback = callback;
        var remoteLevelLoadQuery = this.LevelQuery;
        remoteLevelLoadQuery.cursor = "";
        if (update == 1)
        {
            this.PreviousCursor = remoteLevelLoadQuery.cursor;
            remoteLevelLoadQuery.cursor = this.NextCursor;
        }
        else if (update == -1)
        {
            this.NextCursor = remoteLevelLoadQuery.cursor;
            remoteLevelLoadQuery.cursor = this.PreviousCursor;
        }

        this.LevelQuery = remoteLevelLoadQuery;
        DestroyAndRefreshLevels();
    }

    private void OnSortingCriteriaChanged(SortingState doNotUse, SortingSelector newSelector)
    {
        _currentSortingState = newSelector.CurrentState;
        _sortingCriterionName = newSelector.SortingCriterionName;
        DestroyAndRefreshLevels();
    }

    public void OnLabelFilterChanged(UserTagType userTag)
    {
        DestroyAndRefreshLevels();
    }

    public override void DoOnEnable()
    {
        this.LevelManager.DoOnEnable();

        LevelBrowserLevel.OnBrowserLevelDownload += OnLevelDownloadRequested;
        DownloadByCode.OnDownloadLevelByCodeRequested += OnLevelDownloadRequested;
        DownloadByCode.OnSearchLevelByCodeRequested += OnLevelSearchRequested;
        LevelBrowserLevel.OnBrowserLevelDownloadScreenshot += OnLevelScreenshotDownloadRequested;
        BrowserPager.OnBrowserPagerUpdated += OnBrowserPagerUpdateRequested;
        SortingSelector.OnStateChanged += OnSortingCriteriaChanged;
        LabelFilterHandler.OnLabelFilterChanged += OnLabelFilterChanged;
#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
        RemoteLevelStorageProviderSteam.OnSteamLevelDownloadComplete += OnSteamLevelDownloadComplete;
#endif
        // LevelBrowserLevel.OnBrowserLevelDownload += OnLevelDownload;
        // LevelBrowserLevel.OnBrowserLevelDownloadComplete += OnLevelDownloadComplete;
    }

    public override void DoOnDisable()
    {
        this.LevelManager.DoOnDisable();

        LevelBrowserLevel.OnBrowserLevelDownload -= OnLevelDownloadRequested;
        DownloadByCode.OnDownloadLevelByCodeRequested -= OnLevelDownloadRequested;
        DownloadByCode.OnSearchLevelByCodeRequested -= OnLevelSearchRequested;
        LevelBrowserLevel.OnBrowserLevelDownloadScreenshot -= OnLevelScreenshotDownloadRequested;
        BrowserPager.OnBrowserPagerUpdated -= OnBrowserPagerUpdateRequested;
        SortingSelector.OnStateChanged -= OnSortingCriteriaChanged;
        LabelFilterHandler.OnLabelFilterChanged -= OnLabelFilterChanged;
#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
        RemoteLevelStorageProviderSteam.OnSteamLevelDownloadComplete -= OnSteamLevelDownloadComplete;
#endif
        // LevelBrowserLevel.OnBrowserLevelDownload -= OnLevelDownload;
        // LevelBrowserLevel.OnBrowserLevelDownloadComplete -= OnLevelDownloadComplete;
    }
}
