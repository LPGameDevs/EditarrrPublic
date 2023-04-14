using UnityEngine;

namespace Editarrr.Misc
{
    /// <summary>
    /// This attribute should be used to attach some text to a class in C#.
    /// </summary>
    /// <remarks>
    /// This attribute inherits from PropertyAttribute and can be applied to fields.
    /// It allows for attaching a string value to provide additional information about the class.
    /// It needs to be attached to the first field in the class.
    /// </remarks>
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    sealed class InfoAttribute : PropertyAttribute
    {
        // The text to be attached to the class.
        string infoText = null;
        public string InfoText { get => this.infoText; private set => this.infoText = value; }

        /// <summary>
        /// Initializes a new instance of the InfoAttribute class with the specified info text.
        /// </summary>
        /// <param name="text">The text to be attached to the class (field).</param>
        public InfoAttribute(string text)
        {
            this.InfoText = text;
        }

    }
}
