using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TheAreaOfEffect : SkillBase
{
    public float BasicMaxRadius = 1f;
    public int AOEBaseDamage = 0;
    public float AOEBasMultiply = 1f;
    public float expandSpeed = 1.0f;  // 파동이 퍼져나가는 속도


    public List<SkillObjTrigger> skillKeywordList; //미리 작성된 키워드들
    private HashSet<BaseController> damagedObjects = new HashSet<BaseController>();  // 대미지를 입힌 객체들을 저장하는 HashSet

    private float checkInterval = 0f;
    private float nextCheckTime = 0f;
    private float curRadius = 0f;
    public List<long> myTrigger = new List<long>(); //내 자신의 트리거만 관리하면 됨
    private void FixedUpdate()
    {
        if (expandSpeed > 0f)
        {
            curRadius += expandSpeed * Time.deltaTime;
            if (Time.time >= nextCheckTime)
            {
                CheckHit(curRadius);

                // 최대 반경에 도달하면 파동을 중지합니다.
                if (curRadius >= BasicMaxRadius)
                {
                    this.enabled = false;
                    InActiveSkill();
                }
                nextCheckTime = Time.time + checkInterval;  // 다음 체크 시간 설정
            }
        }
    }

    public void CheckHit(float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);  // 중심부로부터 현재 반경 내의 모든 콜라이더를 검색

        for (int i = 0; i < hitColliders.Length; i++)
        {
            var hitObject = hitColliders[i].GetComponent<BaseController>();  // 'YourDamageableComponent'는 대미지를 받을 수 있는 컴포넌트의 타입입니다.

            if (hitObject != null && !damagedObjects.Contains(hitObject))  // 객체가 대미지를 받을 수 있고, 아직 대미지를 받지 않았다면
            {
                HitEnemy(hitObject);  // 대미지를 입힙니다. 'ApplyDamage'는 당신이 구현해야 할 함수입니다.
                damagedObjects.Add(hitObject);  // 대미지를 입힌 객체들의 목록에 추가합니다.
            }
        }
    }
    public override void ActivateSkill(BaseController user, Vector3 targetPos)
    {
        this.user = user;
        if (isOnCooldown || !ShouldActivateSkill()) return;
        checkInterval = 0f;
        nextCheckTime = 0f;
        myTrigger.Clear();
        myTrigger.Add(ID); //스킬 트리거에 스킬 아이디 저장
        foreach (var trigger in skillKeywordList)
        {
            myTrigger.Add((long)trigger);
        }

        if(expandSpeed <= 0f)
        {
            CheckHit(BasicMaxRadius);
            InActiveSkill();
        }
    }

    public override void InActiveSkill()
    {
        myTrigger.Add((long)SkillObjTrigger.Remove);
        SkillManager.Instance.CheckTrigger(user, myTrigger, transform.position);
        myTrigger.Remove((long)SkillObjTrigger.Remove);
        PoolingManager.Instance.ReturnObject(gameObject);
    }
    //Hit되었을때

    private void HitEnemy(BaseController controller)
    {
        float damage = (
            (AOEBaseDamage +
            user.GetStatValue(StatName.BaseDamage) +
            user.GetStatValue(StatName.BaseDamage, ID)) *
            (1f + user.GetStatMuliply(StatName.BaseDamage) / 100f +
            user.GetStatMuliply(StatName.BaseDamage, ID) / 100f)) *
            AOEBasMultiply;
        //controller.GetDamageByHit((
        //    (AOEBaseDamage +
        //    user.GetStatValue(StatName.BaseDamage) +
        //    user.GetStatValue(StatName.BaseDamage, ID)) *
        //    (1f + user.GetStatMuliply(StatName.BaseDamage) / 100f +
        //    user.GetStatMuliply(StatName.BaseDamage, ID) / 100f)) *
        //    AOEBasMultiply
        //    );
        controller.GetHit(user, myTrigger, damage);
    }

}
