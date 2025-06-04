using UnityEngine;
using System.Collections.Generic;

namespace Pool
{
    public class ParticlePool : BehaviourSingleton<ParticlePool>
    {
        private class GameObjectPool
        {
            private Queue<GameObject> _poolQueue = new Queue<GameObject>();
            private GameObject _prefab;
            private Transform _parent;
            private int _initialSize;

            public GameObjectPool(GameObject prefab, int initialSize, Transform parent)
            {
                _prefab = prefab;
                _parent = parent;
                _initialSize = initialSize;

                for (int i = 0; i < initialSize; i++)
                {
                    GameObject obj = Object.Instantiate(prefab, parent);
                    obj.SetActive(false);
                    _poolQueue.Enqueue(obj);
                }
            }

            public GameObject Get()
            {
                if (_poolQueue.Count > 0)
                {
                    GameObject obj = _poolQueue.Dequeue();
                    obj.SetActive(true);
                    return obj;
                }
                else
                {
                    GameObject newObj = Object.Instantiate(_prefab, _parent);
                    return newObj;
                }
            }

            public void Return(GameObject obj)
            {
                obj.SetActive(false);
                _poolQueue.Enqueue(obj);
            }
        }

        private Dictionary<string, GameObjectPool> _particlePools = new Dictionary<string, GameObjectPool>();
        
        public void InitializePool(GameObject prefab, string poolName, int poolSize)
        {
            if (!_particlePools.ContainsKey(poolName))
            {
                var pool = new GameObjectPool(prefab, poolSize, transform);
                _particlePools.Add(poolName, pool);
            }
        }

        public GameObject GetParticle(string poolName)
        {
            if (_particlePools.TryGetValue(poolName, out var pool))
            {
                return pool.Get();
            }
            return null;
        }

        public void ReturnParticle(string poolName, GameObject particle)
        {
            if (_particlePools.TryGetValue(poolName, out var pool))
            {
                pool.Return(particle);
            }
        }
    }
} 