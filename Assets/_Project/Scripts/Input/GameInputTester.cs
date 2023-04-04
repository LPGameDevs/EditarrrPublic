using UnityEngine;

namespace Editarrr.Input
{
    public class GameInputTester : MonoBehaviour
    {
        [field: SerializeField] public InputValue[] InputValues { get; private set; }

        private void Update()
        {
            foreach (var inputValue in this.InputValues)
            {
                if (inputValue.WasPressed)
                {
                    Debug.Log($"Was Pressed: {inputValue.InputAction.name}");
                }
                else if (inputValue.WasReleased)
                {
                    Debug.Log($"Was Released: {inputValue.InputAction.name}");
                }
                else if (inputValue.IsPressed)
                {
                    Debug.Log($"Is Pressed: {inputValue.InputAction.name}");
                }
            }
        }
    }
}
