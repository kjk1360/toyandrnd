using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TheProjectile : SkillBase
{
    //����ü�� �ʿ��� ���ݵ�
    public int projectileBounceCount; //����ü ���� �� Ƚ��
    public float projectileSpeed;  //����ü �ӵ�
    public int projectilePiercingCount; //����ü ���� �� Ƚ��
    public float lifeTime = 3f;    //����ü ���� �ð�
    public int projectileBaseDamage = 0; //����ü �⺻ ������
    public float projectileBaseMultiply = 1f;
    public float projectileAddedDamageMultiply = 0f; //����ü�� ����Ǵ� ���ݷ��� ���� (100�̸� ���ݷ��� 100�ۼ�Ʈ�� �߰�)
    public GameObject projectileAni;



    public List<SkillObjTrigger> skillKeywordList; //�̸� �ۼ��� Ű�����



    //���Ǵ� temp������
    private int currentBounce;      //����ü �� ���� Ƚ��
    private Vector2 moveDirection = Vector2.up; //����ü ����
    private float angle;            //����ü ����
    private float currentLifeTime;  //����ü �� ���� �ð�
    private int currentPiercing;      //����ü �� ���� Ƚ��




    public List<long> myTrigger = new List<long>(); //�� �ڽ��� Ʈ���Ÿ� �����ϸ� ��

    //����ü�� ������ ������ ��ġ���� targetPos�� ���ư��⸸ �ϸ� ��
    public override void ActivateSkill(BaseController user, Vector3 targetPos)
    {
        this.user = user;
        if (isOnCooldown || !ShouldActivateSkill()) return;

        moveDirection = (targetPos - transform.position).normalized; //firstdirection
        myTrigger.Clear();
        myTrigger.Add(ID); //��ų Ʈ���ſ� ��ų ���̵� ����
        foreach(var trigger in skillKeywordList)
        {
            myTrigger.Add((long)trigger);
        }
    }
    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            InActiveSkill();
        }
        MoveProjectile();
    }
    public override void InActiveSkill()
    {
        myTrigger.Add((long)SkillObjTrigger.Remove);
        SkillManager.Instance.CheckTrigger(user, myTrigger, transform.position);
        myTrigger.Remove((long)SkillObjTrigger.Remove);
        PoolingManager.Instance.ReturnObject(gameObject);
    }
    //Hit�Ǿ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitEnemy(collision.transform.GetComponent<BaseController>());
                        
            if (currentBounce < projectileBounceCount)
            {
                Bounce();
                currentBounce++;
            }else if(currentPiercing < projectilePiercingCount)
            {
                currentPiercing++;
            }
            else
            {
                InActiveSkill();
                return;
                //SpawnManager.Instance.chainShotObjectPool.ReturnToPool(gameObject);
            }
        }
    }
    private void HitEnemy(BaseController controller)
    {
        float damage = (
            (projectileBaseDamage +
            user.GetStatValue(StatName.BaseDamage) +
            user.GetStatValue(StatName.BaseDamage, ID)) *
            (1f + user.GetStatMuliply(StatName.BaseDamage) / 100f +
            user.GetStatMuliply(StatName.BaseDamage, ID) / 100f)
            ) * projectileBaseMultiply;

        //controller.GetDamageByHit((
        //    (projectileBaseDamage + 
        //    user.GetStatValue(StatName.BaseDamage) + 
        //    user.GetStatValue(StatName.BaseDamage, ID)) *
        //    (1f + user.GetStatMuliply(StatName.BaseDamage) / 100f +
        //    user.GetStatMuliply(StatName.BaseDamage, ID) / 100f)
        //    ) * projectileBaseMultiply

        //    );
        controller.GetHit(user, myTrigger, damage);
    }
    //����ü ������
    private void MoveProjectile()
    {
        //����ü ����
        angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        projectileAni.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //����ü �̵�
        transform.Translate(moveDirection * projectileSpeed * Time.deltaTime);
    }
    //chain�� ������ ƨ�� ����
    private void Bounce()
    {
        currentLifeTime = lifeTime;

        float minDistance = Mathf.Infinity;
        Collider2D closestEnemy = null;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    if (closestEnemy == null || closestEnemy != collider)
                    {
                        closestEnemy = collider;
                    }
                }
            }
        }

        if (closestEnemy != null)
        {
            //target = closestEnemy.GetComponent<Collider2D>();
            moveDirection = (closestEnemy.transform.position - transform.position).normalized;
        }
    }
}
