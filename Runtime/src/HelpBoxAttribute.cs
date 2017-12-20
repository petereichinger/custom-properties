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

        /// <summary>Type of message box.</summary>
        public readonly HelpBoxType type;

        /// <summary>Message to display.</summary>
        public readonly string message;

        /// <summary>Height of the message box.</summary>
        public readonly int height;

        /// <summary>Create a new instance of the attribute.</summary>
        /// <param name="type">   Type of message.</param>
        /// <param name="message">Message to display.</param>
        /// <param name="height"> Height of the box in multiples of inspector lines.</param>
        public HelpBoxAttribute(HelpBoxType type, string message, int height = 2) {
            this.type = type;
            this.message = message;
            this.height = height;
        }
    }
}
