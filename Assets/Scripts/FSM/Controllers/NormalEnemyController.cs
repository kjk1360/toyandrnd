using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyController : BaseController, IDropItem, IPooledObject
{
    public PlayerController player;
    public float speed;
    public GameObject itemPrefab;
    public float dropChance;
    //public Rigidbody2D myBody;
    private Vector3 previousPosition;
    float horizontal = 0f;
    float vertical = 0f;
    int movetype = 0;

    GameObject IDropItem.itemPrefab => itemPrefab;
    float IDropItem.dropChance => dropChance;

    public override void InitializeController()
    {
        base.InitializeController();
        previousPosition = transform.position;
        MaxHP += PassiveManager.Instance.curLV;
        player = SpawnManager.Instance.player.GetComponent<PlayerController>();// GameObject.FindGameObjectWithTag("Player");
        ChangeState(new NormalEnemyChaseState());
    }
    public int GetMyBodyMoveType()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movementDirection = currentPosition - previousPosition;

        horizontal = movementDirection.x;
        vertical = movementDirection.y;

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            if (horizontal > 0)
            {
                // 오른쪽으로 움직임
                movetype = 2;
            }
            else
            {
                // 왼쪽으로 움직임
                movetype = 1;
            }
        }
        else
        {
            if (vertical > 0)
            {
                // 위로 움직임
                movetype = 3;
            }
            else
            {
                // 아래로 움직임
                movetype = 0;
            }
        }

        previousPosition = currentPosition;
        return movetype;
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
