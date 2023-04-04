using Editarrr.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class EditorItemFrame : MonoBehaviour
    {
        protected ITileManager _trapsManager;

        protected bool _arrowsChecked;

        [SerializeField] private Image itemFrameImage;
        [SerializeField] private Image itemFrameCountWrapper;
        [SerializeField] private TextMeshProUGUI itemFrameCount;
        [SerializeField] private Button[] itemFrameArrowButtons;

        #region Input
        [field: SerializeField] private InputValue UINext { get; set; }
        #endregion

        public Color allowedColour = Color.blue;
        public Color disallowedColour = Color.red;

        protected void UpdateTrapDisplay(IFrameSelectable trap)
        {
            if (trap.getCurrentItemCount() > 0)
            {
                itemFrameCountWrapper.color = allowedColour;
            }
            else
            {
                itemFrameCountWrapper.color = disallowedColour;
            }

            if (!_arrowsChecked)
            {
                UpdateArrows();
                _arrowsChecked = true;
            }

            itemFrameImage.sprite = trap.getItemFrameImage();
            itemFrameCount.text = trap.getCurrentItemCount().ToString();
            itemFrameCount.gameObject.transform.parent.gameObject.SetActive(trap.showCount());
        }

        protected void UpdateArrows()
        {
            if (!_trapsManager.HasMultipleTraps())
            {
                UpdateArrowButtons();
            }
            else
            {
                UpdateArrowButtons(true);
            }
        }

        private void UpdateArrowButtons(bool interactable = false)
        {
            foreach (var itemFrameArrowButton in itemFrameArrowButtons)
            {
                itemFrameArrowButton.interactable = interactable;
            }
        }

        public void NextItem()
        {
            if (!_trapsManager.HasMultipleTraps())
            {
                return;
            }

            _trapsManager.NextTrap();
        }

        public void PreviousItem()
        {
            if (!_trapsManager.HasMultipleTraps())
            {
                return;
            }

            _trapsManager.PreviousTrap();
        }

        private void Awake()
        {
            _trapsManager = EditorItemManager.Instance;
        }

        private void Update()
        {
            if (UINext.WasPressed)
            {
                NextItem();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _trapsManager.SelectTrap(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _trapsManager.SelectTrap(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _trapsManager.SelectTrap(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _trapsManager.SelectTrap(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _trapsManager.SelectTrap(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                _trapsManager.SelectTrap(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                _trapsManager.SelectTrap(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                _trapsManager.SelectTrap(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                _trapsManager.SelectTrap(8);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _trapsManager.SelectTrap(9);
            }
        }

        protected void OnEnable()
        {
            EditorItemManager.OnTileSelected += UpdateTrapDisplay;
            TilePainter.OnTilePlaced += UpdateTrapDisplay;
            TilePainter.OnTileRemoved += UpdateTrapDisplay;
        }

        protected void OnDisable()
        {
            EditorItemManager.OnTileSelected -= UpdateTrapDisplay;
            TilePainter.OnTilePlaced -= UpdateTrapDisplay;
            TilePainter.OnTileRemoved -= UpdateTrapDisplay;
        }
    }
}
