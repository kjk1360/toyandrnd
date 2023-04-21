using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyController : BaseController, IDropItem, IPooledObject
{
    public PlayerController player;
    public float speed;
    public GameObject itemPrefab;
    public float dropChance;

    GameObject IDropItem.itemPrefab => itemPrefab;
    float IDropItem.dropChance => dropChance;

    public override void InitializeController()
    {
        base.InitializeController();
        MaxHP += PassiveManager.Instance.curLV;
        player = SpawnManager.Instance.player.GetComponent<PlayerController>();// GameObject.FindGameObjectWithTag("Player");
        ChangeState(new NormalEnemyChaseState());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //임시로 적과 플레이어가 충돌하면 죽는것으로 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetHit(1);
        }
    }
    public void TryDropItem()
    {
        if (Random.value <= dropChance)
        {
            SpawnManager.Instance.SpawnItem(transform.position);
        }else if(Random.value <= dropChance)
        {
            SpawnManager.Instance.SpawnPowerItem(transform.position);
        }
    }
    public override void GetHit(float dmg)
    {
        base.GetHit(dmg);
        if(HP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        ChangeState(new NormalEnemyDeadState());
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
        InitializeController();
    }
}
