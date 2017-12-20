using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {

    /// <summary>Drawer for <see cref="HelpBoxAttribute"/>.</summary>
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : PropertyDrawer {
        private const float SPACER = 4f;

        /// <summary>Get height for the property.</summary>
        /// <param name="property">Property.</param>
        /// <param name="label">   Label.</param>
        /// <returns>The height in pixels.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float baseHeight = base.GetPropertyHeight(property, label);
            var helpBoxAttribute = attribute as HelpBoxAttribute;
            float boxHeight = 0f;
            if (helpBoxAttribute != null) {
                boxHeight = helpBoxAttribute.height * EditorGUIUtility.singleLineHeight;
            }
            return baseHeight + boxHeight + SPACER;
        }

        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var attrib = attribute as HelpBoxAttribute;
            if (attrib == null) {
                return;
            }

            var helpRect = new Rect(position);
            var propertyHeight = base.GetPropertyHeight(property, label);
            helpRect.height -= propertyHeight + SPACER;

            EditorGUI.HelpBox(helpRect, attrib.message, GetTypeForAttrib(attrib));

            var propertyRect = new Rect(position);
            propertyRect.yMin = propertyRect.yMax - propertyHeight;
            EditorGUI.PropertyField(propertyRect, property, label);
        }

        private MessageType GetTypeForAttrib(HelpBoxAttribute attrib) {
            switch (attrib.type) {
                case HelpBoxAttribute.HelpBoxType.Info:
                    return MessageType.Info;

                case HelpBoxAttribute.HelpBoxType.Warning:
                    return MessageType.Warning;

                case HelpBoxAttribute.HelpBoxType.Error:
                    return MessageType.Error;

                default:
                    return MessageType.None;
            }
        }
    }
}
