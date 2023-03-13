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
            LevelManager.Instance.GotoLevel(LevelName);
        }

        /**
         * Restarts the current level.
        */
        public void RestartLevel()
        {
            LevelManager.Instance.RestartLevel();
        }


        public void GoToLevelSelection()
        {
            LevelManager.Instance.GotoLevel(LevelManager.LevelSelectionSceneName);
        }
    }
}
