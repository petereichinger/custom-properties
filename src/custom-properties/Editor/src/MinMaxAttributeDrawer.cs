using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary>Drawer for <see cref="MinMaxAttribute" />.</summary>
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    internal class MinMaxAttributeDrawer : PropertyDrawer {
        private float _lastLabelWidth;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!(property.propertyType == SerializedPropertyType.Vector2 ||
                  property.propertyType == SerializedPropertyType.Vector2Int)) {
                return EditorGUIUtility.singleLineHeight * 2;
            }

            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            switch (property.propertyType) {
                case SerializedPropertyType.Vector2:
                    ShowVector2(position, property);
                    break;

                case SerializedPropertyType.Vector2Int:
                    ShowVector2Int(position, property);
                    break;

                default:
                    ShowErrorBox(position);
                    return;
            }

            EditorGUI.EndProperty();
        }

        private void SetLabelWidth(float width) {
            _lastLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width;
        }

        private void ResetLabelWidth() {
            EditorGUIUtility.labelWidth = _lastLabelWidth;
        }

        private void ShowVector2(Rect content, SerializedProperty property) {
            var value = property.vector2Value;
            EditorGuiHelpers.SplitRectVertically(content, out var left, out var right, 0.5f);
            SetLabelWidth(30f);
            using (var check = new EditorGUI.ChangeCheckScope()) {
                var newMin = EditorGUI.FloatField(left, "Min", value.x);
                var newMax = EditorGUI.FloatField(right, "Max", value.y);

                if (check.changed) {
                    var newValue = new Vector2(newMin, newMax);
                    if (newValue.y < value.x) {
                        property.vector2Value = new Vector2(value.x, value.x);
                    } else if (newValue.x > value.y) {
                        property.vector2Value = new Vector2(value.y, value.y);
                    } else {
                        property.vector2Value = newValue;
                    }
                }
            }

            ResetLabelWidth();
        }

        private void ShowVector2Int(Rect content, SerializedProperty property) {
            var value = property.vector2IntValue;

            EditorGuiHelpers.SplitRectVertically(content, out var left, out var right, 0.5f);
            SetLabelWidth(30f);
            using (var check = new EditorGUI.ChangeCheckScope()) {
                var newMin = EditorGUI.IntField(left, "Min", value.x);
                var newMax = EditorGUI.IntField(right, "Max", value.y);
                if (check.changed) {
                    var newValue = new Vector2Int(newMin, newMax);
                    if (newValue.y < value.x) {
                        property.vector2IntValue = new Vector2Int(value.x, value.x);
                    } else if (newValue.x > value.y) {
                        property.vector2IntValue = new Vector2Int(value.y, value.y);
                    } else {
                        property.vector2Value = newValue;
                    }
                }
            }

            ResetLabelWidth();
        }

        private static void ShowErrorBox(Rect content) {
            EditorGUI.HelpBox(content, "Only use with Vector2 or Vector2Int", MessageType.Error);
        }
    }
}