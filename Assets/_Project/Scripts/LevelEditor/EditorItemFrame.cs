using Editarrr.Input;
using System;
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
        [field: SerializeField, Header("Input")] private InputValue UINext { get; set; }
        [field: SerializeField] private InputValue SelectTile { get; set; }
        #endregion

        public Color allowedColour = Color.blue;
        public Color disallowedColour = Color.red;

        protected void UpdateTrapDisplay(IFrameSelectable trap)
        {
            if (trap.getCurrentItemCount() > 0)
            {
                this.itemFrameCountWrapper.color = this.allowedColour;
            }
            else
            {
                this.itemFrameCountWrapper.color = this.disallowedColour;
            }

            if (!this._arrowsChecked)
            {
                this.UpdateArrows();
                this._arrowsChecked = true;
            }

            this.itemFrameImage.sprite = trap.getItemFrameImage();
            this.itemFrameCount.text = trap.getCurrentItemCount().ToString();
            this.itemFrameCount.gameObject.transform.parent.gameObject.SetActive(trap.showCount());
        }

        protected void UpdateArrows()
        {
            if (!this._trapsManager.HasMultipleTraps())
            {
                this.UpdateArrowButtons();
            }
            else
            {
                this.UpdateArrowButtons(true);
            }
        }

        private void UpdateArrowButtons(bool interactable = false)
        {
            foreach (var itemFrameArrowButton in this.itemFrameArrowButtons)
            {
                itemFrameArrowButton.interactable = interactable;
            }
        }

        public void NextItem()
        {
            if (!this._trapsManager.HasMultipleTraps())
            {
                return;
            }

            this._trapsManager.NextTrap();
        }

        public void PreviousItem()
        {
            if (!this._trapsManager.HasMultipleTraps())
            {
                return;
            }

            this._trapsManager.PreviousTrap();
        }

        private void Awake()
        {
            this._trapsManager = EditorItemManager.Instance;
        }

        private void Update()
        {
            if (this.UINext.WasPressed)
            {
                this.NextItem();
            }

            if (this.SelectTile.WasPressed)
            {
                int index = (int)this.SelectTile.Read<float>();
                this._trapsManager.SelectTrap((index - 1).Loop(9));
            }


            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    this._trapsManager.SelectTrap(0);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    this._trapsManager.SelectTrap(1);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    this._trapsManager.SelectTrap(2);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    this._trapsManager.SelectTrap(3);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha5))
            //{
            //    this._trapsManager.SelectTrap(4);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha6))
            //{
            //    this._trapsManager.SelectTrap(5);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha7))
            //{
            //    this._trapsManager.SelectTrap(6);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha8))
            //{
            //    this._trapsManager.SelectTrap(7);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha9))
            //{
            //    this._trapsManager.SelectTrap(8);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha0))
            //{
            //    this._trapsManager.SelectTrap(9);
            //}
        }

        protected void OnEnable()
        {
            EditorItemManager.OnTileSelected += this.UpdateTrapDisplay;
            TilePainter.OnTilePlaced += this.UpdateTrapDisplay;
            TilePainter.OnTileRemoved += this.UpdateTrapDisplay;
        }

        protected void OnDisable()
        {
            EditorItemManager.OnTileSelected -= this.UpdateTrapDisplay;
            TilePainter.OnTilePlaced -= this.UpdateTrapDisplay;
            TilePainter.OnTileRemoved -= this.UpdateTrapDisplay;
        }
    }
}
