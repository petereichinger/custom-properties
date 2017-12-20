using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {
    /// <summary>
    /// Drawer for the Attribute <see cref="NotNullAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(NotNullAttribute))]
    public class NotNullAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!CheckType(property) || property.objectReferenceValue == null) {
                return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2f + 2f;
            }
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            NotNullAttribute nna = (NotNullAttribute) attribute;
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
                string text = "Should not be empty";
                if (!string.IsNullOrWhiteSpace(nna.Message)) {
                    text = nna.Message;
                }
                EditorGUI.HelpBox(warningRect, text, MessageType.Error);
            }
        }

        private bool CheckType(SerializedProperty property) {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }
    }
}
