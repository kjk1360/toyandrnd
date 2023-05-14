using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;

/// <summary>
/// 1. 유한 상태머신의 상태 컨트롤러 역할
/// 2. 스탯을 관리하기 위한 컨트롤러 역할
/// </summary>
public abstract class BaseController : MonoBehaviour
{
    /// <summary>
    /// 스텟 딕셔너리
    /// </summary>
    public Dictionary<StatName, Stat> statDic = new Dictionary<StatName, Stat>();
    /// <summary>
    /// 스킬 리스트
    /// </summary>
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

    public void SetPlayerMoveType(int moveType)
    {
        if (moveType != MoveType)
        {
            //animator.SetInteger(moveTypeID, moveType);
            MoveType = moveType;
            isChangingDirection = true;
            if(directionCo != null)
            {
                StopCoroutine(directionCo);
            }
            directionCo = StartCoroutine(ChangeDirectionCoroutine());
        }
        isChangingDirection = false;
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
        currentState.UpdateState(this);
    }

    public virtual void GetHit(float dmg)
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

        //hp는 변경시 즉시 적용이라 셋 다시해줌
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