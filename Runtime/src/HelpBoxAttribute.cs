using UnityEngine;

namespace CustomProperties {

    /// <summary>Attribute to add a help box to a field.</summary>
    public class HelpBoxAttribute : PropertyAttribute {

        /// <summary>
        /// Type of help box. This has to be added here because UnityEditor can not be added to game code.
        /// </summary>
        public enum HelpBoxType {

            /// <summary>None message.</summary>
            None,

            /// <summary>Info message.</summary>
            Info,

            /// <summary>Warning message.</summary>
            Warning,

            /// <summary>Error message.</summary>
            Error
        }

        /// <summary>The type of help box.</summary>
        public HelpBoxType Type { get; set; }

        /// <summary>The text to display.</summary>
        public string Text { get; set; }

        /// <summary>Toggle to make box indented</summary>
        public bool Indented { get; set; } = true;
    }
}
