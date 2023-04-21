using System.Collections;
using UnityEngine;

public class ChainShotBlast : MonoBehaviour, IPooledObject
{
    private BaseController user;
    public SpriteRenderer radius;
    private float blastRadius;
    //private LayerMask enemyLayer; 지금은 태그로 처리중

    public void Initialize(BaseController user)
    {
        this.user = user;
        this.blastRadius = (0.4f + user.GetStatValue(StatName.BlastRadius)) * (1f + user.GetStatMuliply(StatName.BlastRadius));
        radius.transform.localScale = Vector3.one * blastRadius;
        //enemyLayer = LayerMask.GetMask("Enemy"); // 적이 있는 레이어 이름을 지정하세요.
        StartCoroutine(DealDamageAndReturnToPool());
    }

    private IEnumerator DealDamageAndReturnToPool()
    {
        yield return new WaitForSeconds(0.1f); // 폭발 이펙트가 재생되는 동안 딜레이를 줍니다.

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (Collider2D enemy in enemies)
        {
            // 데미지 처리 로직을 여기에 구현하세요.
            // 예를 들어, 아래와 같이 데미지를 처리할 수 있습니다.
            // enemy.GetComponent<EnemyController>().TakeDamage(user.GetStatValue(StatName.ProjectileDamage));
            if(enemy.gameObject.CompareTag("Enemy"))
            enemy.GetComponent<BaseController>().GetHit(user.GetStatValue(StatName.ProjectileDamage) + 5);
        }

        yield return new WaitForSeconds(0.2f); // 폭발 이펙트가 끝날 때까지 기다립니다.

        // 오브젝트를 풀에 다시 반환합니다.
        SpawnManager.Instance.chainShotBlastObjectPool.ReturnToPool(gameObject);
    }

    private void OnDrawGizmos()
    {
        // 폭발 반경을 Scene 창에서 확인할 수 있도록 도움을 줍니다.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }

    void IPooledObject.OnReturnToPool()
    {
        gameObject.SetActive(false);
    }
}