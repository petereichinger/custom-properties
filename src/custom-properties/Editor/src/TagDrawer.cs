using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {

    /// <summary>Drawer for <see cref="TagAttribute"/>.</summary>
    [CustomPropertyDrawer(typeof(TagAttribute))]
    internal class TagDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.String) {
                EditorGUI.HelpBox(position, "[Tag] only useable on string fields.", MessageType.Error);
                return;
            }

            using (var scope = new EditorGUI.PropertyScope(position, label, property)) {
                position = EditorGUI.PrefixLabel(position, scope.content);
                var maskStyle = new GUIStyle(EditorStyles.popup);
                if (property.prefabOverride) {
                    maskStyle.font = EditorStyles.boldFont;
                }
                if (property.hasMultipleDifferentValues) {
                    EditorGUI.showMixedValue = true;
                }
                property.stringValue = EditorGUI.TagField(position, GUIContent.none, property.stringValue, maskStyle);
                EditorGUI.showMixedValue = false;
            }
        }
    }
}
