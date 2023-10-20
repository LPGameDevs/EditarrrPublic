using System;
using Singletons;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "NewConfirmModalPopup", menuName = "Modals/new Confirm Modal Popup")]
    public class ModalPopupConfirmation : ModalPopup
    {
        private Action _onConfirm;

        [SerializeField] protected string ConfirmText;

        public override void Open(Transform parent = null, bool alwaysShow = false)
        {
            if (!alwaysShow && PreferencesManager.Instance.IsModalEventTracked(this.name, ModalPopupAction.Close))
            {
                this.Confirm();
                return;
            }

            if (parent == null)
            {
                parent = FindObjectOfType<Canvas>().transform;
            }
            var popup = Instantiate(Prefab, parent);
            popup.Setup(this);

            PreferencesManager.Instance.SetModalEventTracked(this.name, ModalPopupAction.Open);
        }

        public void Confirm()
        {
            this._onConfirm?.Invoke();

            PreferencesManager.Instance.SetModalEventTracked(this.name, ModalPopupAction.Confirm);
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
