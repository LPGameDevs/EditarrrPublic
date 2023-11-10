using UI;
using UnityEngine;

namespace Gameplay.GUI
{
    public class ExitGameButton : MonoBehaviour
    {
        [field: SerializeField] public ModalPopupConfirmation ExitModal { get; private set; }

        [SerializeField] Canvas _modalCanvas;

        public void OpenModal()
        {
            if (ExitModal is ModalPopupConfirmation confirmModal)
            {
                confirmModal.SetConfirm(ExitGame);
            }

            ExitModal.Open(_modalCanvas.transform, true);
        }

        private void ExitGame()
        {
            Debug.Log("Application quit via exit button");
            Application.Quit();
        }
    }
}
