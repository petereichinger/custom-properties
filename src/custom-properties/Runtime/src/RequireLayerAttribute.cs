using UnityEngine;

namespace UnityExtensions.CustomProperties {
    /// <summary>
    ///     Attribute to add to <see cref="LayerMask" /> fields. Will show an error message if the
    ///     layermask is left empty.
    /// </summary>
    public class RequireLayerAttribute : PropertyAttribute {
        /// <summary>Create a new instance of the attribute.</summary>
        /// <param name="message">Message to display.</param>
        public RequireLayerAttribute(string message = null) {
            Message = message;
        }

        /// <summary>Message to display. Leave <c>null</c> for a default message.</summary>
        public string Message { get; set; }
    }
}