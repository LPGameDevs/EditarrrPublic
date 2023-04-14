using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editarrr.Misc
{
#if UNITY_EDITOR
    /// <summary>
    /// Custom property drawer to display an InfoAttribute with a HelpBox style containing the information text.
    /// </summary>
    [CustomPropertyDrawer(typeof(InfoAttribute))]
    public class InfoDrawer : DecoratorDrawer
    {
        // Margin between the HelpBox and the property below it
        const int HeightMargin = 25;

        // Minimum height needed to display the HelpBox with the icon
        const int HeightMin = 42;

        // Get the InfoAttribute associated with the property being drawn
        InfoAttribute InfoAttribute { get { return (InfoAttribute)attribute; } }

        /// <summary>
        /// Override of the GetHeight method to calculate the required height of the HelpBox.
        /// </summary>
        /// <returns>The height needed to display the HelpBox with the icon and text.</returns>
        public override float GetHeight()
        {
            // Get the Unity HelpBox style and calculate the required height
            GUIStyle style = GUI.skin.GetStyle("helpbox");
            float height = style.CalcHeight(new GUIContent(this.InfoAttribute.InfoText), EditorGUIUtility.currentViewWidth);

            // Add the required margin on both sides
            height += InfoDrawer.HeightMargin * 2;

            // Return the greater value of the calculated height and the minimum required height
            return Mathf.Max(height, InfoDrawer.HeightMin);
        }

        /// <summary>
        /// Override of the OnGUI method to draw the HelpBox with the icon and text.
        /// </summary>
        /// <param name="position">The position and size of the property being drawn.</param>
        public override void OnGUI(Rect position)
        {
            // Adjust the position to account for the margin
            Rect panelPosition = position;
            panelPosition.height -= InfoDrawer.HeightMargin;

            // Draw the HelpBox with the icon and text
            EditorGUI.HelpBox(panelPosition, InfoAttribute.InfoText, MessageType.Info);

            // Move the position down to avoid overlapping with the HelpBox
            position.y += panelPosition.height + InfoDrawer.HeightMargin;
        }
    }
#endif
}
