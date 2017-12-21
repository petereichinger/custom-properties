using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {

    /// <summary><see cref="PropertyDrawer"/> for the attribute <see cref="MinAttribute"/></summary>
    [CustomPropertyDrawer(typeof(MinAttribute))]
    public class MinDrawer : PropertyDrawer {

        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.Float) {
                EditorGuiHelpers.GetLabelContentRects(position, out Rect labelRect, out Rect contentRect);
                GUI.Label(labelRect, label);
                EditorGUI.HelpBox(contentRect, "Only applicable to float.", MessageType.Error);
            }

            float value = EditorGUI.FloatField(position, label, property.floatValue);

            var minAttribute = (MinAttribute)attribute;
            value = Mathf.Max(value, minAttribute.Min);

            property.floatValue = value;

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
