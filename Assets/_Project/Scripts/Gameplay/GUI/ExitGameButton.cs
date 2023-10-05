using UnityEngine;

namespace Gameplay.GUI
{
    public class ExitGameButton : MonoBehaviour
    {
        public void OnButtonPressed()
        {
            Application.Quit();
        }
    }
}
