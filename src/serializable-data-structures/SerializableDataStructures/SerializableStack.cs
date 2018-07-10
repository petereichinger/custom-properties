using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerializableDataStructures {
    public class SerializableStack<T> :ISerializationCallbackReceiver {
        private Stack<T> _stack;

        [SerializeField,HideInInspector]
        private List<T> _values;

        public SerializableStack() {
            _stack = new Stack<T>();
        }

        public void OnBeforeSerialize() { _values = new List<T>(_stack); }

        public void OnAfterDeserialize() {
            if (_values == null) {
                _stack = new Stack<T>();
                return;
            }
            _stack =  new Stack<T>(_values);
        }

        public void Push(T item) { _stack.Push(item); }

        public T Pop() { return _stack.Pop(); }

        public int Count => _stack.Count;

        public bool Contains(T item) { return _stack.Contains(item); }
    }
}