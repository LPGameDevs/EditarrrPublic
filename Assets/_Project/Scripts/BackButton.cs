using Editarrr.Level;
using LevelEditor;
using UnityEngine;
using LevelManager = Singletons.LevelManager;

/**
 * Button logic for leaving the game level and returning to the editor.
 */
[RequireComponent(typeof(EditorLevelSelector))]
public class BackButton : MonoBehaviour
{
    private EditorLevelSelector _selector;

    private void Awake()
    {
        _selector = GetComponent<EditorLevelSelector>();
    }

    /**
     * Safety mechanism to prevent levels being edited once they are
     * published.
     */
    public void ButtonClicked(LevelState levelData)
    {
        // Once the level is published it can no longer be edited.
        if (levelData.Published)
        {
            _selector.LevelName = LevelManager.LevelSelectionSceneName;
        }

        _selector.GoToLevel();
    }
}
