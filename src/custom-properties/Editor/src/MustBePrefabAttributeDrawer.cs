using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {
    [CustomPropertyDrawer(typeof(MustBePrefabAttribute))]
    internal class MustBePrefabAttributeDrawer : PropertyDrawer {
        private float BoxXOffset => EditorGUIUtility.labelWidth;
        private float BoxWidth => EditorGUIUtility.fieldWidth;
        private MustBePrefabAttribute Attribute => (MustBePrefabAttribute) attribute;

        private string GetMessage(SerializedProperty property) {
            if (property.propertyType != SerializedPropertyType.ObjectReference) {
                return "Only valid for object references";
            }

            return Attribute.Message;
        }

        private GUIContent GetGUIContent(SerializedProperty property) {
            var guiContent = new GUIContent(GetMessage(property));
            return guiContent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var height = base.GetPropertyHeight(property, label);
            if (!IsPrefab(property)) {
                height += EditorStyles.helpBox.CalcHeight(GetGUIContent(property), BoxWidth);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var height = base.GetPropertyHeight(property, label);

            position.height = height;
            EditorGUI.PropertyField(position, property, label);
            position.yMin += height;

            if (!IsPrefab(property)) {
                var size = EditorStyles.helpBox.CalcHeight(GetGUIContent(property), BoxWidth);
                var boxRect = new Rect(position) {height = size};
                boxRect.xMin += BoxXOffset;
                EditorGUI.HelpBox(boxRect, GetMessage(property), MessageType.Error);
                position.yMin += size;
            }
        }


        private bool IsPrefab(SerializedProperty property) {
            if (property.propertyType != SerializedPropertyType.ObjectReference) {
                return false;
            }

            if (property.objectReferenceValue == null) {
                return false;
            }

            var prefabType = PrefabUtility.GetPrefabType(property.objectReferenceValue);

            return prefabType == PrefabType.Prefab || prefabType == PrefabType.ModelPrefab;
        }
    }
}