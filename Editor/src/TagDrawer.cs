using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {

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
            GUIStyle maskStyle = new GUIStyle(EditorStyles.popup);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            if (property.prefabOverride) {
                labelStyle.font = maskStyle.font = EditorStyles.boldFont;
            }

            using (new EditorGUI.PropertyScope(position, label, property)) {
                position = EditorGUI.PrefixLabel(position, label, labelStyle);

                property.stringValue = EditorGUI.TagField(position, GUIContent.none, property.stringValue, maskStyle);
            }
        }
    }
}
