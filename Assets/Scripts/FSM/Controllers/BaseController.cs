using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;
using System;

/// <summary>
/// 1. ���� ���¸ӽ��� ���� ��Ʈ�ѷ� ����
/// 2. ������ �����ϱ� ���� ��Ʈ�ѷ� ����
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
    /// ���� ��ųʸ�
    /// </summary>
    /// 
    public List<Stat> stats = new List<Stat>();
   // public SerializableDictionary<StatName, Stat> statDic = new SerializableDictionary<StatName, Stat>();
    //public Dictionary<StatName, lStat> statDic = new Dictionary<StatName, Stat>();
    /// <summary>
    /// ��ų Trigger
    /// </summary>
    public List<long> MyTrigger = new List<long>(); //��ų �����

    public List<SkillBase> skills;
    // ScriptableObject�� ���� FloatVariable
    public float HP;
    public float MaxHP;
    // ScriptableObject�� ���� GameEvent
    public UnityEvent gameEvtHPChange;
    public Animator animator;

    public State currentState;
    private int moveTypeID = -1;
    public int MoveType;
    bool isChangingDirection = false;
    float elapsedTime = 0f;
    public float rotationTime = 0.2f; // ȸ���� �ɸ��� �ð�
    private Coroutine directionCo;
    public SkeletonAnimation spine;

    public SkillBase lastDamagedSkill;
    //�����̻��
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
        // �⺻ ��ų�� ��Ÿ���� ���� �Ϸ�Ǿ���, �ٽ� �⺻ ��ų�� �ߵ��ؾ���
        MyTrigger.Add((long)ControllerTrigger.Attack);
        SkillManager.Instance.CheckTrigger(this, MyTrigger, transform.position);
        MyTrigger.Remove((long)ControllerTrigger.Attack);
    }
    public virtual void GetDamageByHit(float dmg)
    {
        // ���ظ� ������ dmg ��ŭ runtimeValue ���� ����      
        HP -= dmg;
        if (HP <= 0)
        {
            HP = 0;
        }
        // �̺�Ʈ �߻� ��Ŵ
        gameEvtHPChange.Invoke();
    }
    public void GetHit(BaseController user, List<long> skillTrigger, float damage)
    {
        MyTrigger.Add((long)ControllerTrigger.Damaged);
        SkillManager.Instance.CheckTrigger(user, this, skillTrigger, transform.position); //��ų �Ŵ����� projectile hit ����
        MyTrigger.Remove((long)ControllerTrigger.Damaged);
        GetDamageByHit(damage);
    }
    public virtual void GetDamageByDotDamage(float dmg)
    {
        // ���ظ� ������ dmg ��ŭ runtimeValue ���� ����      
        HP -= dmg;
        if (HP <= 0)
        {
            HP = 0;
        }
        // �̺�Ʈ �߻� ��Ŵ
        gameEvtHPChange.Invoke();
    }
    public virtual void Heal(float heal)
    {
        // ġ���� ������ heal ��ŭ runtimeValue ����
        HP += heal;
        if (HP >= MaxHP)
        {
            HP = MaxHP;
        }

        // �̺�Ʈ �߻� ��Ŵ
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

        //hp�� ����� ��� �����̶� �� �ٽ�����
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
    //�����̻��
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