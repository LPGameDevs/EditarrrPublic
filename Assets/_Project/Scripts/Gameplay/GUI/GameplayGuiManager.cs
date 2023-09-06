using Editarrr.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.GUI
{
    public class GameplayGuiManager : MonoBehaviour
    {
        [SerializeField]
        private WinMenu _winMenu;
        [SerializeField]
        private Transform _loseMenu;
        [SerializeField]
        private Image _overlay;

        private void Awake()
        {
            _winMenu.gameObject.SetActive(false);
            _loseMenu.gameObject.SetActive(false);
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
        }

        private void ShowLoseMenu()
        {
            SetOverlayAlpha(0.5f);
            _loseMenu.gameObject.SetActive(true);
        }

        private void SetOverlayAlpha(float alpha)
        {
            // Set overlay transpacency to 0
            Color color = _overlay.color;
            color.a = alpha;
            _overlay.color = color;
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
