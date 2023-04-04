using UnityEngine;
using UnityEngine.InputSystem;

namespace Editarrr.Input
{
    public abstract class InputComponent : MonoBehaviour
    {
        /// <summary>
        /// A reference to the input action collection associated with this component
        /// </summary>
        private IInputActionCollection2 InputActionCollection { get; set; }

        /// <summary>
        /// An array of links between input actions and input values
        /// </summary>
        [SerializeField] private InputLink[] links;
        /// <summary>
        /// Get the links array
        /// </summary>
        public InputLink[] Links { get { return links; } }

        /// <summary>
        /// A delegate that is used to update the input values associated with this component
        /// </summary>
        InputValueUpdate InputValueUpdate { get; set; }

        private void Awake()
        {
            // Get the input action collection and enable it
            this.InputActionCollection = this.GetInputAssetObject();
            this.InputActionCollection.Enable();

            // Map each input action to an input value
            foreach (var link in this.Links)
            {
                InputAction action = this.InputActionCollection.FindAction($"{link.Action.name}");

                if (link.Action.action == null)
                    throw new UnityException($"No Action for Input '{link.Action}' ({link.Action.name})");

                this.Link(link.Value, action);
            }
        }

        /// <summary>
        /// This method is called to link an input value to an input action
        /// </summary>
        private void Link(InputValue inputValue, InputAction inputAction)
        {
            InputValueUpdate inputStateUpdate = this.InputValueUpdate;

            inputValue.Link(inputAction, ref inputStateUpdate);

            this.InputValueUpdate = inputStateUpdate;
        }

        /// <summary>
        /// This method is called once per frame and updates the input values associated with this component
        /// </summary>
        private void Update()
        {
            this.InputValueUpdate?.Invoke();
        }

        /// <summary>
        /// This method must be implemented by the extending class and returns the input action collection associated with this component
        /// </summary>
        protected abstract IInputActionCollection2 GetInputAssetObject();
    }

    public delegate void InputValueUpdate();
}
