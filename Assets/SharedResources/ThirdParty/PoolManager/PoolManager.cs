#region Using Statements
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#endregion
    public class PoolManager : MonoBehaviour
    {
        #region Fields
        private static PoolManager s_instance;
        public static PoolManager Instance => s_instance;

        public List<PrefabPool> PrefabPoolCollection;

        private readonly Dictionary<string, PrefabPool> _pools = new Dictionary<string, PrefabPool>();

        #endregion

        #region Methods

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;
            InitializePrefabPools();
        }

        private void InitializePrefabPools()
        {
            if (PrefabPoolCollection == null)
                return;

            _pools.Clear();
            foreach (var pp in PrefabPoolCollection.Where(pp => pp != null && pp.Prefab != null))
            {
                pp.Awake();
                _pools.Add(pp.Prefab.PrefabName, pp);
            }
        }

        public static bool PoolExists(string name) => Instance._pools.ContainsKey(name);

        public static bool PoolExists(GameObject go)
        {
            var po = go.GetComponent<PoolObject>();
            return po != null && Instance._pools.ContainsKey(po.PrefabName);
        }

        public static GameObject Spawn(string name)
        {
            return Instance._pools.TryGetValue(name, out var pp) ? pp.Spawn() : null;
        }

        public static GameObject Spawn(GameObject go)
        {
            var po = go.GetComponent<PoolObject>();
            return po != null && Instance._pools.TryGetValue(po.PrefabName, out var pp) ? pp.Spawn() : null;
        }

        public static void Despawn(GameObject go)
        {
            if (go == null)
                return;

            var po = go.GetComponent<PoolObject>();
            if (po == null || !Instance._pools.TryGetValue(po.PrefabName, out var pp))
            {
                Destroy(go);
                return;
            }

            pp.Despawn(go);
            pp.Poll();
        }


        private void OnApplicationQuit() => _pools.Clear();

        #endregion
    }
