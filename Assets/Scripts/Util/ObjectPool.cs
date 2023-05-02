using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public long ID;
    public ObjectPoolGroup GroupID;
    public GameObject objectPrefab;
    public int initialPoolSize = 10;

    private List<PooledObject> pooledObjects;

    //private void Start()
    //{
    //    pooledObjects = new List<GameObject>();

    //    for (int i = 0; i < initialPoolSize; i++)
    //    {
    //        GameObject obj = Instantiate(objectPrefab);
    //        obj.SetActive(false);
    //        pooledObjects.Add(obj);
    //    }
    //}

    public void InitializePool(long id, ObjectPoolGroup groupID, GameObject prefab, int initialPoolSize)
    {
        pooledObjects = new List<PooledObject>();

        ID = id;
        GroupID = groupID;
        objectPrefab = prefab;
        this.initialPoolSize = initialPoolSize;

        gameObject.name = objectPrefab.name + "_Pool";

        for (int i = 0; i < initialPoolSize; i++)
        {
            //GameObject obj = Instantiate(objectPrefab);
            //obj.AddComponent<PooledObject>().ID = id;
            //obj.SetActive(false);
            pooledObjects.Add(CreateObject());
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].IsPooled)
            {
                pooledObjects[i].IsPooled = false;
                return pooledObjects[i].gameObject;
            }
        }

        var obj = CreateObject();
        pooledObjects.Add(obj);
        obj.IsPooled = false;
        return obj.gameObject;
    }

    public PooledObject CreateObject()
    {
        GameObject obj = Instantiate(objectPrefab, transform);
        var pooledObj = obj.AddComponent<PooledObject>();
        pooledObj.ID = ID;
        pooledObj.IsPooled = true;
        obj.SetActive(false);
        return pooledObj;
        //pooledObjects.Add(obj);
    }

    public void ReturnToPool(GameObject obj)
    {
        PooledObject pooledObject = obj.GetComponent<PooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnReturnToPool();
        }
    }

    public void DestroyPool()
    {
        foreach (PooledObject obj in pooledObjects)
        {
            // IPooledObject �������̽��� ������ ���, ���� �� ó���� �����մϴ�. ondestroy�� �ʿ��� ��쿡 �ٽ� �ּ�Ǯ��
            //IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
            //if (pooledObject != null)
            //{
            //    pooledObject.OnDestroy();
            //}

            // ���� ������Ʈ�� �����մϴ�.
            Destroy(obj);
        }

        // ������Ʈ Ǯ ���� ������Ʈ�� �����մϴ�.
        Destroy(gameObject);
    }
}
