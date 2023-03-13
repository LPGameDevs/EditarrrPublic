using System.Collections.Generic;
using System.IO;
using CorgiExtension;
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

    void Start()
    {
        DestroyAndRefreshLevels();
    }

    /**
     * Destroy all levels in the selection scene and reload.
     *
     * This is easier than trying to track what has changed.
     */
    private void DestroyAndRefreshLevels()
    {
        // Remove all existing levels.
        foreach (var level in _loadedLevels)
        {
            Destroy(level.gameObject);
        }

        _loadedLevels = new List<Transform>();

        var info = EditorLevelStorage.Instance.GetStoredLevelFiles();
        foreach (FileInfo f in info)
        {
            // Ignore the level reset template.
            if (f.Name == "level.json")
            {
                continue;
            }

            // Get the level code from the file name without the extension.
            string levelCode = f.Name.Remove(f.Name.Length - f.Extension.Length);
            SetupLevelPrefabByCode(levelCode);
        }
    }

    /**
     * Lookup the saved level data from the filename and create a level prefab.
     */
    private void SetupLevelPrefabByCode(string code)
    {
        LevelSave levelData = EditorLevelStorage.Instance.GetLevelData(code);
        string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);
        EditorLevel level;

        if (levelData.published)
        {
            level = Instantiate(LevelPrefab, transform);
        }
        else
        {
            level = Instantiate(DraftPrefab, transform);
        }

        // Set visual information on the level from data.
        level.SetTitle(code);
        level.SetCreator(levelData.creator);
        level.SetScreenshot(code);

        // Dont allow someone to edit a level if they didnt create it.
        if (levelData.creator.ToLower() != userName.ToLower())
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
