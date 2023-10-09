using System;
using Editarrr.UI;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "NewConfirmModalPopup", menuName = "Modals/new Confirm Modal Popup")]
    public class ModalPopupConfirmation : ModalPopup
    {
        private Action _onConfirm;

        [SerializeField] protected string ConfirmText;

        public void Confirm()
        {
            this._onConfirm?.Invoke();
        }

        public void SetConfirm(Action uploadLevel)
        {
            this._onConfirm = uploadLevel;
        }

        public virtual string GetConfirmText()
        {
            return ConfirmText;
        }
    }
}
