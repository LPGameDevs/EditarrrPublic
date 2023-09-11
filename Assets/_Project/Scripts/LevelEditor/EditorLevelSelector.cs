using Singletons;
using UnityEngine;

namespace LevelEditor
{
    /**
     * Component logic for navigating between levels.
     *
     * Example usage:
     * - Add to a button and navigate to a level on click.
     */
    public class EditorLevelSelector : MonoBehaviour
    {
        // The level to navigate to (chosen in Editor).
        public string LevelName;

        public void GoToLevel()
        {
            SceneManager.Instance.GoToScene(LevelName);
        }

        /**
         * Restarts the current level.
        */
        public void RestartLevel()
        {
            SceneManager.Instance.RestartLevel();
        }


        public void GoToLevelSelection()
        {
            SceneManager.Instance.GoToScene(SceneManager.LevelSelectionSceneName);
        }
    }
}
