using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary>Drawer for the Attribute <see cref="NotNullAttribute" />.</summary>
    [CustomPropertyDrawer(typeof(NotNullAttribute))]
    internal class NotNullAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!CheckType(property) || !CheckValue(property)) {
                return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2f + 2f;
            }

            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var nna = (NotNullAttribute) attribute;
            var message = string.IsNullOrEmpty(nna.Message) ? "Should not be null" : nna.Message;
            AttributeDrawerHelpers.MessageDrawerOnGUI(position, property, label, SerializedPropertyType.ObjectReference,
                CheckValue, message);
        }

        private bool CheckType(SerializedProperty property) {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }

        private bool CheckValue(SerializedProperty property) {
            return property.objectReferenceValue != null;
        }
    }
}