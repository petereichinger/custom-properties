using System.Collections.Generic;
using UnityEngine;

namespace SerializableDataStructures {
    public class SerializableStack<T> :ISerializationCallbackReceiver{
        public Stack<T> stack;

        [SerializeField,HideInInspector]
        private List<T> _values;

        public SerializableStack() {
            stack = new Stack<T>();
        }

        public void OnBeforeSerialize() { _values = new List<T>(stack); }

        public void OnAfterDeserialize() {
            if (_values == null) {
                stack = new Stack<T>();
                return;
            }
            stack =  new Stack<T>(_values);
        }
    }
}