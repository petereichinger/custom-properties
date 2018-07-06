using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary>Drawer for the attribute <see cref="EnumFlagsAttribute" />.</summary>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    internal class EnumFlagsDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight * (property.propertyType != SerializedPropertyType.Enum ? 2 : 1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.Enum) {
                EditorGUI.HelpBox(position, "EnumFlags only works on enums", MessageType.Error);
                return;
            }

            var maskStyle = new GUIStyle(EditorStyles.popup);
            var labelStyle = new GUIStyle(EditorStyles.label);
            if (property.prefabOverride) {
                labelStyle.font = maskStyle.font = EditorStyles.boldFont;
            }

            position = EditorGUI.PrefixLabel(position, label, labelStyle);

            if (property.hasMultipleDifferentValues) {
                EditorGUI.showMixedValue = true;
            }

            using (var check = new EditorGUI.ChangeCheckScope()) {
                var newValue = EditorGUI.MaskField(position, GUIContent.none, property.intValue, property.enumNames,
                    maskStyle);

                if (check.changed) {
                    property.intValue = newValue;
                }
            }

            EditorGUI.showMixedValue = false;
        }
    }
}