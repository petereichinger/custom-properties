using UnityExtensions.CustomProperties;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {

    /// <summary>Drawer for <see cref="TagAttribute"/>.</summary>
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagDrawer : PropertyDrawer {

        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.String) {
                EditorGUI.HelpBox(position, "[Tag] only useable on string fields.", MessageType.Error);
                return;
            }

            using (var scope = new EditorGUI.PropertyScope(position, label, property)) {
                position = EditorGUI.PrefixLabel(position, scope.content);
                GUIStyle maskStyle = new GUIStyle(EditorStyles.popup);
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
