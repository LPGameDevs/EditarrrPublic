using System.Collections.Generic;
using Browser;
using Editarrr.Level;
using Level.Storage;
using Singletons;
using SteamIntegration;
using UnityEngine;

public class LevelBrowserLoader : MonoBehaviour
{
    private LevelManager _levelManager;

    [SerializeField] private LevelBrowserLevel _levelPrefab;
    private List<Transform> _loadedLevels = new List<Transform>();

    private void Awake()
    {
        SteamManager.Instance.Init();
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
        _loadedLevels = new List<Transform>();
    }

    public void AddLevelPrefabFromData(LevelStub levelStub)
    {
        string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);
        LevelBrowserLevel level;

        level = Instantiate(_levelPrefab, transform);

        // Set visual information on the level from data.
        level.SetTitle(levelStub.Code);
        level.SetCreator(levelStub.Creator);
        level.SetRemoteId(levelStub.RemoteId);
        // level.SetScreenshot(screenshotPath);

        if (this._levelManager.LevelExists(levelStub.Code))
        {
            level.SetDownloaded();
        }

        // Dont allow someone to edit a level if they didnt create it.
        if (levelStub.Creator.ToLower() != userName.ToLower())
        {
            level.HideEditorTools();
        }

        _loadedLevels.Add(level.transform);
    }

    public void GoToSelection()
    {
        SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
    }
}
