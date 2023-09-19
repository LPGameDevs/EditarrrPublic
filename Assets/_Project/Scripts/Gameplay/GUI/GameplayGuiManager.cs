using Editarrr.Input;
using Editarrr.Level;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.GUI
{
    public class GameplayGuiManager : MonoBehaviour
    {
        [SerializeField] private WinMenu _winMenu;
        [SerializeField] private Transform _pauseMenu;
        [SerializeField] private Image _overlay;

        [field: SerializeField, Tooltip("Pause input")]
        private InputValue PauseInput { get; set; }


        private bool _isPaused = false;
        private bool _isWin = false;

        private void Awake()
        {
            _winMenu.gameObject.SetActive(false);
            _pauseMenu.gameObject.SetActive(false);
            SetOverlayAlpha(0);
        }

        public void SetLevelCode(string code)
        {
            _winMenu.SetCode(code);
        }

        public void SetLevelState(LevelState levelState)
        {
            _winMenu.SetLevelData(levelState);
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
            if (_isWin)
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
            // Set overlay transpacency to 0
            Color color = _overlay.color;
            color.a = alpha;
            _overlay.color = color;
        }

        private void PauseGame()
        {
            GameEvent.Trigger(GameEventType.Pause);
        }

        private void UnPauseGame()
        {
            GameEvent.Trigger(GameEventType.Unpause);
        }

        private void Update()
        {
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
