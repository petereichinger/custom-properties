using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerializableDataStructures {
    public class SerializableHashSet<T> :ISerializationCallbackReceiver, ISet<T> {
        private HashSet<T> _set;

        [SerializeField,HideInInspector]
        private List<T> _values;

        public SerializableHashSet() {
            _set = new HashSet<T>();
        }

        public void OnBeforeSerialize() { _values = new List<T>(_set); }

        public void OnAfterDeserialize() {
            if (_values == null) {
                _set = new HashSet<T>();
                return;
            }
            Debug.Log("Deserialize Set with: " + _values.Count);
            _set =  new HashSet<T>(_values);
        }

        public IEnumerator<T> GetEnumerator() { return _set.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        void ICollection<T>.Add(T item) { _set.Add(item); }
        public void ExceptWith(IEnumerable<T> other) { _set.ExceptWith(other); }
        public void IntersectWith(IEnumerable<T> other) { _set.IntersectWith(other); }
        public bool IsProperSubsetOf(IEnumerable<T> other) { return _set.IsProperSubsetOf(other); }
        public bool IsProperSupersetOf(IEnumerable<T> other) { return _set.IsProperSupersetOf(other); }
        public bool IsSubsetOf(IEnumerable<T> other) { return _set.IsSubsetOf(other); }
        public bool IsSupersetOf(IEnumerable<T> other) { return _set.IsSupersetOf(other); }
        public bool Overlaps(IEnumerable<T> other) { return _set.Overlaps(other); }
        public bool SetEquals(IEnumerable<T> other) { return _set.SetEquals(other); }
        public void SymmetricExceptWith(IEnumerable<T> other) { _set.SymmetricExceptWith(other); }
        public void UnionWith(IEnumerable<T> other) { _set.UnionWith(other); }
        bool ISet<T>.Add(T item) { return _set.Add(item); }
        public void Clear() { _set.Clear(); }
        public bool Contains(T item) { return _set.Contains(item);}
        public void CopyTo(T[] array, int arrayIndex) { _set.CopyTo(array,arrayIndex); }
        public bool Remove(T item) { return _set.Remove(item); }
        public int Count => _set.Count;
        public bool IsReadOnly => false;
    }
}