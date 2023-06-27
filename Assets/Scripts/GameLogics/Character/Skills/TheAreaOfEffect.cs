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
    public float expandSpeed = 1.0f;  // �ĵ��� ���������� �ӵ�


    public List<SkillObjTrigger> skillKeywordList; //�̸� �ۼ��� Ű�����
    private HashSet<BaseController> damagedObjects = new HashSet<BaseController>();  // ������� ���� ��ü���� �����ϴ� HashSet

    private float checkInterval = 0f;
    private float nextCheckTime = 0f;
    private float curRadius = 0f;
    public List<long> myTrigger = new List<long>(); //�� �ڽ��� Ʈ���Ÿ� �����ϸ� ��
    private void FixedUpdate()
    {
        if (expandSpeed > 0f)
        {
            curRadius += expandSpeed * Time.deltaTime;
            if (Time.time >= nextCheckTime)
            {
                CheckHit(curRadius);

                // �ִ� �ݰ濡 �����ϸ� �ĵ��� �����մϴ�.
                if (curRadius >= BasicMaxRadius)
                {
                    this.enabled = false;
                    InActiveSkill();
                }
                nextCheckTime = Time.time + checkInterval;  // ���� üũ �ð� ����
            }
        }
    }

    public void CheckHit(float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);  // �߽ɺηκ��� ���� �ݰ� ���� ��� �ݶ��̴��� �˻�

        for (int i = 0; i < hitColliders.Length; i++)
        {
            var hitObject = hitColliders[i].GetComponent<BaseController>();  // 'YourDamageableComponent'�� ������� ���� �� �ִ� ������Ʈ�� Ÿ���Դϴ�.

            if (hitObject != null && !damagedObjects.Contains(hitObject))  // ��ü�� ������� ���� �� �ְ�, ���� ������� ���� �ʾҴٸ�
            {
                HitEnemy(hitObject);  // ������� �����ϴ�. 'ApplyDamage'�� ����� �����ؾ� �� �Լ��Դϴ�.
                damagedObjects.Add(hitObject);  // ������� ���� ��ü���� ��Ͽ� �߰��մϴ�.
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
        myTrigger.Add(ID); //��ų Ʈ���ſ� ��ų ���̵� ����
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
    //Hit�Ǿ�����

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
