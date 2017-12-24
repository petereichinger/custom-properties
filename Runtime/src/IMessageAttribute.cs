using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProperties {
    /// <summary> Interface for attribute with a Message property. </summary>
    public interface IMessageAttribute {
        /// <summary>
        /// Message to display
        /// </summary>
        string Message { get; set; }
    }
}
