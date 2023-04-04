using UnityEngine;

namespace Editarrr.Input
{
    /// <summary>
    /// This class extends InputValue to track how long a button has been held down and released
    /// </summary>
    [CreateAssetMenu(fileName = "Input Button", menuName = "Data/Input/new Input Button")]
    public class InputButton : InputValue
    {
        /// <summary>
        /// The amount of time the button has been held down
        /// </summary>
        public float DownTime { get; private set; }
        /// <summary>
        /// The amount of time the button has been released
        /// </summary>
        public float UpTime { get; private set; }

        /// <summary>
        /// Called when an InputAction is linked to this InputButton to register the Update() method
        /// </summary>
        protected override void OnLink(ref InputValueUpdate action)
        {
            action += this.Update;
        }

        /// <summary>
        /// Updates the DownTime and UpTime properties based on the current state of the button
        /// </summary>
        void Update()
        {
            if (this.IsPressed)
            {
                this.DownTime += Time.deltaTime;
                this.UpTime = 0;
            }
            else
            {
                this.DownTime = 0;
                this.UpTime += Time.deltaTime;
            }
        }
    }
}
