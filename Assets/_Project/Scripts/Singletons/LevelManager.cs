using UnityEngine.SceneManagement;

namespace Singletons
{
    /**
     * Level Manager Singleton
     *
     * Responsible for:
     *  - Loading levels
     *  - Player spawning
     *  - Level/camera boundaries and collision
     *  - Player life/death tracking and respawn
     */
    public class LevelManager : UnitySingleton<LevelManager>
    {

        public static string LevelSelectionSceneName = "EditorSelection";
        public static string TestLevelSceneName = "EditorTest";
        public static string CreateLevelSceneName = "EditorCreate";

        public void GotoLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void RestartLevel()
        {
            GotoLevel(SceneManager.GetActiveScene().name);
        }
    }
}
