using System.Collections;
using UnityEngine;

public class ChainShotBlast : MonoBehaviour, IPooledObject
{
    private BaseController user;
    public SpriteRenderer radius;
    private float blastRadius;
    //private LayerMask enemyLayer; ������ �±׷� ó����

    public void Initialize(BaseController user)
    {
        this.user = user;
        this.blastRadius = (0.4f + user.GetStatValue(StatName.BlastRadius)) * (1f + user.GetStatMuliply(StatName.BlastRadius));
        radius.transform.localScale = Vector3.one * blastRadius;
        //enemyLayer = LayerMask.GetMask("Enemy"); // ���� �ִ� ���̾� �̸��� �����ϼ���.
        StartCoroutine(DealDamageAndReturnToPool());
    }

    private IEnumerator DealDamageAndReturnToPool()
    {
        yield return new WaitForSeconds(0.1f); // ���� ����Ʈ�� ����Ǵ� ���� �����̸� �ݴϴ�.

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (Collider2D enemy in enemies)
        {
            // ������ ó�� ������ ���⿡ �����ϼ���.
            // ���� ���, �Ʒ��� ���� �������� ó���� �� �ֽ��ϴ�.
            // enemy.GetComponent<EnemyController>().TakeDamage(user.GetStatValue(StatName.ProjectileDamage));
            if(enemy.gameObject.CompareTag("Enemy"))
            enemy.GetComponent<BaseController>().GetHit(user.GetStatValue(StatName.ProjectileDamage) + 5);
        }

        yield return new WaitForSeconds(0.2f); // ���� ����Ʈ�� ���� ������ ��ٸ��ϴ�.

        // ������Ʈ�� Ǯ�� �ٽ� ��ȯ�մϴ�.
        SpawnManager.Instance.chainShotBlastObjectPool.ReturnToPool(gameObject);
    }

    private void OnDrawGizmos()
    {
        // ���� �ݰ��� Scene â���� Ȯ���� �� �ֵ��� ������ �ݴϴ�.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }

    void IPooledObject.OnReturnToPool()
    {
        gameObject.SetActive(false);
    }
}