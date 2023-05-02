using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;
    public ObjectPoolData poolData;
    public GameObject objectPoolBase;
    private Dictionary<long, ObjectPool> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Initialize(null);
    }

    public void Initialize(List<ObjectPoolGroup> poolingDataGroupList)
    {
        if(poolDictionary == null)
        {
            poolDictionary = new Dictionary<long, ObjectPool>();
        }
        else
        {
            if(poolDictionary.Count > 0)
            {
                foreach(var pool in poolDictionary.Values)
                {
                    poolDictionary.Remove(pool.ID);
                    pool.DestroyPool();
                }
            }
        }
        poolDictionary.Clear();

        if(poolingDataGroupList == null)
        {
            return;
        }
        foreach (var groupID in poolingDataGroupList)
        {
            //Queue<GameObject> objectPool = new Queue<GameObject>();
            var groupList = poolData.GetListByGroupID(groupID);

            if(groupList != null && groupList.Count > 0)
            {
                foreach(var data in groupList)
                {
                    var objectPool = CreateObjectPool();

                    objectPool.InitializePool(data.ID, data.GroupID, data.prefab, data.initialSize);
                    poolDictionary.Add(data.ID, objectPool);
                }
            }
        }
    }
    public GameObject GetObjectByID(long prefabId, Vector3 position)
    {
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