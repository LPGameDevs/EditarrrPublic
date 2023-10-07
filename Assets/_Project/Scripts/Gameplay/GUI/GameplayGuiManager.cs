using System;
using Editarrr.Input;
using Editarrr.Level;
using Singletons;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.GUI
{
    public class GameplayGuiManager : MonoBehaviour
    {
        public static event Action OnGameStarted;

        [SerializeField] private WinMenu _winMenu;
        [SerializeField] private Transform _pauseMenu;
        [SerializeField] private Image _overlay;
        [SerializeField] private GameObject _inputPrompt;

        [field: SerializeField, Tooltip("Move input map")] private InputValue MoveInput { get; set; }
        [field: SerializeField, Tooltip("Jump input map")] private InputValue JumpInput { get; set; }
        [field: SerializeField, Tooltip("Pause input")] private InputValue PauseInput { get; set; }

        private string _levelCode;
        private LevelState _levelState;
        bool _awaitingInput;

        private bool _isPaused = false;
        private bool _isWin = false;

        private void Awake()
        {
            _winMenu.gameObject.SetActive(false);
            _pauseMenu.gameObject.SetActive(false);
            _inputPrompt.SetActive(false);
            SetOverlayAlpha(0);

            ShowInputPrompt();
        }

        public void SetLevelCode(string code)
        {
            _levelCode = code;
            _winMenu.SetCode(code);
        }

        public void SetLevelState(LevelState levelState)
        {
            _levelState = levelState;
            _winMenu.SetLevelData(levelState);
        }

        private void ShowInputPrompt()
        {
            _awaitingInput = true;
            PauseGame();
            SetOverlayAlpha(0.5f);
            _inputPrompt.SetActive(true);
        }

        private void HideInputPrompt()
        {
            _awaitingInput = false;
            UnPauseGame();
            SetOverlayAlpha(0f);
            _inputPrompt.SetActive(false);
            OnGameStarted?.Invoke();
        }

        private void ShowWinMenu()
        {
            _isWin = true;
            PauseGame();
            SetOverlayAlpha(0.5f);
            _winMenu.gameObject.SetActive(true);
            _winMenu.Show();
        }

        private void ShowPauseMenu()
        {
            _isPaused = true;
            PauseGame();
            SetOverlayAlpha(0.5f);
            _pauseMenu.gameObject.SetActive(true);
        }

        private void HidePauseMenu()
        {
            _isPaused = false;
            UnPauseGame();
            SetOverlayAlpha(0f);
            _pauseMenu.gameObject.SetActive(false);
        }

        private void TogglePauseMenu()
        {
            // We dont want to unpause the game if the Win menu is active.
            if (_isWin || _awaitingInput)
            {
                return;
            }

            if (_isPaused)
            {
                HidePauseMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }

        private void SetOverlayAlpha(float alpha)
        {
            Color color = _overlay.color;
            color.a = alpha;
            _overlay.color = color;
            _overlay.enabled = alpha > 0f;
        }

        private void PauseGame()
        {
            GameEvent.Trigger(GameEventType.Pause);
        }

        private void UnPauseGame()
        {
            GameEvent.Trigger(GameEventType.Unpause);
        }

        // This is the x in the top left corner.
        public void OnQuitButtonPressed()
        {
            string goToScene = SceneTransitionManager.CreateLevelSceneName;
            if (_levelState.Published)
            {
                goToScene = SceneTransitionManager.LevelSelectionSceneName;
            }
            SceneTransitionManager.Instance.GoToScene(goToScene);
        }

        private void Update()
        {
            if(_inputPrompt.activeInHierarchy && _awaitingInput)
            {
                if(MoveInput.WasPressed || JumpInput.WasPressed)
                    HideInputPrompt();
            }
            if (PauseInput.WasPressed)
            {
                TogglePauseMenu();
            }
        }

        private void OnEnable()
        {
            Chest.OnChestOpened += ShowWinMenu;
        }

        private void OnDisable()
        {
            Chest.OnChestOpened -= ShowWinMenu;
        }
    }
}
