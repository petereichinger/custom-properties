using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SerializableDataStructures {
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver, IDictionary<TKey, TValue> {

        private Dictionary<TKey, TValue> _dict;

        [SerializeField,HideInInspector]
        private List<TKey> _keys;

        [SerializeField,HideInInspector]
        private List<TValue> _values;

        public SerializableDictionary() {
            _dict = new Dictionary<TKey, TValue>();
        }

        public void OnBeforeSerialize() {
            _keys = new List<TKey>();
            _values = new List<TValue>();

            foreach (var entry in _dict) {
                _keys.Add(entry.Key);
                _values.Add(entry.Value);
            }
        }

        public void OnAfterDeserialize() {
            _dict = new Dictionary<TKey, TValue>();
            if (_keys == null) {
                return;
            }
            for (int i = 0; i < _keys.Count; i++) {
                _dict[_keys[i]] = _values[i];
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() { return _dict.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public void Add(KeyValuePair<TKey, TValue> item) { _dict.Add(item.Key, item.Value); }
        public void Clear() { _dict.Clear(); }
        public bool Contains(KeyValuePair<TKey, TValue> item) { return _dict.Contains(item); }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<TKey, TValue> item) { return _dict.Remove(item.Key); }
        public int Count => _dict.Count;
        public bool IsReadOnly => false;
        public void Add(TKey key, TValue value) { _dict.Add(key, value); }
        public bool ContainsKey(TKey key) { return _dict.ContainsKey(key); }
        public bool Remove(TKey key) { return _dict.Remove(key); }
        public bool TryGetValue(TKey key, out TValue value) { return _dict.TryGetValue(key, out value); }
        public TValue this[TKey key] { get { return _dict[key]; } set { _dict[key] = value; } }

        public ICollection<TKey> Keys => _dict.Keys;
        public ICollection<TValue> Values => _dict.Values;
    }
}
