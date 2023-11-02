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
        public static readonly string StartSceneName = "StartScreen";
        public static readonly string LevelSelectionSceneName = "EditorSelection";
        public static readonly string TestLevelSceneName = "EditorTest";
        public static readonly string CreateLevelSceneName = "EditorCreate";
        // @todo Create the replay scene.
        public static readonly string ReplayLevelSceneName = "EditorReplay";
        public static readonly string BrowserSceneName = "LevelBrowser";

        public static Action<string> OnSceneChanged;
        public static Action<string> OnSceneAdded;
        public static Action<string> OnSceneRemoved;    

        [field: SerializeField, Tooltip("Restart input map")] private InputValue RestartInput { get; set; }
        [field: SerializeField, Tooltip("Active scene reloads after this time")] private float TransitionTime { get; set; }
        [field: SerializeField, Tooltip("Played on restart")] private AudioClip RestartSound { get; set; }
        [field: SerializeField, Tooltip("Played when changing to a different scene")] private AudioClip TransitionSound { get; set; }

        bool _restartInitiated;

        private bool StopRestartTransition { get; set; }


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
            {
                bool cancelAutoRestart = this._restartInitiated;

                this.RestartLevel();

                this.StopRestartTransition = cancelAutoRestart;
            }
        }

        public void TransitionedRestart()
        {
            if (_restartInitiated)
                return;

            _restartInitiated = true;

            Invoke(nameof(RestartLevel), TransitionTime);
        }

        public void GoToScene(string sceneName)
        {
            if (sceneName.Equals(TestLevelSceneName))
                AudioManager.Instance.FadeVolume(AudioManager.Instance.BgmSourceTwo, TransitionTime, 1f);
            else
            {
                TimeManager.Instance.StartTime();
                AudioManager.Instance.FadeVolume(AudioManager.Instance.BgmSourceTwo, TransitionTime, 0f);
            }


            AudioManager.Instance.PlayRandomizedAudioClip(TransitionSound.name, 0.1f, 0.1f);
            SceneManager.LoadScene(sceneName);
            OnSceneChanged?.Invoke(sceneName);
        }

        public void RestartLevel()
        {
            _restartInitiated = false;
            AudioManager.Instance.PlayAudioClip(RestartSound);
            GoToScene(SceneManager.GetActiveScene().name);
        }

        public void AddScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            OnSceneAdded?.Invoke(sceneName);
        }

        public void RemoveScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            OnSceneRemoved?.Invoke(sceneName);
        }

        private void OnDeath(object sender, System.EventArgs e) => TransitionedRestart();


    }
}
