using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {
    /// <summary><see cref="PropertyDrawer" /> for the attribute <see cref="MinAttribute" /></summary>
    [CustomPropertyDrawer(typeof(MinAttribute))]
    public class MinDrawer : PropertyDrawer {
        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var minAttribute = (MinAttribute) attribute;
            switch (property.propertyType) {
                case SerializedPropertyType.Integer:
                    var intValue = EditorGUI.IntField(position, label, property.intValue);
                    intValue = Mathf.Max(intValue, (int) minAttribute.Min);
                    property.intValue = intValue;
                    break;

                case SerializedPropertyType.Float:
                    var value = EditorGUI.FloatField(position, label, property.floatValue);
                    value = Mathf.Max(value, minAttribute.Min);
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
