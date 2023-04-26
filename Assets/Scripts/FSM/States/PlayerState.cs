using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // 공격(이동, 멈춤) 상태에 들어왔을 때 실행할 로직
            player.SetMoveType(0);
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            //이동, 멈춤 상태에서 실행하는 로직, 공격하는 로직이다

            // 스킬 리스트를 순회하며 쿨다운이 끝난 스킬을 사용
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
            // 상태에서 나갈 때 실행할 로직
        }
    }
}
public class PlayerSkillUseState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // 필살기 실행
        }
    }

    public override void UpdateState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            //필살기 진행중
        }
    }

    public override void ExitState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // 필살기 끝날때 로직
        }
    }
}
public class PlayerDeadState : State
{
    public override void EnterState(BaseController controller)
    {
        if (controller is PlayerController player)
        {
            // 사망 상태에 들어왔을 때 실행할 로직
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
            // 사망 상태에서 나갈 때 실행할 로직
        }
    }
}