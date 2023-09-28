using System.Collections.Generic;
using Editarrr.Level;
using Level.Storage;
using SteamIntegration;
using UnityEngine;

public class LevelBrowserLoader : MonoBehaviour
{
    [SerializeField] private LevelBrowserLevel _levelPrefab;
    private List<Transform> _loadedLevels = new List<Transform>();

    private void Awake()
    {
        SteamManager.Instance.Init();
    }

    public void DestroyLevels()
    {
        // Remove all existing levels.
        foreach (var level in _loadedLevels)
        {
            Destroy(level.gameObject);
        }
        _loadedLevels = new List<Transform>();
    }

    public void AddLevelPrefabFromData(LevelStub levelData, string screenshotPath)
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
