using UnityExtensions.CustomProperties;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary>Drawer for the Attribute <see cref="NotWhiteSpaceAttribute" />.</summary>
    [CustomPropertyDrawer(typeof(NotWhiteSpaceAttribute))]
    public class NotWhiteSpaceAttributeDrawer : PropertyDrawer {
        /// <summary>Get height for the property.</summary>
        /// <param name="property">Property.</param>
        /// <param name="label">   Label.</param>
        /// <returns>The height in pixels.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!CheckType(property) || !CheckValue(property)) {
                return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2f + 2f;
            }
            return base.GetPropertyHeight(property, label);
        }

        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var nwsa = (NotWhiteSpaceAttribute) attribute;
            string message = string.IsNullOrEmpty(nwsa.Message) ? "Should not be empty or whitespace" : nwsa.Message;
            AttributeDrawerHelpers.MessageDrawerOnGUI(position, property, label, SerializedPropertyType.String, CheckValue, message);
        }

        private bool CheckType(SerializedProperty property) {
            return property.propertyType == SerializedPropertyType.String;
        }

        private bool CheckValue(SerializedProperty property) {
            return !string.IsNullOrWhiteSpace(property.stringValue);
        }
    }
}
