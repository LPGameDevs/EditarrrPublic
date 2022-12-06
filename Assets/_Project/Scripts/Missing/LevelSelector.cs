using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string LevelName;


    public virtual void GoToLevel()
    {
        LevelManager.Instance.GotoLevel(LevelName);
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void RestartLevel()
    {
        LevelManager.Instance.GotoLevel(SceneManager.GetActiveScene().name);
    }
}
