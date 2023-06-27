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
    /// 트리거 1~99까지는 확률 발동 로직에 할당된 공간임 : 100퍼센트가 없는 이유는 안쓰면 되기 때문
    /// </summary>
    public List<long> UserTrigger = new List<long>(); //스킬 사용자
    public List<long> SkillTrigger = new List<long>(); //스킬
    public List<long> TargetTrigger = new List<long>(); //스킬 피격자
    public SkillShapeType skillShape;
    public SkillTargetPos skillTargetPos;
    [Header("Base Stat : Trigger")]
    public SkillBase linkedSkill;
    public int skillCount = 0; //스킬 발동 횟수
    public float fireRate = 0.1f;
    //private
    private int currentSkill = 0;
    private Vector3 targetPos;
    private Vector3 startPos;
    public override void ActivateSkill(BaseController user, Vector3 triggerPos)
    {
        this.user = user;
        if (isOnCooldown || !ShouldActivateSkill()) return;
        
        //트리거 호출된 위치가 시작위치
        startPos = triggerPos;
        //스킬 시작
        switch (skillTargetPos)
        {
            case SkillTargetPos.User:
                //유저 위치가 타겟
                targetPos = user.transform.position;
                break;
            case SkillTargetPos.CloseEnemy:
                //근접한 적을 타겟 활용
                FindCloseEnemy();
                break;
            case SkillTargetPos.TriggerPos:
                //triggerPos를 활용
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
            //투사체는 vector3를 방향으로 활용
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
            //시작 위치에 스킬 소환
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
