using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectPoolGroup
{
    Skill = 0,
    //��ų Ű���� �� �׷���̵� 1~999
    Enemy = 1000,
    //�� �� �׷� ���̵� 1001~1999
    Scene = 2000,
    //�� �� �׷� ���̵� 2001~2999
    Map = 3000,
    //�� �� �׷� ���̵� 3001~3999
}
[CreateAssetMenu(fileName = "ObjectPoolData", menuName = "ScriptableObjects/ObjectPoolData", order = 1)]
public class ObjectPoolData : ScriptableObject
{
    public List<ObjectPoolItem> objectPoolItems;

    [System.Serializable]
    public struct ObjectPoolItem
    {
        public long ID;
        public ObjectPoolGroup GroupID;
        public GameObject prefab;
        public int initialSize;
    }
}