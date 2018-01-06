using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomEditors {  
    /// <summary>Custom editor with some enhanced functionality.</summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    internal class EnhancedTransformEditor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            var positionProperty = serializedObject.FindProperty("m_LocalPosition");
            var rotationProperty = serializedObject.FindProperty("m_LocalRotation");
            var eulerProperty = serializedObject.FindProperty("m_LocalEulerAnglesHint");
            var scaleProperty = serializedObject.FindProperty("m_LocalScale");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(positionProperty,new GUIContent("Position"), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("R",EditorStyles.miniButton,GUILayout.ExpandWidth(false))) {
                positionProperty.vector3Value = Vector3.zero;
            }
            EditorGUILayout.EndHorizontal();
            using (var changed = new EditorGUI.ChangeCheckScope()) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(eulerProperty,new GUIContent("Rotation"), GUILayout.ExpandWidth(true));
                bool reset = false;
                if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                    eulerProperty.vector3Value = Vector3.zero;
                    reset = true;
                }
                EditorGUILayout.EndHorizontal();
                if (changed.changed || reset) {
                    rotationProperty.quaternionValue = Quaternion.Euler(eulerProperty.vector3Value);
                }
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(scaleProperty, new GUIContent("Scale"), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                scaleProperty.vector3Value = Vector3.one;
            }
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();

        }
    }
}
