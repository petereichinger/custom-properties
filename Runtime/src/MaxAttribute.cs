using UnityEngine;

namespace CustomProperties {

    /// <summary>
    /// Attribute that restricts a field to be always smaller than or equal to a specified value. Only applicable
    /// to <see cref="float"/> fields.
    /// </summary>
    public class MaxAttribute : PropertyAttribute {

        /// <summary>The restricted value.</summary>
        public float Max { get; set; }

        /// <summary>Create a new instance of this attribute.</summary>
        /// <param name="max">The value to restrict to.</param>
        public MaxAttribute(float max) {
            Max = max;
        }
    }
}
