using System.Reflection;
using UnityExtensions.CustomProperties;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityExtensions.CustomProperties.Editor {

    /// <summary>Drawer for <see cref="ExecuteButtonAttribute"/>.</summary>
    [CustomPropertyDrawer(typeof(ExecuteButtonAttribute))]
    public class ExecuteButtonAttributeDrawer : PropertyDrawer {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
        private ExecuteButtonAttribute Attribute => (ExecuteButtonAttribute)attribute;

        /// <summary>Get height for the property.</summary>
        /// <param name="property">Property.</param>
        /// <param name="label">   Label.</param>
        /// <returns>The height in pixels.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
        }

        /// <summary>Method that draws the attributed property.</summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property to draw.</param>
        /// <param name="label">   The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var propRect = new Rect(position);

            propRect.yMax -= EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(propRect, property, label);

            var buttonRect = new Rect(position);
            buttonRect.yMin = buttonRect.yMax - EditorGUIUtility.singleLineHeight;

            var type = property.serializedObject.targetObject.GetType();
            var method = type.GetMethod(Attribute.MethodName, FLAGS);

            GUI.enabled = method != null;
            string text = Attribute.MethodName;

            if (method == null) {
                text = $"{Attribute.MethodName} does not exist!";
            }
            if (GUI.Button(buttonRect, text)) {
                method?.Invoke(property.serializedObject.targetObject, null);
            }
            GUI.enabled = true;
        }
    }
}
