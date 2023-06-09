using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectPoolGroup
{
    Skill = 0,
    //스킬 키워드 별 그룹아이디 1~999
    FireBall,
    Enemy = 1000,
    //적 별 그룹 아이디 1001~1999
    Scene = 2000,
    //씬 별 그룹 아이디 2001~2999
    Map = 3000,
    //맵 별 그룹 아이디 3001~3999
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
    public List<ObjectPoolItem> GetListByGroupID(ObjectPoolGroup groupID)
    {
        var list = new List<ObjectPoolItem>();

        foreach(var item in objectPoolItems)
        {
            if(item.GroupID == groupID)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public ObjectPoolItem Find(long id)
    {
        foreach(var item in objectPoolItems)
        {
            if(item.ID == id)
            {
                return item;
            }
        }
        return new ObjectPoolItem();
    }
}
