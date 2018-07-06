using System;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    internal static class AttributeDrawerHelpers {
        internal static void MessageDrawerOnGUI(Rect position, SerializedProperty property, GUIContent label,
            SerializedPropertyType type, Func<SerializedProperty, bool> valuePredicate, string message) {
            var propertyRect = position;
            var propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
            propertyRect.height = propertyHeight;
            EditorGUI.PropertyField(propertyRect, property, label, true);
            var warningRect = new Rect(position);
            warningRect.yMin += propertyHeight + 2f;
            warningRect.xMin += 12f;
            if (property.propertyType != type) {
                EditorGUI.HelpBox(warningRect, $"Only applicable to {ObjectNames.NicifyVariableName(type.ToString())}",
                    MessageType.Error);
            }

            if (!valuePredicate(property)) {
                EditorGUI.HelpBox(warningRect, message, MessageType.Error);
            }
        }

        internal static void ValueRestrictionDrawerOnGUI(Rect position, SerializedProperty property, GUIContent label,
            Func<int, int> intModifier, Func<float, float> floatModifier) {
            switch (property.propertyType) {
                case SerializedPropertyType.Integer:
                    using (var check = new EditorGUI.ChangeCheckScope()) {
                        EditorGUI.PropertyField(position, property, label, true);

                        if (check.changed) {
                            var intValue = property.intValue;
                            intValue = intModifier(intValue);
                            property.intValue = intValue;
                        }
                    }

                    break;

                case SerializedPropertyType.Float:
                    using (var check = new EditorGUI.ChangeCheckScope()) {
                        EditorGUI.PropertyField(position, property, label, true);
                        if (check.changed) {
                            var floatValue = property.floatValue;
                            floatValue = floatModifier(floatValue);
                            property.floatValue = floatValue;
                        }
                    }

                    break;

                default:
                    position = EditorGUI.PrefixLabel(position, label);
                    EditorGUI.HelpBox(position, "Only applicable to float or int.", MessageType.Error);
                    break;
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}