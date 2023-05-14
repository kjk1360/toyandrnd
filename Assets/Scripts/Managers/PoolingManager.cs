using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager _instance;
    public ObjectPoolData poolData;
    public GameObject objectPoolBase;
    private Dictionary<long, ObjectPool> poolDictionary;

    public static PoolingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolingManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("PoolingManager");
                    _instance = singleton.AddComponent<PoolingManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void Initialize(List<ObjectPoolGroup> poolingDataGroupList, System.Action<float> onProgressUpdate = null)
    {
        StartCoroutine(InitializeCoroutine(poolingDataGroupList, onProgressUpdate));
    }

    public IEnumerator InitializeCoroutine(List<ObjectPoolGroup> poolingDataGroupList, System.Action<float> onProgressUpdate)
    {
        // 기존 Initialize 메서드의 코드를 이곳으로 옮깁니다.
        if (poolDictionary == null)
        {
            poolDictionary = new Dictionary<long, ObjectPool>();
        }
        else
        {
            if (poolDictionary.Count > 0)
            {
                foreach (var pool in poolDictionary.Values)
                {
                    poolDictionary.Remove(pool.ID);
                    pool.DestroyPool();
                }
            }
        }
        poolDictionary.Clear();

        if (poolingDataGroupList == null)
        {
            yield return null;
        }
        foreach (var groupID in poolingDataGroupList)
        {
            //Queue<GameObject> objectPool = new Queue<GameObject>();
            var groupList = poolData.GetListByGroupID(groupID);

            if (groupList != null && groupList.Count > 0)
            {
                foreach (var data in groupList)
                {
                    if (data.prefab == null || poolDictionary.ContainsKey(data.ID))
                    {
                        continue;
                    }
                    var objectPool = CreateObjectPool();

                    objectPool.InitializePool(data.ID, data.GroupID, data.prefab, data.initialSize);
                    poolDictionary.Add(data.ID, objectPool);
                }
            }
            var currentIndex = poolingDataGroupList.IndexOf(groupID);
            var totalObjectPools = poolingDataGroupList.Count;
            float progress = (float)currentIndex / (float)totalObjectPools;
            onProgressUpdate?.Invoke(progress);
            yield return null;
        }
    }
    public GameObject GetObjectByID(long prefabId, Vector3 position)
    {
        if(poolDictionary == null)
        {
            //로드 아직 안된거임 리턴!
            return null;
        }

        if (!poolDictionary.ContainsKey(prefabId))
        {
            var data = poolData.Find(prefabId);
            if(data.prefab == null)
            {
                return null;
            }
            else
            {
                var objectPool = CreateObjectPool();

                objectPool.InitializePool(data.ID, data.GroupID, data.prefab, data.initialSize);
                poolDictionary.Add(data.ID, objectPool);
            }
        }

        GameObject objectToSpawn = poolDictionary[prefabId].GetPooledObject();

        objectToSpawn.SetActive(true);
        //objectToSpawn.GetComponent<PooledObject>().ID = prefabId;
        objectToSpawn.transform.position = position;

        //poolDictionary[prefabId].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    public ObjectPool CreateObjectPool()
    {
        return Instantiate(objectPoolBase, this.transform).GetComponent<ObjectPool>();
    }

    public void ReturnObject(GameObject returnedObject)
    {
        var pooledobj = returnedObject.GetComponent<PooledObject>();
        if (pooledobj != null)
        {
            if (poolDictionary.ContainsKey(pooledobj.ID))
            {
                poolDictionary[pooledobj.ID].ReturnToPool(returnedObject);
            }
        }
    }
}