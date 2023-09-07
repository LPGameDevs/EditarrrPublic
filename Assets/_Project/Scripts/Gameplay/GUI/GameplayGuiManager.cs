using System;
using Editarrr.Input;
using Editarrr.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.GUI
{
    public class GameplayGuiManager : MonoBehaviour
    {
        public static event Action<bool> OnGamePauseChanged;

        [SerializeField]
        private WinMenu _winMenu;
        [SerializeField]
        private Transform _pauseMenu;
        [SerializeField]
        private Image _overlay;

        [field: SerializeField, Tooltip("Pause input")] private InputValue PauseInput { get; set; }


        private bool _isPaused = false;

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
            SetOverlayAlpha(0.5f);
            _winMenu.gameObject.SetActive(true);
            _winMenu.Show();
            OnGamePauseChanged?.Invoke(true);
        }

        private void ShowPauseMenu()
        {
            _isPaused = true;
            OnGamePauseChanged?.Invoke(true);
            SetOverlayAlpha(0.5f);
            _pauseMenu.gameObject.SetActive(true);
        }

        private void HidePauseMenu()
        {
            _isPaused = false;
            OnGamePauseChanged?.Invoke(false);
            SetOverlayAlpha(0f);
            _pauseMenu.gameObject.SetActive(false);
        }

        private void TogglePauseMenu()
        {
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
