using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;
using System;

/// <summary>
/// 1. 유한 상태머신의 상태 컨트롤러 역할
/// 2. 스탯을 관리하기 위한 컨트롤러 역할
/// </summary>
/// 
[Serializable]
public abstract class BaseController : MonoBehaviour
{
    public const int NORTH = 0;
    public const int NORTHEAST = 1;
    public const int EAST = 2;
    public const int SOUTHEAST = 3;
    public const int SOUTH = 4;
    public const int SOUTHWEST = 5;
    public const int WEST = 6;
    public const int NORTHWEST = 7;
    /// <summary>
    /// 스텟 딕셔너리
    /// </summary>
    /// 
    public List<Stat> stats = new List<Stat>();
   // public SerializableDictionary<StatName, Stat> statDic = new SerializableDictionary<StatName, Stat>();
    //public Dictionary<StatName, lStat> statDic = new Dictionary<StatName, Stat>();
    /// <summary>
    /// 스킬 Trigger
    /// </summary>
    public List<long> MyTrigger = new List<long>(); //스킬 사용자

    public List<SkillBase> skills;
    // ScriptableObject로 만든 FloatVariable
    public float HP;
    public float MaxHP;
    // ScriptableObject로 만든 GameEvent
    public UnityEvent gameEvtHPChange;
    public Animator animator;

    public State currentState;
    private int moveTypeID = -1;
    public int MoveType;
    bool isChangingDirection = false;
    float elapsedTime = 0f;
    public float rotationTime = 0.2f; // 회전에 걸리는 시간
    private Coroutine directionCo;
    public SkeletonAnimation spine;

    public SkillBase lastDamagedSkill;
    //상태이상용
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    public event Action UpdateStatus;

    //base Attack Coro
    public Coroutine attack;

    public virtual void Start()
    {
        //MaxHP = (1f + GetStatValue(StatName.BaseHealth)) * (1f + GetStatMuliply(StatName.BaseHealth));
        moveTypeID = Animator.StringToHash("MoveType");
        InitializeController();
    }
    public void SetMoveType(int moveType)
    {
        if(moveType != MoveType)
        {
            animator.SetInteger(moveTypeID, moveType);
            MoveType = moveType;
        }
    }

    //public void SetPlayerMoveType(int moveType)
    //{
    //    if (moveType != MoveType)
    //    {
    //        //animator.SetInteger(moveTypeID, moveType);
    //        MoveType = moveType;
    //        isChangingDirection = true;
    //        if(directionCo != null)
    //        {
    //            StopCoroutine(directionCo);
    //        }
    //        if(moveType == 1)
    //        {
    //            //spine.transform.localScale = new Vector3(-1, 1, 1);
    //            spine.Skeleton.ScaleX = -1;
    //        }
    //        else
    //        {
    //            //spine.transform.localScale = Vector3.one;
    //            spine.Skeleton.ScaleX = 1;
    //        }
    //        //MoveType == 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);

    //        //directionCo = StartCoroutine(ChangeDirectionCoroutine());
    //    }
    //    isChangingDirection = false;
    //}
    public void SetPlayerMoveType(int moveType)
    {
        if (moveType != MoveType)
        {
            MoveType = moveType;
            isChangingDirection = true;

            if (directionCo != null)
            {
                StopCoroutine(directionCo);
            }

            // Update the animation based on the new move type
            switch (moveType)
            {
                case NORTH:
                    spine.AnimationName = "05_back";
                    spine.Skeleton.ScaleX = 1;
                    break;
                case NORTHEAST:
                    spine.AnimationName = "04_back_half";
                    spine.Skeleton.ScaleX = -1;
                    break;
                case EAST:
                    spine.AnimationName = "03_side";
                    spine.Skeleton.ScaleX = -1;
                    break;
                case SOUTHEAST:
                    spine.AnimationName = "02_front_half";
                    spine.Skeleton.ScaleX = -1;
                    break;
                case SOUTH:
                    spine.AnimationName = "01_front";
                    spine.Skeleton.ScaleX = 1;
                    break;
                case SOUTHWEST:
                    spine.AnimationName = "02_front_half";
                    spine.Skeleton.ScaleX = 1;
                    break;
                case WEST:
                    spine.AnimationName = "03_side";
                    spine.Skeleton.ScaleX = 1;
                    break;
                case NORTHWEST:
                    spine.AnimationName = "04_back_half";
                    spine.Skeleton.ScaleX = 1;
                    break;
            }

            isChangingDirection = false;
        }
    }
    private IEnumerator ChangeDirectionCoroutine()
    {
        if (!isChangingDirection)
        {
            elapsedTime = rotationTime - elapsedTime;
        }
        else
        {
            elapsedTime = 0f;
        }

        Vector3 startScale = spine.transform.localScale;
        Vector3 targetScale = MoveType == 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);

        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationTime);
            spine.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        isChangingDirection = false;
    }

    public void SetHPMax()
    {
        var diff = MaxHP - (1f + GetStatValue(StatName.BaseHealth)) * (1f + GetStatMuliply(StatName.BaseHealth));
        MaxHP = (1f + GetStatValue(StatName.BaseHealth)) * (1f + GetStatMuliply(StatName.BaseHealth));
        HP -= diff;
    }
    public virtual void InitializeController() 
    {
        HP = MaxHP;
        gameEvtHPChange.Invoke();
    }

    public virtual void Update()
    {
        UpdateStatus?.Invoke();
        currentState.UpdateState(this);
    }
    public void Attack()
    {
        // 기본 스킬의 쿨타임이 충전 완료되었음, 다시 기본 스킬을 발동해야함
        MyTrigger.Add((long)ControllerTrigger.Attack);
        SkillManager.Instance.CheckTrigger(this, MyTrigger, transform.position);
        MyTrigger.Remove((long)ControllerTrigger.Attack);
    }
    public virtual void GetDamageByHit(float dmg)
    {
        // 피해를 받으면 dmg 만큼 runtimeValue 에서 차감      
        HP -= dmg;
        if (HP <= 0)
        {
            HP = 0;
        }
        // 이벤트 발생 시킴
        gameEvtHPChange.Invoke();
    }
    public void GetHit(BaseController user, List<long> skillTrigger, float damage)
    {
        MyTrigger.Add((long)ControllerTrigger.Damaged);
        SkillManager.Instance.CheckTrigger(user, this, skillTrigger, transform.position); //스킬 매니저에 projectile hit 보고
        MyTrigger.Remove((long)ControllerTrigger.Damaged);
        GetDamageByHit(damage);
    }
    public virtual void GetDamageByDotDamage(float dmg)
    {
        // 피해를 받으면 dmg 만큼 runtimeValue 에서 차감      
        HP -= dmg;
        if (HP <= 0)
        {
            HP = 0;
        }
        // 이벤트 발생 시킴
        gameEvtHPChange.Invoke();
    }
    public virtual void Heal(float heal)
    {
        // 치유를 받으면 heal 만큼 runtimeValue 증가
        HP += heal;
        if (HP >= MaxHP)
        {
            HP = MaxHP;
        }

        // 이벤트 발생 시킴
        gameEvtHPChange.Invoke();
    }
    public int GetStatValue(StatName name, long id = 0)
    {
        foreach(var stat in stats)
        {
            if(stat.name == name && stat.targetID == id)
            {
                return stat.value;
            }
        }
        return 0;
    }

    public float GetStatMuliply(StatName name, long id = 0)
    {
        foreach (var stat in stats)
        {
            if (stat.name == name && stat.targetID == id)
            {
                return stat.multiply;
            }
        }
        return 0;
    }

    public void AddStatValue(StatName name, int addvalue, long id = 0)
    {
        int i = 0;
        foreach (var stat in stats)
        {
            i++;
            if (stat.name == name && stat.targetID == id)
            {
                stats[i].value += addvalue;
                return;
            }
        }
        stats.Add(new Stat(name, id, addvalue, 0f));

        //hp는 변경시 즉시 적용이라 셋 다시해줌
        if(name == StatName.BaseHealth)
        {
            SetHPMax();
        }
    }
    public void AddStatMuliply(StatName name, float addvalue, long id = 0)
    {
        int i = 0;
        foreach (var stat in stats)
        {
            i++;
            if (stat.name == name && stat.targetID == id)
            {
                stats[i].multiply += addvalue;
                return;
            }
        }
        stats.Add(new Stat(name, id, 0, addvalue));

    }
    public void AddSkill(SkillBase newSkill)
    {
        skills.Add(newSkill);
    }

    public void UseSkill(int index)
    {
        if (index >= 0 && index < skills.Count)
        {
            if (!skills[index].IsOnCooldown())
            {
                skills[index].ActivateSkill(this, transform.position);
                skills[index].isOnCooldown = true;
            }
        }
    }
    public void ChangeState(State newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    //상태이상용
    public void ApplyEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
        effect.Apply(this);
    }

    public void RemoveEffect(StatusEffect effect)
    {
        if (statusEffects.Contains(effect))
        {
            effect.Remove(this);
            statusEffects.Remove(effect);
        }
    }

    public IEnumerator UpdateAttack()
    {
        while(currentState is PlayerAttackState)
        {
            yield return new WaitForSeconds(1/(GetStatValue(StatName.AttackSpeed)*(1f + GetStatMuliply(StatName.AttackSpeed)/100f)));
            Attack();
        }
    }
}