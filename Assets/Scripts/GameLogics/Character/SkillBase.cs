using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillBase : MonoBehaviour
{
    /// <summary>
    /// ��� ��ų�� ��� ������ ��Ÿ���� �ִ�(�װ� 0����������)
    /// </summary>
    public float cooldownTime;
    public float fireChance = 100;
    public bool isOnCooldown = false;
    public string name;
    public long ID;
    public BaseController user;
    /// <summary>
    /// ��ų ���
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userStats"></param>
    public abstract void ActivateSkill(BaseController user, Vector3 pos);
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
    protected bool ShouldActivateSkill()
    {
        if (fireChance + user.GetStatValue(StatName.Lucky) >= 100)
        {
            return true;
        }
        else
        {
            // 0�� 100 ������ ������ ���� �����մϴ�.
            int randomChance = UnityEngine.Random.Range(0, 100);

            // fireChance�� randomChance���� ���ų� ū ��� ��ų�� �ߵ��մϴ�.
            if (fireChance >= randomChance)
            {
                return true;
            }
        }

        // ���� ��찡 �ƴ� ��� ��ų�� �ߵ����� �ʽ��ϴ�.
        return false;
    }
}
