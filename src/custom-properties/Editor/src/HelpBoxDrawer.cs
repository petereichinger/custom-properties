﻿using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    /// <summary>Drawer for <see cref="HelpBoxAttribute" />.</summary>
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    internal class HelpBoxDrawer : DecoratorDrawer {
        public override float GetHeight() {
            var attr = (HelpBoxAttribute) attribute;
            var width = EditorGUIUtility.currentViewWidth;
            if (attr.Indented) {
                var rect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel++;
                width = EditorGUI.IndentedRect(rect).width;
                EditorGUI.indentLevel--;
            }

            var boxHeight = EditorStyles.helpBox.CalcHeight(new GUIContent(attr.Text), width);
            boxHeight = Mathf.Max(EditorGUIUtility.singleLineHeight * 1.5f, boxHeight);
            return boxHeight;
        }

        public override void OnGUI(Rect position) {
            var attr = (HelpBoxAttribute) attribute;
            if (attr.Indented) {
                EditorGUI.indentLevel++;
                position = EditorGUI.IndentedRect(position);
                EditorGUI.indentLevel--;
            }

            EditorGUI.HelpBox(position, attr.Text, GetTypeForBox(attr));
        }

        /// <summary>
        ///     Get the <see cref="MessageType" /> that fits to the <see cref="HelpBoxAttribute" /> of <paramref name="attr" />.
        /// </summary>
        /// <param name="attr">Attribute</param>
        /// <returns>The appropriate <see cref="MessageType" />.</returns>
        private MessageType GetTypeForBox(HelpBoxAttribute attr) {
            switch (attr.Type) {
                case HelpBoxAttribute.HelpBoxType.Info:
                    return MessageType.Info;

                case HelpBoxAttribute.HelpBoxType.Warning:
                    return MessageType.Warning;

                case HelpBoxAttribute.HelpBoxType.Error:
                    return MessageType.Error;

                default:
                    return MessageType.None;
            }
        }
    }
}