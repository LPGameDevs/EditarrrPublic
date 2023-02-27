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

    public void GotoLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
