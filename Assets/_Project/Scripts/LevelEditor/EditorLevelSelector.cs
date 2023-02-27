using UnityEngine;
using UnityEngine.SceneManagement;

namespace CorgiExtension
{
    public class EditorLevelSelector : MonoBehaviour
    {

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
            LevelManager.Instance.GotoLevel(SceneManager.GetActiveScene().name);
        }

        public void GoToLevelSelection()
        {
            LevelManager.Instance.GotoLevel("EditorSelection");
        }

        protected void OnEnable()
        {
            // @todo Reconnect when we have a win menu.
            // WinMenu.OnScoreSubmitted += GoToLevelSelection;
        }

        protected void OnDisable()
        {
            // WinMenu.OnScoreSubmitted -= GoToLevelSelection;
        }
    }
}
