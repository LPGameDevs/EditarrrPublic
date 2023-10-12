using Browser;
using Editarrr.Level;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using Level.Storage;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBrowserManager", menuName = "Managers/Level/new Level Browser Manager")]
public class LevelBrowserManager : ManagerComponent
{
    private const string Documentation = "This handles loading levels for level browser.";

    [field: SerializeField, Info(Documentation), Header("Managers")] private LevelManager LevelManager { get; set; }

    // From system
    private LevelBrowserLoader _levelLoader { get; set; }


    public void SetLevelLoader(LevelBrowserLoader levelLoader)
    {
        _levelLoader = levelLoader;
        _levelLoader.SetLevelManager(LevelManager);
    }

    public override void DoAwake()
    {
        LevelManager.DoAwake();
    }

    public override void DoStart()
    {
        LevelManager.DoStart();
        this.DestroyAndRefreshLevels();
    }

    private void DestroyAndRefreshLevels()
    {
        _levelLoader.DestroyLevels();

        LevelManager.LoadAll(this.LevelStorage_AllLevelsLoadedCallback);
    }

    private void LevelStorage_AllLevelsLoadedCallback(LevelStub[] levels)
    {
        _levelLoader.SetLevels(levels);
    }

    private void OnLevelDownloadRequested(string code)
    {
        Debug.Log("Download requested for level " + code + ".");
        LevelManager.Download(code, OnLevelDownloadComplete);

        // Update display - this is only necessary with long downloads.
        // DestroyAndRefreshLevels();
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
        DestroyAndRefreshLevels();
    }

#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
    private void OnSteamLevelDownloadComplete(Steamworks.Ugc.Item item)
    {
        // @todo Make sure we also store the steam id with the level
        // as it didnt exist at time of upload.

        // save level to local storage.
        LevelManager.CopyLevelFromSteamDirectory(item);

        // Update display.
        DestroyAndRefreshLevels();
    }
#endif

    public override void DoOnEnable()
    {
        LevelBrowserLevel.OnBrowserLevelDownload += OnLevelDownloadRequested;
        LevelBrowserLevel.OnBrowserLevelDownloadScreenshot += OnLevelScreenshotDownloadRequested;
#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
        RemoteLevelStorageProviderSteam.OnSteamLevelDownloadComplete += OnSteamLevelDownloadComplete;
#endif
        // LevelBrowserLevel.OnBrowserLevelDownload += OnLevelDownload;
        // LevelBrowserLevel.OnBrowserLevelDownloadComplete += OnLevelDownloadComplete;
    }

    public override void DoOnDisable()
    {
        LevelBrowserLevel.OnBrowserLevelDownload -= OnLevelDownloadRequested;
        LevelBrowserLevel.OnBrowserLevelDownloadScreenshot -= OnLevelScreenshotDownloadRequested;
#if !UNITY_WEBGL && !UNITY_EDITOR_OSX
        RemoteLevelStorageProviderSteam.OnSteamLevelDownloadComplete -= OnSteamLevelDownloadComplete;
#endif
        // LevelBrowserLevel.OnBrowserLevelDownload -= OnLevelDownload;
        // LevelBrowserLevel.OnBrowserLevelDownloadComplete -= OnLevelDownloadComplete;
    }
}
