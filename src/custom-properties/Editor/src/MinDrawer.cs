using UnityExtensions.CustomProperties;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary><see cref="PropertyDrawer" /> for the attribute <see cref="MinAttribute" /></summary>
    [CustomPropertyDrawer(typeof(MinAttribute))]
    internal class MinDrawer : PropertyDrawer {
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
