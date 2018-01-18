using UnityEngine;

namespace UnityExtensions.CustomProperties {
    /// <summary>
    /// Attribute that shows a message when the attached property is not a prefab.
    /// </summary>
    public class MustBePrefabAttribute : PropertyAttribute {
        /// <summary>
        /// Message to show.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Create a new instance of this attribute.
        /// </summary>
        /// <param name="message">Message to show.</param>
        public MustBePrefabAttribute(string message = "Please select a prefab.") {
            Message = message;
        }
    }
}