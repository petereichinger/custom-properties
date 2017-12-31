using UnityExtensions.CustomProperties;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary><see cref="PropertyDrawer" /> for the attribute <see cref="MinAttribute" /></summary>
    [CustomPropertyDrawer(typeof(MinAttribute))]
    public class MinDrawer : PropertyDrawer {
        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var minAttribute = (MinAttribute) attribute;

            if (property.hasMultipleDifferentValues) {
                EditorGUI.showMixedValue = true;
            }
            AttributeDrawerHelpers.ValueRestrictionDrawerOnGUI(position, property, label, i => Mathf.Max(i, (int) minAttribute.Value), f => Mathf.Max(f, minAttribute.Value));
            EditorGUI.showMixedValue = false;
        }
    }
}
