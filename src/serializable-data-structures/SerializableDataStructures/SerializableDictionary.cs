using System.Collections.Generic;
using UnityEngine;

namespace SerializableDataStructures {
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver {

        public Dictionary<TKey, TValue> dict;

        [SerializeField,HideInInspector]
        private List<TKey> _keys;

        [SerializeField,HideInInspector]
        private List<TValue> _values;

        public SerializableDictionary() {
            dict = new Dictionary<TKey, TValue>();
        }

        public void OnBeforeSerialize() {
            _keys = new List<TKey>();
            _values = new List<TValue>();

            foreach (var entry in dict) {
                _keys.Add(entry.Key);
                _values.Add(entry.Value);
            }
        }

        public void OnAfterDeserialize() {
            dict = new Dictionary<TKey, TValue>();
            if (_keys == null) {
                return;
            }
            for (int i = 0; i < _keys.Count; i++) {
                dict[_keys[i]] = _values[i];
            }
        }
    }
}
