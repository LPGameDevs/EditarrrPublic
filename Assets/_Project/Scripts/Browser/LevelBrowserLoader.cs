using System.Collections.Generic;
using Editarrr.Level;
using SteamIntegration;
using UnityEngine;

public class LevelBrowserLoader : MonoBehaviour
{
    [SerializeField] private LevelBrowserLevel _levelPrefab;
    [SerializeField] private LevelManager _levelManager;

    private List<Transform> _loadedLevels = new List<Transform>();


    private void Awake()
    {
        SteamManager.Instance.Init();
        // _levelManager.LoadAll(this.LevelStorage_AllLevelsLoadedCallback);
    }


    // private void LevelStorage_AllLevelsLoadedCallback(LevelState[] levels)
    // {
    //     foreach (var level in levels)
    //     {
    //         string screenshotPath = _levelManager.GetScreenshotPath(level.Code);
    //         AddLevelPrefabFromData(level, screenshotPath);
    //     }
    // }

    public void DestroyLevels()
    {
        // Remove all existing levels.
        foreach (var level in _loadedLevels)
        {
            Destroy(level.gameObject);
        }
        _loadedLevels = new List<Transform>();
    }

    public void AddLevelPrefabFromData(LevelState levelData, string screenshotPath)
    {
        string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);
        LevelBrowserLevel level;

        level = Instantiate(_levelPrefab, transform);

        // Set visual information on the level from data.
        level.SetTitle(levelData.Code);
        level.SetCreator(levelData.Creator);
        level.SetScreenshot(screenshotPath);

        // Dont allow someone to edit a level if they didnt create it.
        if (levelData.Creator.ToLower() != userName.ToLower())
        {
            level.HideEditorTools();
        }

        _loadedLevels.Add(level.transform);
    }
}
