using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillBase : MonoBehaviour
{
    /// <summary>
    /// 모든 스킬은 사용 종료후 쿨타임이 있다(그게 0초일지언정)
    /// </summary>
    public float cooldownTime;
    public float fireChance = 100;
    public bool isOnCooldown = false;
    public string name;
    public long ID;
    public BaseController user;
    /// <summary>
    /// 스킬 사용
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userStats"></param>
    public abstract void ActivateSkill(BaseController user, Vector3 pos);
    /// <summary>
    /// 스킬 종료(쿨타임 호출)
    /// </summary>
    public abstract void InActiveSkill();
    /// <summary>
    /// 쿨타임인지 확인
    /// </summary>
    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }
    protected bool ShouldActivateSkill()
    {
        if (fireChance + user.GetStatValue(StatName.Lucky) >= 100)
        {
            return true;
        }
        else
        {
            // 0과 100 사이의 무작위 수를 생성합니다.
            int randomChance = UnityEngine.Random.Range(0, 100);

            // fireChance가 randomChance보다 같거나 큰 경우 스킬을 발동합니다.
            if (fireChance >= randomChance)
            {
                return true;
            }
        }

        // 위의 경우가 아닌 경우 스킬을 발동하지 않습니다.
        return false;
    }
}
