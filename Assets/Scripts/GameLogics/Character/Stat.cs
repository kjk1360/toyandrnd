using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatName
{
    BaseHealth,
    BaseDamage,
    BaseGuard,
    ProjectileCount,
    ProjectileDamage,
    ProjectileSpeed,
    ChainCount,
    BlastRadius,
    Lucky, //Çà¿î, È®·ü¹ßµ¿¿¡ Àû¿ëµÊ
    AttackSpeed,
    None
}
[Serializable]
public class Stat
{
    public StatName name;
    public long targetID;
    public int value;
    public float multiply;

    public Stat(StatName name, long id, int value, float multiply)
    {
        this.name = name;
        this.targetID = id;
        this.value = value;
        this.multiply = multiply;
    }
}
