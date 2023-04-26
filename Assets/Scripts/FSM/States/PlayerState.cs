using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // ����(�̵�, ����) ���¿� ������ �� ������ ����
            player.SetMoveType(0);
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            //�̵�, ���� ���¿��� �����ϴ� ����, �����ϴ� �����̴�

            // ��ų ����Ʈ�� ��ȸ�ϸ� ��ٿ��� ���� ��ų�� ���
            foreach (SkillBase skill in player.skills)
            {
                if (!skill.IsOnCooldown())
                {
                    player.UseSkill(player.skills.IndexOf(skill));
                }
            }

            player.SetMoveType(player.GetMyBodyMoveType());

        }
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // ���¿��� ���� �� ������ ����
        }
    }
}
public class PlayerSkillUseState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // �ʻ�� ����
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            //�ʻ�� ������
        }
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // �ʻ�� ������ ����
        }
    }
}
public class PlayerDeadState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // ��� ���¿� ������ �� ������ ����
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
        }
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // ��� ���¿��� ���� �� ������ ����
        }
    }
}