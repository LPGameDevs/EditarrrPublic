using UnityEngine.InputSystem;

namespace Editarrr.Input
{
    public class GameInputSystem : InputComponent
    {
        protected override IInputActionCollection2 GetInputAssetObject()
        {
            return new GameInput();
        }
    }
}
