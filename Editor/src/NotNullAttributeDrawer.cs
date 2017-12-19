using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {
    [CustomPropertyDrawer(typeof(NotNullAttribute))]
    public class NotNullAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!CheckType(property) || property.objectReferenceValue == null) {
                return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2f + 2f;
            }
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var propertyRect = position;
            float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
            propertyRect.height = propertyHeight;
            EditorGUI.PropertyField(propertyRect, property, label, true);
            var warningRect = new Rect(position);
            warningRect.yMin += propertyHeight + 2f;
            warningRect.xMin += 12f;
            if (!CheckType(property)) {
                EditorGUI.HelpBox(warningRect, "Only applicable to Object References", MessageType.Error);
            }

            if (property.objectReferenceValue == null) {
                EditorGUI.HelpBox(warningRect, "Should not be empty", MessageType.Error);
            }
        }

        private bool CheckType(SerializedProperty property) {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }
    }
}
