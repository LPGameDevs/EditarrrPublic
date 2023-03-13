using UnityEngine.SceneManagement;

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

    public void GotoLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartLevel()
    {
        GotoLevel(SceneManager.GetActiveScene().name);
    }
}
