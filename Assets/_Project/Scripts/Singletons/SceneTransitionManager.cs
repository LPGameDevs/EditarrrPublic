using Editarrr.Audio;
using Editarrr.Input;
using Player;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Singletons
{
    /**
     * Scene Manager Singleton
     *
     * Responsible for:
     *  - Loading scenes
     *  - Player spawning
     *  - Level/camera boundaries and collision
     *  - Player life/death tracking and respawn
     */
    public class SceneTransitionManager : UnityPersistentSingleton<SceneTransitionManager>
    {
        public static string LevelSelectionSceneName = "EditorSelection";
        public static string TestLevelSceneName = "EditorTest";
        public static string CreateLevelSceneName = "EditorCreate";
        // @todo Create the replay scene.
        public static string ReplayLevelSceneName = "EditorReplay";
        public static string BrowserSceneName = "LevelBrowser";

        public static Action<string> OnSceneChange;

        [field: SerializeField, Tooltip("Restart input map")] private InputValue RestartInput { get; set; }
        [field: SerializeField, Tooltip("Active scene reloads after this time")] private float TransitionTime { get; set; }
        [field: SerializeField, Tooltip("Played on restart")] private AudioClip RestartSound { get; set; }
        [field: SerializeField, Tooltip("Played when changing to a different scene")] private AudioClip TransitionSound { get; set; }

        bool _restartInitiated;

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
            if (RestartInput == null)
                return;

            if (RestartInput.WasPressed && SceneManager.GetActiveScene().name.Equals(TestLevelSceneName))
                RestartLevel();
        }

        public void TransitionedRestart()
        {
            if (_restartInitiated)
                return;

            _restartInitiated = true;

            //TODO: Play sfx/jingles, transitiion animations, etc.
            //TODO: Stop time?
            Invoke(nameof(RestartLevel), TransitionTime);
        }

        public void GoToScene(string sceneName)
        {
            if (sceneName.Equals(TestLevelSceneName))
                AudioManager.Instance.FadeVolume(AudioManager.Instance.BgmSourceTwo, TransitionTime, 1f);
            else
                AudioManager.Instance.FadeVolume(AudioManager.Instance.BgmSourceTwo, TransitionTime, 0f);

            AudioManager.Instance.PlayAudioClip(TransitionSound);
            SceneManager.LoadScene(sceneName);
            OnSceneChange?.Invoke(sceneName);
        }

        public void RestartLevel()
        {
            _restartInitiated = false;
            AudioManager.Instance.PlayAudioClip(RestartSound);
            GoToScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        private void OnDeath(object sender, System.EventArgs e) => TransitionedRestart();
    }
}
