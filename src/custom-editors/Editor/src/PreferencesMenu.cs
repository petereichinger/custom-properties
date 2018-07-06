using UnityEditor;
using UnityEngine;

namespace UnityExtensions.EnhancedEditor {
    internal static class PreferencesMenu {
        private const string UNITY_EX_REORDERABLE_LIST = "UNITY-EX-REORDERABLE-LIST";

        private static bool _prefsLoaded;

        private static bool _useReorderableList;

        public static bool UseReorderableList {
            get {
                LoadPrefs();
                return _useReorderableList;
            }
        }

        [PreferenceItem("Extensions")]
        public static void PreferencesGUI() {
            LoadPrefs();

            using (var change = new EditorGUI.ChangeCheckScope()) {
                GUILayout.Label("Enhanced Editor", EditorStyles.boldLabel);

                _useReorderableList = EditorGUILayout.Toggle("Use reorderable list for arrays", _useReorderableList);

                if (change.changed) {
                    WritePrefs();
                }
            }
        }

        private static void LoadPrefs() {
            if (!_prefsLoaded) {
                _useReorderableList = EditorPrefs.GetBool(UNITY_EX_REORDERABLE_LIST, false);
                _prefsLoaded = true;
            }
        }

        private static void WritePrefs() {
            EditorPrefs.SetBool(UNITY_EX_REORDERABLE_LIST, _useReorderableList);
        }
    }
}