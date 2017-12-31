using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {

    /// <summary>Drawer for <see cref="ReadonlyAttribute"/>.</summary>
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    internal class ReadonlyDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            string value;

            switch (property.propertyType) {
                case SerializedPropertyType.Integer:
                    value = property.intValue.ToString();
                    break;

                case SerializedPropertyType.Boolean:
                    value = property.boolValue.ToString();
                    break;

                case SerializedPropertyType.Float:
                    value = property.floatValue.ToString();
                    break;

                case SerializedPropertyType.String:
                    value = property.stringValue;
                    break;

                case SerializedPropertyType.Color:
                    value = property.colorValue.ToString();
                    break;

                case SerializedPropertyType.ObjectReference:
                    value = property.objectReferenceValue.name;
                    break;

                case SerializedPropertyType.Vector2:
                    value = property.vector2Value.ToString();
                    break;

                case SerializedPropertyType.Vector2Int:
                    value = property.vector2IntValue.ToString();
                    break;

                case SerializedPropertyType.Vector3:
                    value = property.vector3Value.ToString();
                    break;

                case SerializedPropertyType.Vector3Int:
                    value = property.vector3IntValue.ToString();
                    break;

                case SerializedPropertyType.Vector4:
                    value = property.vector4Value.ToString();
                    break;

                case SerializedPropertyType.Rect:
                    value = property.rectValue.ToString();
                    break;

                case SerializedPropertyType.Bounds:
                    value = property.boundsValue.ToString();
                    break;

                case SerializedPropertyType.Quaternion:
                    value = $"{property.quaternionValue}{property.quaternionValue.eulerAngles}";
                    break;

                case SerializedPropertyType.Enum:
                    value = property.enumDisplayNames[property.enumValueIndex];
                    break;

                default:
                    EditorGUI.HelpBox(position, "Generic type is not supported by readonly attribute", MessageType.Error);
                    return;
            }
            position = EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));
            EditorGUI.SelectableLabel(position, value);
        }
    }
}
