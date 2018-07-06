using System.Collections.Generic;
using UnityEngine;

namespace SerializableDataStructures {
    public class SerializableHashSet<T> :ISerializationCallbackReceiver{
        public HashSet<T> set;

        [SerializeField,HideInInspector]
        private List<T> _values;

        public SerializableHashSet() {
            set = new HashSet<T>();
        }

        public void OnBeforeSerialize() { _values = new List<T>(set); }

        public void OnAfterDeserialize() {
            if (_values == null) {
                set = new HashSet<T>();
                return;
            }
            Debug.Log("Deserialize Set with: " + _values.Count);
            set =  new HashSet<T>(_values);
        }
    }
}