using CorgiExtension;
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
        foreach (var level in levels)
        {
            string screenshotPath = LevelManager.GetScreenshotPath(level.Code);
            _levelLoader.AddLevelPrefabFromData(level, screenshotPath);
        }
    }

    private void OnLevelDownloadStarted(string code)
    {
        Debug.Log("Download started for level " + code + ".");
        LevelManager.Download(code, OnLevelDownloadComplete);

        // Update display.
        DestroyAndRefreshLevels();
    }

    private void OnLevelDownloadComplete(LevelStub level)
    {
        Debug.Log("Download finished for level " + level.Code + ".");

        // Update display.
        DestroyAndRefreshLevels();
    }

    public override void DoOnEnable()
    {
        LevelBrowserLevel.OnBrowserLevelDownload += OnLevelDownloadStarted;
        // LevelBrowserLevel.OnBrowserLevelDownload += OnLevelDownload;
        // LevelBrowserLevel.OnBrowserLevelDownloadComplete += OnLevelDownloadComplete;
    }

    public override void DoOnDisable()
    {
        LevelBrowserLevel.OnBrowserLevelDownload -= OnLevelDownloadStarted;
        // LevelBrowserLevel.OnBrowserLevelDownload -= OnLevelDownload;
        // LevelBrowserLevel.OnBrowserLevelDownloadComplete -= OnLevelDownloadComplete;
    }
}
