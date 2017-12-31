using UnityExtensions.CustomProperties;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary><see cref="PropertyDrawer" /> for the attribute <see cref="MaxAttribute" /></summary>
    [CustomPropertyDrawer(typeof(MaxAttribute))]
    public class MaxDrawer : PropertyDrawer {
        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var maxAttribute = (MaxAttribute) attribute;

            if (property.hasMultipleDifferentValues) {
                EditorGUI.showMixedValue = true;
            }
            AttributeDrawerHelpers.ValueRestrictionDrawerOnGUI(position, property, label, i => Mathf.Min(i, (int) maxAttribute.Value), f => Mathf.Min(f, maxAttribute.Value));
            EditorGUI.showMixedValue = false;
        }
    }
}
