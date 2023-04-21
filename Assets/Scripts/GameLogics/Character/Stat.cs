using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatName
{
    BaseHealth,
    ProjectileCount,
    ProjectileDamage,
    ProjectileSpeed,
    ChainCount,
    BlastRadius,
    None
}

public class Stat
{
    public int value;
    public float multiply;

    public Stat(int value, float multiply)
    {
        this.value = value;
        this.multiply = multiply;
    }
}
