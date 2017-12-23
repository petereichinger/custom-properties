using UnityEngine;

namespace CustomProperties {

    /// <summary>
    /// Attribute that restricts a field to be always greater than or equal to a specified value.
    /// Only applicable to <see cref="float"/> and <see cref="int"/> fields.
    /// </summary>
    public class MinAttribute : PropertyAttribute {

        /// <summary>Create a new instance of this attribute.</summary>
        /// <param name="min">The value to restrict to.</param>
        public MinAttribute(float min) {
            Min = min;
        }

        /// <summary>The restricted value.</summary>
        public float Min { get; set; }
    }
}
