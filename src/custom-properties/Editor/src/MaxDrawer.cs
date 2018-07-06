using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary><see cref="PropertyDrawer" /> for the attribute <see cref="MaxAttribute" /></summary>
    [CustomPropertyDrawer(typeof(MaxAttribute))]
    internal class MaxDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var maxAttribute = (MaxAttribute) attribute;

            if (property.hasMultipleDifferentValues) {
                EditorGUI.showMixedValue = true;
            }

            AttributeDrawerHelpers.ValueRestrictionDrawerOnGUI(position, property, label,
                i => Mathf.Min(i, (int) maxAttribute.Value), f => Mathf.Min(f, maxAttribute.Value));
            EditorGUI.showMixedValue = false;
        }
    }
}