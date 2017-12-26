using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {
    internal static class AttributeDrawerHelpers {

        internal static void MessageDrawerOnGUI(string message, Rect position, SerializedProperty property, GUIContent label,
             Func<SerializedProperty, bool> valueCheckFunc, SerializedPropertyType type, string valueErrorMessage) {
            var propertyRect = position;
            float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
            propertyRect.height = propertyHeight;
            EditorGUI.PropertyField(propertyRect, property, label, true);
            var warningRect = new Rect(position);
            warningRect.yMin += propertyHeight + 2f;
            warningRect.xMin += 12f;
            if (property.propertyType != type) {
                EditorGUI.HelpBox(warningRect, $"Only applicable to {ObjectNames.NicifyVariableName(type.ToString())}", MessageType.Error);
            }

            if (!valueCheckFunc(property)) {
                string text = valueErrorMessage;
                if (!string.IsNullOrWhiteSpace(message)) {
                    text = message;
                }
                EditorGUI.HelpBox(warningRect, text, MessageType.Error);
            }
        }

        internal static void ValueRestrictionDrawerOnGUI(Rect position, SerializedProperty property, GUIContent label,
            Func<int, int> intModifier, Func<float,float> floatModifier) {
            switch (property.propertyType) {
                case SerializedPropertyType.Integer:
                    EditorGUI.PropertyField(position, property, label, true);
                    var intValue = property.intValue;
                    intValue = intModifier(intValue);
                    property.intValue = intValue;
                    break;

                case SerializedPropertyType.Float:
                    EditorGUI.PropertyField(position, property, label, true);
                    var floatValue = property.floatValue;
                    floatValue = floatModifier(floatValue);
                    property.floatValue = floatValue;
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
