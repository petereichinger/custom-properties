using System;
using System.Collections.Generic;
using UnityEngine;

using ID = System.Int32;

namespace UnityExtensions.EasyPool {

	[Serializable]
	public class Pool : ScriptableObject, ISerializationCallbackReceiver {
		public ID instanceID;
		public GameObject prefab;

		private Transform _inactiveContainer;

		private Stack<GameObject> _inactive;
		private HashSet<ID> _members;

		private List<ID> _membersSerializer;
		private List<GameObject> _inactiveSerializer;

		public static Pool CreatePool(GameObject poolObjectPrefab, Transform inactiveContainer) {
			var instance = CreateInstance<Pool>();

			instance.instanceID = poolObjectPrefab.GetInstanceID();
			instance.prefab = poolObjectPrefab;
			instance._inactive = new Stack<GameObject>();
			instance._members = new HashSet<int>();
			instance._inactiveContainer = inactiveContainer;

			return instance;
		}

		public void OnBeforeSerialize() {
			_inactiveSerializer = _inactive != null ? new List<GameObject>(_inactive) : new List<GameObject>();
			_membersSerializer = _members != null ? new List<ID>(_members) : new List<ID>();
		}

		public void OnAfterDeserialize() {
			_inactive = _inactiveSerializer != null
				? new Stack<GameObject>(_inactiveSerializer)
				: new Stack<GameObject>();

			_members =_membersSerializer!=null ? new HashSet<int>(_membersSerializer) : new HashSet<int>();
		}


		public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null) {
			GameObject instance;

			if (_inactive.Count == 0) {
				instance = Instantiate(prefab, parent, false);
				_members.Add(instance.GetInstanceID());
			} else {
				instance = _inactive.Pop();
				if (instance == null) {
					return Spawn(position, rotation);
				}
			}

			instance.transform.SetParent(parent, false);
			instance.transform.localPosition = position;
			instance.transform.localRotation = rotation;
			instance.SetActive(true);
			return instance;
		}

		public void Despawn(GameObject objectToDespawn) {
			objectToDespawn.SetActive(false);

			objectToDespawn.transform.SetParent(_inactiveContainer ,false);
			_inactive.Push(objectToDespawn);
		}

		public bool IsMember(GameObject instance) {
			return _members.Contains(instance.GetInstanceID());
		}

		public bool IsMember(ID id) {
			return _members.Contains(id);
		}
	}
}
