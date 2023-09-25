using CorgiExtension;
using Editarrr.Level;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
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

    private void LevelStorage_AllLevelsLoadedCallback(LevelState[] levels)
    {
        foreach (var level in levels)
        {
            string screenshotPath = LevelManager.GetScreenshotPath(level.Code);
            _levelLoader.AddLevelPrefabFromData(level, screenshotPath);
        }
    }

    private void OnLevelDownload(Steamworks.Ugc.Item item)
    {
        Debug.Log("Download started for level " + item.Id + ".");

        // Update display.
        DestroyAndRefreshLevels();
    }

    private void OnLevelDownloadComplete(string code)
    {
        Debug.Log("Download finished for level " + code + ".");
        // Update display.
        DestroyAndRefreshLevels();
    }

    public override void DoOnEnable()
    {
        LevelBrowserLevel.OnBrowserLevelDownload += OnLevelDownload;
        LevelBrowserLevel.OnBrowserLevelDownloadComplete += OnLevelDownloadComplete;
    }

    public override void DoOnDisable()
    {
        LevelBrowserLevel.OnBrowserLevelDownload -= OnLevelDownload;
        LevelBrowserLevel.OnBrowserLevelDownloadComplete -= OnLevelDownloadComplete;
    }
}
