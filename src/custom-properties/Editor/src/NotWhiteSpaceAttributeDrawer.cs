using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {

    /// <summary>Drawer for the Attribute <see cref="NotWhiteSpaceAttribute"/>.</summary>
    [CustomPropertyDrawer(typeof(NotWhiteSpaceAttribute))]
    internal class NotWhiteSpaceAttributeDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!CheckType(property) || !CheckValue(property)) {
                return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2f + 2f;
            }
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var nwsa = (NotWhiteSpaceAttribute) attribute;
            var message = string.IsNullOrEmpty(nwsa.Message) ? "Should not be empty or whitespace" : nwsa.Message;
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
