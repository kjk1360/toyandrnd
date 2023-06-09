using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChainShotProjectile : MonoBehaviour
{
    public int chainCount;
    private float blastRadius;
    private float speed;
    private int currentChain;
    private BaseController user;
    private Vector2 moveDirection = Vector2.up;
    private float lifeTime = 3f;
    private float currentLifeTime;
    private float angle;
    public GameObject projectileAni;
    private Collider2D target;
    public void Initialize(BaseController user)
    {
        this.user = user;
        this.chainCount = user.GetStatValue(StatName.ChainCount) + 2;
        //this.blastRadius = blastRadius;
        speed = 2f + user.GetStatValue(StatName.ProjectileSpeed);
        currentChain = 0;
        //rb.velocity = direction * speed;
        Bounce();
        // 발사체의 생존 시간을 초기화합니다.
        currentLifeTime = lifeTime;
        angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        projectileAni.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            PoolingManager.Instance.ReturnObject(gameObject);
        }
        MoveProjectile();
    }
    private void MoveProjectile()
    {
        //투사체 각도
        angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        projectileAni.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //투사체 이동
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Explode(collision.transform.position);
            if (currentChain < chainCount)
            {
                Bounce();
                currentChain++;
            }
            else
            {
                PoolingManager.Instance.ReturnObject(gameObject);
                //SpawnManager.Instance.chainShotObjectPool.ReturnToPool(gameObject);
            }
        }
    }

    private void Explode(Vector3 position)
    {
        // 폭발 효과와 데미지 처리 로직을 이곳에 구현하세요.
        var blast = SpawnManager.Instance.SpawnChainShotBlast(transform.position);
        blast.GetComponent<ChainShotBlast>().Initialize(user);
    }

    private void Bounce()
    {
        currentLifeTime = lifeTime;

        float minDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    if(target == null || target != collider)
                    {
                        closestEnemy = collider.transform;
                    }
                }
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.GetComponent<Collider2D>();
            moveDirection = (closestEnemy.position - transform.position).normalized;
        }
    }
}
