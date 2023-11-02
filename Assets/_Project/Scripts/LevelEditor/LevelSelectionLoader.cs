using System.Collections.Generic;
using Editarrr.Audio;
using Level.Storage;
using LevelEditor;
using Singletons;
using UnityEngine;

/**
 * Responsible for showing levels on the selection screen.
 */
public class LevelSelectionLoader : MonoBehaviour
{
    public EditorLevel LevelPrefab;
    public EditorLevel DraftPrefab;

    private List<Transform> _loadedLevels = new List<Transform>();

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

        // Dont allow someone to edit a level if they didnt create it.
        if (levelData.Creator.ToLower() != userId.ToLower())
        {
            level.HideEditorTools();
        }

        _loadedLevels.Add(level.transform);
    }

    public void OpenBrowser()
    {
        SceneTransitionManager.Instance.AddScene(SceneTransitionManager.BrowserSceneName);
        AudioManager.Instance.PlayRandomizedAudioClip(AudioManager.BUTTONCLICK_CLIP_NAME, 0.1f, 0.1f);
    }
}
