using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    /// <summary>
    /// ��� ��ų�� ��� ������ ��Ÿ���� �ִ�(�װ� 0����������)
    /// </summary>
    public float cooldownTime;
    public bool isOnCooldown = false;

    /// <summary>
    /// ��ų ���
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userStats"></param>
    public abstract void ActivateSkill(BaseController user);
    /// <summary>
    /// ��ų ����(��Ÿ�� ȣ��)
    /// </summary>
    public abstract void InActiveSkill();
    /// <summary>
    /// ��Ÿ������ Ȯ��
    /// </summary>
    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }
}
