using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    public long ID { get; set; }
    void OnReturnToPool();
}
