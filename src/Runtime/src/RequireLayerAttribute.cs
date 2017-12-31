﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomProperties {
    /// <summary>
    /// Attribute to add to <see cref="LayerMask"/> fields. Will show an error message if the layermask is left empty.
    /// </summary>
    public class RequireLayerAttribute : PropertyAttribute{
        /// <summary>
        /// Message to display. Leave <c>null</c> for a default message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Create a new instance of the attribute.
        /// </summary>
        /// <param name="message">Message to display.</param>
        public RequireLayerAttribute(string message = null) {
            Message = message;
        }
    }
}