using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour, IPooledObject
{
    public long ID { get; set; }
    public bool IsPooled;
    public void OnReturnToPool()
    {
        IsPooled = true;
        gameObject.SetActive(false);
    }
}
