using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TheProjectile : SkillBase
{
    //투사체에 필요한 스텟들
    public int projectileBounceCount; //투사체 연쇄 총 횟수
    public float projectileSpeed;  //투사체 속도
    public int projectilePiercingCount; //투사체 관통 총 횟수
    public float lifeTime = 3f;    //투사체 생존 시간
    public int projectileBaseDamage = 0; //투사체 기본 데미지
    public float projectileBaseMultiply = 1f;
    public float projectileAddedDamageMultiply = 0f; //투사체에 적용되는 공격력의 배율 (100이면 공격력의 100퍼센트가 추가)
    public GameObject projectileAni;



    public List<SkillObjTrigger> skillKeywordList; //미리 작성된 키워드들



    //사용되는 temp변수들
    private int currentBounce;      //투사체 현 연쇄 횟수
    private Vector2 moveDirection = Vector2.up; //투사체 방향
    private float angle;            //투사체 각도
    private float currentLifeTime;  //투사체 현 생존 시간
    private int currentPiercing;      //투사체 현 관통 횟수




    public List<long> myTrigger = new List<long>(); //내 자신의 트리거만 관리하면 됨

    //투사체는 본인이 생성된 위치부터 targetPos로 날아가기만 하면 됨
    public override void ActivateSkill(BaseController user, Vector3 targetPos)
    {
        this.user = user;
        if (isOnCooldown || !ShouldActivateSkill()) return;

        moveDirection = (targetPos - transform.position).normalized; //firstdirection
        myTrigger.Clear();
        myTrigger.Add(ID); //스킬 트리거에 스킬 아이디 저장
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
    //Hit되었을때
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
    //투사체 움직임
    private void MoveProjectile()
    {
        //투사체 각도
        angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        projectileAni.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //투사체 이동
        transform.Translate(moveDirection * projectileSpeed * Time.deltaTime);
    }
    //chain이 있을때 튕김 구현
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
