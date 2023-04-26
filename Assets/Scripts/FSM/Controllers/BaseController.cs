using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 1. ���� ���¸ӽ��� ���� ��Ʈ�ѷ� ����
/// 2. ������ �����ϱ� ���� ��Ʈ�ѷ� ����
/// </summary>
public abstract class BaseController : MonoBehaviour
{
    /// <summary>
    /// ���� ��ųʸ�
    /// </summary>
    public Dictionary<StatName, Stat> statDic = new Dictionary<StatName, Stat>();
    /// <summary>
    /// ��ų ����Ʈ
    /// </summary>
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
        currentState.UpdateState(this);
    }

    public virtual void GetHit(float dmg)
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
    public int GetStatValue(StatName name)
    {
        if (statDic.ContainsKey(name))
        {
            return statDic[name].value;
        }
        else
        {
            return 0;
        }
    }

    public float GetStatMuliply(StatName name)
    {
        if (statDic.ContainsKey(name))
        {
            return statDic[name].multiply;
        }
        return 0f;
    }

    public void AddStatValue(StatName name, int addvalue)
    {
        if (statDic.ContainsKey(name))
        {
            statDic[name].value += addvalue;
        }
        else
        {
            statDic.Add(name, new Stat(addvalue, 0f));
        }

        //hp�� ����� ��� �����̶� �� �ٽ�����
        if(name == StatName.BaseHealth)
        {
            SetHPMax();
        }
    }
    public void AddStatMuliply(StatName name, float addvalue)
    {
        if (statDic.ContainsKey(name))
        {
            statDic[name].multiply += addvalue;
        }
        else
        {
            statDic.Add(name, new Stat(0, addvalue));
        }
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
                skills[index].ActivateSkill(this);
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
}