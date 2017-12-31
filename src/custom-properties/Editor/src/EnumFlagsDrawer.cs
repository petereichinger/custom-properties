using UnityExtensions.CustomProperties;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {

    /// <summary>Drawer for the attribute <see cref="EnumFlagsAttribute"/>.</summary>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsDrawer : PropertyDrawer {

        /// <summary>Get height for the property.</summary>
        /// <param name="property">Property.</param>
        /// <param name="label">   Label.</param>
        /// <returns>The height in pixels.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight * (property.propertyType != SerializedPropertyType.Enum ? 2 : 1);
        }

        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.Enum) {
                EditorGUI.HelpBox(position, "EnumFlags only works on enums", MessageType.Error);
                return;
            }

            GUIStyle maskStyle = new GUIStyle(EditorStyles.popup);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            if (property.prefabOverride) {
                labelStyle.font = maskStyle.font = EditorStyles.boldFont;
            }
            position = EditorGUI.PrefixLabel(position, label, labelStyle);

            if (property.hasMultipleDifferentValues) {
                EditorGUI.showMixedValue = true;
            }
            using (var check = new EditorGUI.ChangeCheckScope()) {
                int newValue = EditorGUI.MaskField(position, GUIContent.none, property.intValue, property.enumNames, maskStyle);

                if (check.changed) {
                    property.intValue = newValue;
                }
            }
            
            EditorGUI.showMixedValue = false;
        }
    }
}
