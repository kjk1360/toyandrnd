using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    /// <summary>
    /// 모든 스킬은 사용 종료후 쿨타임이 있다(그게 0초일지언정)
    /// </summary>
    public float cooldownTime;
    public bool isOnCooldown = false;

    /// <summary>
    /// 스킬 사용
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userStats"></param>
    public abstract void ActivateSkill(BaseController user);
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
}
