using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// NormalEnemy -> ��������
/// </summary>
public class NormalEnemyChaseState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
            // ���� ���¿� ������ �� ������ ����
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
            // �÷��̾ ���� �̵�
            Vector3 direction = (enemy.player.transform.position - enemy.transform.position).normalized;
            enemy.transform.position += direction * enemy.speed * Time.deltaTime;
        }
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
            // ���� ���¿��� ���� �� ������ ����
        }
    }
}
/// <summary>
/// NormalEnemy -> �ʹ� �ָ� ���������� ����(���� ���� ��η� �����̵��Ѵ�)
/// </summary>
public class NormalEnemyPosChangeState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
        }
        // ��� ���¿� ������ �� ������ ����
        // �÷��̾� ���� �� ������ ��ġ ��ġ�� pos move
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
        }
        // ��� ���¿��� ������ ����
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
        }
        // ��� ���¿��� ���� �� ������ ����
    }
}
public class NormalEnemyDeadState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
            enemy.TryDropItem();
            PassiveManager.Instance.KillEnemy();
            SpawnManager.Instance.enemyObjectPool.ReturnToPool(enemy.gameObject);
            //Object.Destroy(enemy.gameObject);
        }
    }

    public override void UpdateState(BaseController controller)
    {
        // ���� ���� ���¿����� ������Ʈ ������ �ʿ����� �ʽ��ϴ�.
    }

    public override void ExitState(BaseController controller)
    {
        // ���� ���� ���¿����� �ٸ� ���·� ��ȯ���� �ʽ��ϴ�.
    }
}