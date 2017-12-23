using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {
    /// <summary><see cref="PropertyDrawer" /> for the attribute <see cref="MaxAttribute" /></summary>
    [CustomPropertyDrawer(typeof(MaxAttribute))]
    public class MaxDrawer : PropertyDrawer {
        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var maxAttribute = (MaxAttribute) attribute;
            switch (property.propertyType) {
                case SerializedPropertyType.Integer:
                    var intValue = EditorGUI.IntField(position, label, property.intValue);
                    intValue = Mathf.Min(intValue, (int) maxAttribute.Max);
                    property.intValue = intValue;
                    break;

                case SerializedPropertyType.Float:
                    var value = EditorGUI.FloatField(position, label, property.floatValue);
                    value = Mathf.Min(value, (int) maxAttribute.Max);
                    property.floatValue = value;
                    break;

                default:
                    EditorGuiHelpers.GetLabelContentRects(position, out Rect labelRect, out Rect contentRect);
                    GUI.Label(labelRect, label);
                    EditorGUI.HelpBox(contentRect, "Only applicable to float or int.", MessageType.Error);
                    break;
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
