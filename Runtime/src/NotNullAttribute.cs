using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProperties
{
    /// <summary>
    /// Attribut for fields that should be set in the editor
    /// </summary>
    public class NotNullAttribute : UnityEngine.PropertyAttribute {

        public string Message { get; private set; }

        public NotNullAttribute(string message = null) {
            Message = message;
        }
    }
}
