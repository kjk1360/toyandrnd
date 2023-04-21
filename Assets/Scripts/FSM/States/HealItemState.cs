using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItemIdleState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is HealItemController healItem)
        {
            // ���ڸ��� ������ ����
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
            // �̵��ϰų� ������ �Ҹ���
        }
    }
}

public class HealItemUsedState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is HealItemController healItem)
        {
            // ���ɶ���
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
            // �̵��ϰų� ������ �Ҹ���
        }
    }
}
