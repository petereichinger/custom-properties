using UnityEngine;

namespace UnityExtensions.CustomProperties {

    /// <summary>
    /// Attribute for <see cref="string"/> fields that shows an error message if it is empty or
    /// contains only whitespace.
    /// </summary>
    public class NotWhiteSpaceAttribute : PropertyAttribute {

        /// <summary>Message to show if the string is empty or whitespace.</summary>
        public string Message { get; set; }

        /// <summary>Create a new instance of this attribute.</summary>
        /// <param name="message">Optional message to display.</param>
        public NotWhiteSpaceAttribute(string message = null) {
            Message = message;
        }
    }
}
