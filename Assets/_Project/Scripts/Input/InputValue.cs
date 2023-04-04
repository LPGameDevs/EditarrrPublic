using UnityEngine;
using UnityEngine.InputSystem;

namespace Editarrr.Input
{
    /// <summary>
    /// This class wraps an InputAction and provides properties and methods to read and handle input
    /// </summary>
    [CreateAssetMenu(fileName = "Input Value", menuName = "Data/Input/new Input Value")]
    public class InputValue : ScriptableObject
    {
        /// <summary>
        /// The InputAction being wrapped by this InputValue
        /// </summary>
        public InputAction InputAction { get; private set; }

        /// <summary>
        /// Whether the input was pressed, released, or is currently being held down
        /// </summary>
        public bool WasPressed { get => this.InputAction.WasPressedThisFrame(); }
        public bool WasReleased { get => this.InputAction.WasReleasedThisFrame(); }
        public bool IsPressed { get => this.InputAction.IsPressed(); }

        /// <summary>
        /// Links an InputAction to this InputValue and calls OnLink() method
        /// </summary>
        public void Link(InputAction inputAction, ref InputValueUpdate action)
        {
            this.InputAction = inputAction;
            this.OnLink(ref action);
        }

        /// <summary>
        /// Called when an InputAction is linked to this InputValue
        /// </summary>
        protected virtual void OnLink(ref InputValueUpdate action) { }

        /// <summary>
        /// Reads the current value of the InputAction and returns it as a generic type
        /// </summary>
        public T Read<T>()
            where T : struct
        {
            return this.InputAction.ReadValue<T>();
        }

        /// <summary>
        /// Implicitly converts this InputValue to a float
        /// </summary>
        public static implicit operator float(InputValue inputValue) => inputValue.Read<float>();

        /// <summary>
        /// Implicitly converts this InputValue to a Vector2
        /// </summary>
        public static implicit operator Vector2(InputValue inputValue) => inputValue.Read<Vector2>();
    }
}
