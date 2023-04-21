using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropItem
{
    GameObject itemPrefab { get; }
    float dropChance { get; }
    void TryDropItem();
}
