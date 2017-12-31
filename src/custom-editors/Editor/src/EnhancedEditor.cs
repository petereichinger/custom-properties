using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtensions.CustomEditors {
    /// <summary>Custom editor with some enhanced functionality.</summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    public class EnhancedEditor : Editor {
        private Dictionary<string, Tuple<ReorderableList, SerializedProperty>> _listDict;

        private void OnEnable() {
            _listDict = new Dictionary<string, Tuple<ReorderableList, SerializedProperty>>();
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            do {
                if (iterator.isArray && iterator.propertyType == SerializedPropertyType.Generic) {
                    var listProperty = serializedObject.FindProperty(iterator.name);
                    var reorderList = new ReorderableList(serializedObject, listProperty, true, true, true, true);

                    reorderList.drawElementCallback += (rect, index, active, focused) => { EditorGUI.PropertyField(rect, listProperty.GetArrayElementAtIndex(index)); };
                    reorderList.drawHeaderCallback += rect => GUI.Label(rect, listProperty.displayName);
                    reorderList.onCanRemoveCallback += reorderableList => true;
                    reorderList.onRemoveCallback += reorderableList => { listProperty.DeleteArrayElementAtIndex(reorderableList.index); };
                    reorderList.drawElementCallback += (rect, index, active, focused) => {
                        var indRect = new Rect(rect);
                        var element = listProperty.GetArrayElementAtIndex(index);
                        EditorGUI.PropertyField(indRect, element, new GUIContent(element.displayName),true);
                    };
                    reorderList.elementHeightCallback += index => {
                        var aElement = listProperty.GetArrayElementAtIndex(index);
                        return EditorGUI.GetPropertyHeight(aElement, true);
                    };
                    _listDict.Add(listProperty.propertyPath, Tuple.Create(reorderList, listProperty));
                }
            } while (iterator.NextVisible(false));
        }

        /// <summary>Do the inspector gui.</summary>
        public override void OnInspectorGUI() {
            serializedObject.Update();
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            do {
                using (new EditorGUI.DisabledGroupScope(iterator.name == "m_Script")) {
                    if (iterator.propertyType == SerializedPropertyType.Generic && iterator.isArray) {
                        var item = _listDict[iterator.propertyPath];
                        var reorderList = item.Item1;
                        var listProperty = item.Item2;
                        reorderList.drawElementCallback = (rect, index, active, focused) => {
                            var indRect = new Rect(rect);
                            indRect.x += 8f;
                            EditorGUI.PropertyField(indRect, listProperty.GetArrayElementAtIndex(index), true);
                        };
                        reorderList.elementHeightCallback = index => {
                            var aElement = listProperty.GetArrayElementAtIndex(index);
                            return EditorGUI.GetPropertyHeight(aElement, true);
                        };
                        EditorGUI.indentLevel++;
                        reorderList.DoLayoutList();
                        EditorGUI.indentLevel--;
                    } else {
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                }
            } while (iterator.NextVisible(false));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
