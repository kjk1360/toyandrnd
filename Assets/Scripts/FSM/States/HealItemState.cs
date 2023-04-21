using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItemIdleState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is HealItemController healItem)
        {
            // 제자리에 대기상태 돌입
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is HealItemController healItem)
        {

        }
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is HealItemController healItem)
        {
            // 이동하거나 먹힐때 불릴듯
        }
    }
}

public class HealItemUsedState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is HealItemController healItem)
        {
            // 사용될때임
            SpawnManager.Instance.healItemObjectPool.ReturnToPool(healItem.gameObject);
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is HealItemController healItem)
        {

        }
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is HealItemController healItem)
        {
            // 이동하거나 먹힐때 불릴듯
        }
    }
}
