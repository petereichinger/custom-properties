using UnityEditor;
using UnityEngine;

namespace UnityExtensions.CustomEditors {

    /// <summary>Class that contains the preferences for the extensions.</summary>
    public static class PreferenceMenu {
        private const string UNITY_EX_REORDERABLE_LIST = "UNITY-EX-REORDERABLE-LIST";

        // Have we loaded the prefs yet
        private static bool _prefsLoaded;

        private static bool _useReorderableList;

        /// <summary>Toggle to enable the use of reorderable lists in the enhanced editor.</summary>
        public static bool UseReorderableList {
            get {
                LoadPrefs();
                return _useReorderableList;
            }
        }

        /// <summary>Method to show the preferences for the extensions. This is called by Unity.</summary>
        [PreferenceItem("Extensions")]
        public static void PreferencesGUI() {
            // Load the preferences
            LoadPrefs();

            using (var change = new EditorGUI.ChangeCheckScope()) {
                GUILayout.Label("Enhanced Editor", EditorStyles.boldLabel);
                // Preferences GUI
                _useReorderableList = EditorGUILayout.Toggle("Use reorderable list for arrays", _useReorderableList);

                // Save the preferences
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
