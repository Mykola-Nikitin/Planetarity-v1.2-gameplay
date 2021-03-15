using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Utils
{
    public class SimplePool : MonoBehaviour
    {
        public static SimplePool Instance { get; private set; }

        public PoolItem[] Pools;

        private Dictionary<PoolItemType, Queue<GameObject>> _pooledObjects;

        private Action _onCompleted = delegate { };

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize(Action onCompleted)
        {
            _onCompleted   = onCompleted;
            _pooledObjects = new Dictionary<PoolItemType, Queue<GameObject>>();

            StartCoroutine(Setup());
        }

        public async Task<GameObject> Get(PoolItemType poolItemType)
        {
            if (_pooledObjects.TryGetValue(poolItemType, out Queue<GameObject> objects))
            {
                if (objects.Any())
                {
                    GameObject go = objects.Dequeue();
                    go.SetActive(true);
                    
                    return go;
                }

                PoolItem pool = Pools.FirstOrDefault(x => x.PoolItemType == poolItemType);
                AsyncOperationHandle<GameObject> operation = pool.Asset.InstantiateAsync();
                await operation.Task;

                return operation.Result;
            }

            Debug.LogError($"Couldn't find pool with type: {poolItemType}");
            return null;
        }

        public void Recycle(PoolItemType poolItemType, GameObject poolableObject)
        {
            poolableObject.SetActive(false);
            poolableObject.transform.SetParent(transform);

            if (_pooledObjects.TryGetValue(poolItemType, out Queue<GameObject> queue))
            {
                queue.Enqueue(poolableObject);
            }
            else
            {
                queue = new Queue<GameObject>();
                queue.Enqueue(poolableObject);
                
                _pooledObjects[poolItemType] = queue;
            }
        }

        private IEnumerator Setup()
        {
            for (var i = 0; i < Pools.Length; i++)
            {
                PoolItem pool = Pools[i];

                for (int j = 0; j < pool.Size; j++)
                {
                    AsyncOperationHandle<GameObject> operation = pool.Asset.InstantiateAsync();

                    yield return operation;

                    Recycle(pool.PoolItemType, operation.Result);
                }
            }

            _onCompleted();
        }
    }
}