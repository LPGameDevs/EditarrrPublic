using System;
using System.Collections.Generic;
using CorgiExtension;
using Editarrr.Level;
using LevelEditor;
using UnityEngine;

/**
 * Responsible for showing levels on the selection screen.
 */
public class LevelSelectionLoader : MonoBehaviour
{
    public EditorLevel LevelPrefab;
    public EditorLevel DraftPrefab;

    private List<Transform> _loadedLevels = new List<Transform>();

    /**
     * Destroy all levels in the selection scene and reload.
     *
     * This is easier than trying to track what has changed.
     */
    private void DestroyAndRefreshLevels()
    {
        throw new NotImplementedException("This method is no longer in use.");
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

    /**
     * Lookup the saved level data from the filename and create a level prefab.
     */
    public void AddLevelPrefabFromData(LevelState levelData, string screenshotPath)
    {
        string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);
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
        level.SetCreator(levelData.Creator);
        level.SetScreenshot(screenshotPath);

        // Dont allow someone to edit a level if they didnt create it.
        if (levelData.Creator.ToLower() != userName.ToLower())
        {
            level.HideEditorTools();
        }

        _loadedLevels.Add(level.transform);
    }

    private void OnEnable()
    {
        EditorLevelStorage.OnLevelRefresh += DestroyAndRefreshLevels;
    }

    private void OnDisable()
    {
        EditorLevelStorage.OnLevelRefresh -= DestroyAndRefreshLevels;
    }
}
