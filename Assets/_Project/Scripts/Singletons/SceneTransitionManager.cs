using Editarrr.Input;
using Player;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Singletons
{
    /**
     * Level Manager Singleton
     *
     * Responsible for:
     *  - Loading scenes
     *  - Player spawning
     *  - Level/camera boundaries and collision
     *  - Player life/death tracking and respawn
     */
    public class SceneTransitionManager : UnitySingleton<SceneTransitionManager>
    {
        public static string LevelSelectionSceneName = "EditorSelection";
        public static string TestLevelSceneName = "EditorTest";
        public static string CreateLevelSceneName = "EditorCreate";

        public static Action OnLevelRestart;

        [field: SerializeField, Tooltip("Restart input map")] private InputValue RestartInput { get; set; }
        [field: SerializeField, Tooltip("Active scene reloads after this time")] private float TransitionTime { get; set; }

        bool restartInitiated;

        private void Start() => DontDestroyOnLoad(this);

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
            if (RestartInput.WasPressed)
                RestartLevel();
        }

        public void TransitionedRestart()
        {
            if (restartInitiated)
                return;

            restartInitiated = true;
            OnLevelRestart?.Invoke();

            //TODO: Play sfx/jingles, transitiion animations, etc.
            //TODO: Stop time?
            Invoke(nameof(RestartLevel), TransitionTime);
        }

        public void GoToScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void RestartLevel()
        {
            GoToScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        private void OnDeath(object sender, System.EventArgs e) => TransitionedRestart();
    }
}
