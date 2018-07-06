
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ID = System.Int32;

namespace UnityExtensions.EasyPool {
	public class EasyPool : MonoBehaviour, ISerializationCallbackReceiver {

		private static EasyPool _instance;
		public static EasyPool Instance {
			get {
				if (!_instance) {
					_instance = FindObjectOfType<EasyPool>();
					Assert.IsNotNull(_instance, "No EasyPool in scene");
				}

				return _instance;
			}
		}

		private Dictionary<ID, Pool> _poolDict= new Dictionary<int, Pool>();

		[SerializeField, HideInInspector]
		private List<Pool> _poolSerialization;
		public void OnBeforeSerialize() {
			if (_poolDict != null) {
				_poolSerialization = new List<Pool>(_poolDict.Values);
			}
		}

		public void OnAfterDeserialize() {
			_poolDict = new Dictionary<int, Pool>();
			foreach (var pool in _poolSerialization) {
				_poolDict[pool.instanceID] = pool;
			}
		}

		public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null) {
			if (!_poolDict.ContainsKey(prefab.GetInstanceID())) {
				_poolDict[prefab.GetInstanceID()] = Pool.CreatePool(prefab, transform);
			}

			return _poolDict[prefab.GetInstanceID()].Spawn(position, rotation, parent);
		}

		public void Despawn(GameObject objectToDespawn) {
			int id = objectToDespawn.GetInstanceID();
			Pool pool = null;
			foreach (var poolEntry in _poolDict) {
				if (poolEntry.Value.IsMember(id)) {
					pool = poolEntry.Value;
				}
			}

			Assert.IsNotNull(pool, "Object that is not pooled was despawned");

			pool.Despawn(objectToDespawn);
		}
	}
}
