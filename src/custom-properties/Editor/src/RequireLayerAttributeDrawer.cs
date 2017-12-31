using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {

    /// <summary>Drawer for the Attribute <see cref="NotWhiteSpaceAttribute"/>.</summary>
    [CustomPropertyDrawer(typeof(RequireLayerAttribute))]
    internal class RequireLayerAttributeDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!CheckType(property) || !CheckValue(property)) {
                return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2f + 2f;
            }
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var nwsa = (RequireLayerAttribute) attribute;
            var message = string.IsNullOrEmpty(nwsa.Message) ? "Requires a layer" : nwsa.Message;
            AttributeDrawerHelpers.MessageDrawerOnGUI(position, property, label, SerializedPropertyType.LayerMask, CheckValue, message);
        }

        private bool CheckType(SerializedProperty property) {
            return property.propertyType == SerializedPropertyType.LayerMask;
        }

        private bool CheckValue(SerializedProperty property) {
            return property.intValue != 0;
        }
    }
}
