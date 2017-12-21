using UnityEngine;

namespace CustomProperties {

    /// <summary>
    /// Attribute that restricts a field to be always greater than or equal to a specified value.
    /// Only applicable to <see cref="float"/> fields.
    /// </summary>
    public class MinAttribute : PropertyAttribute {

        /// <summary>The restricted value.</summary>
        public float Min { get; set; }

        /// <summary>Create a new instance of this attribute.</summary>
        /// <param name="min">The value to restrict to.</param>
        public MinAttribute(float min) {
            Min = min;
        }
    }
}
