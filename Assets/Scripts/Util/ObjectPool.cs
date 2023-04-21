using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int initialPoolSize = 10;

    private List<GameObject> pooledObjects;

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        GameObject obj = Instantiate(objectPrefab);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        IPooledObject pooledObject = obj.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnReturnToPool();
        }
    }
}
