using Editarrr.Input;
using Player;
using UnityEngine;
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

        [field: SerializeField, Tooltip("Restart input map")] private InputValue RestartInput { get; set; }
        [field: SerializeField, Tooltip("Scene reloads after this duration")] private float TransitionTime { get; set; }

        bool restartInitiated;

        private void OnEnable()
        {
            HealthSystem.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            HealthSystem.OnDeath -= OnDeath;
        }

        private void Update()
        {
            if (RestartInput.WasPressed && !restartInitiated)
                TransitionedRestart();
        }

        public void GotoLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void RestartLevel()
        {
            GotoLevel(SceneManager.GetActiveScene().name);
        }

        public void TransitionedRestart()
        {
            restartInitiated = true;
            //TODO: Play sfx/jingles, transitiion animations, etc.
            Invoke(nameof(RestartLevel), TransitionTime);
        }

        private void OnDeath (object sender, System.EventArgs e)
        {
            TransitionedRestart();
        }
    }
}
