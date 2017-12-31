using UnityEngine;

namespace UnityExtensions.CustomProperties {

    /// <summary>
    /// Attribute that adds a button to execute a method. The method must not have any properties.
    /// </summary>
    public class ExecuteButtonAttribute : PropertyAttribute {

        /// <summary>Name of the method.</summary>
        public string MethodName { get; set; }
    }
}
