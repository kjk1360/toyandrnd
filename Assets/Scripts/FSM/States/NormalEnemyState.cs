using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// NormalEnemy -> 추적로직
/// </summary>
public class NormalEnemyChaseState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
            // 추적 상태에 들어왔을 때 실행할 로직
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
            // 플레이어를 향해 이동
            Vector3 direction = (enemy.player.transform.position - enemy.transform.position).normalized;
            enemy.transform.position += direction * enemy.speed * Time.deltaTime;
        }
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
            // 추적 상태에서 나갈 때 실행할 로직
        }
    }
}
/// <summary>
/// NormalEnemy -> 너무 멀리 떨어졌을때 로직(보통 유저 경로로 순간이동한다)
/// </summary>
public class NormalEnemyPosChangeState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
        }
        // 대기 상태에 들어왔을 때 실행할 로직
        // 플레이어 추적 및 적절한 배치 위치로 pos move
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
        }
        // 대기 상태에서 실행할 로직
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is NormalEnemyController enemy)
        {
        }
        // 대기 상태에서 나갈 때 실행할 로직
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
        // 적이 죽은 상태에서는 업데이트 로직이 필요하지 않습니다.
    }

    public override void ExitState(BaseController controller)
    {
        // 적이 죽은 상태에서는 다른 상태로 전환되지 않습니다.
    }
}