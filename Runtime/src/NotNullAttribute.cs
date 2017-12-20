namespace CustomProperties {

    /// <summary>
    /// Attribut for fields that should be set in the editor. Only useable for object references.
    /// </summary>
    public class NotNullAttribute : UnityEngine.PropertyAttribute {

        /// <summary>Message to display if the field is <c>null</c>.</summary>
        public string Message { get; set; }
    }
}
