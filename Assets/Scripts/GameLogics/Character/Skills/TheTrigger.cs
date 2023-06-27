using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//public class TheTriggerInfo
//{
//    public List<long> keywords= new List<long>();
//    public 
//}
[Serializable]
public class TheTrigger : SkillBase
{
    //public
    /// <summary>
    /// Ʈ���� 1~99������ Ȯ�� �ߵ� ������ �Ҵ�� ������ : 100�ۼ�Ʈ�� ���� ������ �Ⱦ��� �Ǳ� ����
    /// </summary>
    public List<long> UserTrigger = new List<long>(); //��ų �����
    public List<long> SkillTrigger = new List<long>(); //��ų
    public List<long> TargetTrigger = new List<long>(); //��ų �ǰ���
    public SkillShapeType skillShape;
    public SkillTargetPos skillTargetPos;
    [Header("Base Stat : Trigger")]
    public SkillBase linkedSkill;
    public int skillCount = 0; //��ų �ߵ� Ƚ��
    public float fireRate = 0.1f;
    //private
    private int currentSkill = 0;
    private Vector3 targetPos;
    private Vector3 startPos;
    public override void ActivateSkill(BaseController user, Vector3 triggerPos)
    {
        this.user = user;
        if (isOnCooldown || !ShouldActivateSkill()) return;
        
        //Ʈ���� ȣ��� ��ġ�� ������ġ
        startPos = triggerPos;
        //��ų ����
        switch (skillTargetPos)
        {
            case SkillTargetPos.User:
                //���� ��ġ�� Ÿ��
                targetPos = user.transform.position;
                break;
            case SkillTargetPos.CloseEnemy:
                //������ ���� Ÿ�� Ȱ��
                FindCloseEnemy();
                break;
            case SkillTargetPos.TriggerPos:
                //triggerPos�� Ȱ��
                targetPos = triggerPos;
                break;
            default:
                break;
        }

        switch (skillShape)
        {
            case SkillShapeType.ProjectileSector:
                break;

        }
        if (linkedSkill is TheProjectile)
        {
            StartCoroutine(FireProjectiles());
        }else if(linkedSkill is TheAreaOfEffect)
        {
            StartCoroutine(FireSkillTargetPos());
        }
        else if(linkedSkill is TheSummoned)
        {
            StartCoroutine(FireSkillTargetPos());
        }
        //moveDirection = (targetPos - transform.position).normalized;
        //projectileCount = projectileCount + user.GetStatValue(StatName.ProjectileCount);

        //StartCoroutine(FireProjectiles(user));
    }
    
    private IEnumerator FireProjectiles()
    {
        while (currentSkill < skillCount)
        {
            var skillObj = PoolingManager.Instance.GetObjectByID(linkedSkill.ID, startPos);
            if(skillObj == null)
            {
                yield return new WaitForSeconds(fireRate);
                continue;
            }
            //����ü�� vector3�� �������� Ȱ��
            skillObj.GetComponent<TheProjectile>().ActivateSkill(user, targetPos);


            currentSkill++;
            yield return new WaitForSeconds(fireRate);
        }

        InActiveSkill();
    }
    private IEnumerator FireSkillTargetPos()
    {
        while (currentSkill < skillCount)
        {
            //���� ��ġ�� ��ų ��ȯ
            var skillObj = PoolingManager.Instance.GetObjectByID(linkedSkill.ID, targetPos);
            if (skillObj == null)
            {
                yield return new WaitForSeconds(fireRate);
                continue;
            }

            skillObj.GetComponent<SkillBase>().ActivateSkill(user, targetPos);


            currentSkill++;
            yield return new WaitForSeconds(fireRate);
        }

        InActiveSkill();
    }

    public override void InActiveSkill()
    {
        StartCoroutine(ApplyCooldown());
    }

    private IEnumerator ApplyCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
        currentSkill = 0;
    }

    private void FindCloseEnemy()
    {
        float minDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(startPos, 3f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(startPos, collider.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    if (closestEnemy == null || closestEnemy != collider)
                    {
                        closestEnemy = collider.transform;
                    }
                }
            }
        }

        if (closestEnemy != null)
        {
            targetPos = closestEnemy.position;
        }
    }
}
