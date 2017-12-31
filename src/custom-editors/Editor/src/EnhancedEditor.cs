using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtensions.CustomEditors {
    internal static class PropertyExtensions {
        internal static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty property) {
            property = property.Copy();
            var nextElement = property.Copy();
            bool hasNextElement = nextElement.NextVisible(false);
            if (!hasNextElement) {
                nextElement = null;
            }

            property.NextVisible(true);
            while (true) {
                if ((SerializedProperty.EqualContents(property, nextElement))) {
                    yield break;
                }

                yield return property;

                bool hasNext = property.NextVisible(false);
                if (!hasNext) {
                    break;
                }
            }
        }
    }

    /// <summary>Custom editor with some enhanced functionality.</summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    public class EnhancedEditor : Editor {

        private Dictionary<string, ReorderableList> _listDict;

   

        void OnEnable() {

            _listDict = new Dictionary<string, ReorderableList>();
            var iterator = serializedObject.GetIterator();
            
            for (var enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false) {
                if (iterator.isArray && iterator.propertyType == SerializedPropertyType.Generic) {
                    var listProperty = serializedObject.FindProperty(iterator.name);
                    var reorderList = new ReorderableList(serializedObject, listProperty, true, true, true, true);
                    reorderList.drawElementCallback += (rect, index, active, focused) => { EditorGUI.PropertyField(rect, listProperty.GetArrayElementAtIndex(index)); };
                    reorderList.drawHeaderCallback += rect => GUI.Label(rect, listProperty.displayName);
                    reorderList.onCanRemoveCallback += reorderableList => true;
                    reorderList.onRemoveCallback += reorderableList => { listProperty.DeleteArrayElementAtIndex(reorderableList.index); };
                    reorderList.drawElementCallback += (rect, index, active, focused) => {
                        var aElement = listProperty.GetArrayElementAtIndex(index);
                        foreach (var child in aElement.GetChildren()) {

                            float height = EditorGUI.GetPropertyHeight(child);
                            rect.height = height;
                            EditorGUI.PropertyField(rect, child, false);
                            rect.yMin += height;
                        }
                    };
                    reorderList.elementHeightCallback += index => {
                        var aElement = listProperty.GetArrayElementAtIndex(index);
                        float height = 0f;
                        foreach (var child in aElement.GetChildren()) { 

                            height += EditorGUI.GetPropertyHeight(child);
                        }
                        return height;
                    };
                    _listDict.Add(listProperty.propertyPath, reorderList);
                }
            }
        }
        
        /// <summary>Do the inspector gui.</summary>
        public override void OnInspectorGUI() {
            serializedObject.Update();
            var iterator = serializedObject.GetIterator();
            for (var enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false) {
                if (iterator.isArray && iterator.propertyType == SerializedPropertyType.Generic) {
                    // We have an array and we want to draw a reorderable list.
                    var list = _listDict[iterator.propertyPath];

                    list.DoLayoutList();
                } else {
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath)) {
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                }

            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
