using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtensions.EnhancedEditor {

    /// <summary>Custom editor with some enhanced functionality.</summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    internal class EnhancedEditor : Editor {
        private Dictionary<string, Tuple<ReorderableList, SerializedProperty>> _listDict;

        private ReorderableList.HeaderCallbackDelegate DrawNormalHeaderCallback(string propertyName) {
            return newRect => GUI.Label(newRect, propertyName);
        }
        private ReorderableList.HeaderCallbackDelegate DrawDragHeaderCallback(string propertyName) {
            return newRect => GUI.Label(newRect, propertyName + " (drop to add)");
        }

        private void OnEnable() {
            _listDict = new Dictionary<string, Tuple<ReorderableList, SerializedProperty>>();
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            do {
                if (iterator.isArray && iterator.propertyType == SerializedPropertyType.Generic) {
                    var listProperty = serializedObject.FindProperty(iterator.name);
                    var reorderList = new ReorderableList(serializedObject, listProperty, true, true, true, true);

                    reorderList.drawElementCallback += (rect, index, active, focused) => { EditorGUI.PropertyField(rect, listProperty.GetArrayElementAtIndex(index)); };
                    reorderList.drawHeaderCallback = DrawNormalHeaderCallback(listProperty.displayName);
                    reorderList.onCanRemoveCallback += reorderableList => true;
                    reorderList.onRemoveCallback += reorderableList => { listProperty.DeleteArrayElementAtIndex(reorderableList.index); };

                    _listDict.Add(listProperty.propertyPath, Tuple.Create(reorderList, listProperty));
                }
            } while (iterator.NextVisible(false));
        }
        private bool dragging = false;
        private ReorderableList current = null;
        /// <summary>Do the inspector gui.</summary>
        public override void OnInspectorGUI() {
            
            switch (Event.current.type) {
                case EventType.DragUpdated:
                    dragging = true;
                    break;
                case EventType.DragPerform:
                case EventType.DragExited:

                    Debug.Log(Event.current.type);
                    dragging = false;
                    current = null;
                    break;
            }
            serializedObject.Update();
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            bool requestRepaint = false;
            bool isInAnyRect = false;
            do {
               
                using (new EditorGUI.DisabledGroupScope(iterator.name == "m_Script")) {
                    if (iterator.propertyType == SerializedPropertyType.Generic && iterator.isArray && PreferencesMenu.UseReorderableList) {
                        var item = _listDict[iterator.propertyPath];
                        var reorderList = item.Item1;
                        var listProperty = item.Item2;
                        reorderList.drawElementCallback = (rect, index, active, focused) => {
                            var indRect = new Rect(rect);
                            indRect.xMin += 8f;
                            EditorGUI.PropertyField(indRect, listProperty.GetArrayElementAtIndex(index), true);
                        };
                        reorderList.elementHeightCallback = index => EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(index), true) + 4f;
                        var lastRect = GUILayoutUtility.GetLastRect();

                        var currentMouseY = Event.current.mousePosition.y;
                        var isInRect = lastRect.yMax <= currentMouseY && lastRect.yMax + reorderList.GetHeight() >= currentMouseY;

                        isInAnyRect |= isInRect;

                        if (dragging && isInRect && current != reorderList) {
                            //Currently dragging over list
//                            Debug.Log(iterator.displayName);
                            reorderList.drawHeaderCallback = DrawDragHeaderCallback(iterator.displayName);
                            current = reorderList;
                            requestRepaint = true;
//                            Debug.Log("IN");
                        }
                        if (!dragging || current != reorderList) {
                            reorderList.drawHeaderCallback = DrawNormalHeaderCallback(iterator.displayName);
                        }
                        reorderList.DoLayoutList();
                    } else {
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                }
            } while (iterator.NextVisible(false));


            if (Event.current.type != EventType.Layout) {
                if (!isInAnyRect && dragging && current != null) {
//                    Debug.Log("OUTSIDE");
                    current = null;
                    requestRepaint = true;
                }

                if (requestRepaint) {
//                    Debug.Log("REPAINT");
                    Repaint();
                }
            }
            serializedObject.ApplyModifiedProperties();
//            Debug.Log(Event.current.mousePosition);
        }
    }
}
