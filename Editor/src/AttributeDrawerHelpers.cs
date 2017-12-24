using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {
    internal class AttributeDrawerHelpers {

        internal static void MessageDrawerOnGUI(IMessageAttribute attribute, Rect position, SerializedProperty property, GUIContent label,
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
                if (!string.IsNullOrWhiteSpace(attribute.Message)) {
                    text = attribute.Message;
                }
                EditorGUI.HelpBox(warningRect, text, MessageType.Error);
            }
        }
    }
}
