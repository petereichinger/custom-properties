using UnityEngine;

namespace CustomProperties {

    /// <summary>
    /// Attribut for fields that should be set in the editor. Only useable for object references.
    /// </summary>
    public class NotNullAttribute : PropertyAttribute, IMessageAttribute {

        /// <summary>Create a new instance of this attribute.</summary>
        /// <param name="message">Optional message to show.</param>
        public NotNullAttribute(string message = null) {
            Message = message;
        }

        /// <summary>Message to display if the field is <c>null</c>.</summary>
        public string Message { get; set; }
    }
}
