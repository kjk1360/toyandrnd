using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SkillManager : MonoBehaviour
{
    //public SerializableDictionary<BaseController, List<TheTrigger>> controllerTriggerDic = new SerializableDictionary<BaseController, List<TheTrigger>>();

    public List<TheTrigger> TriggerList= new List<TheTrigger>();
    public static SkillManager _instance;
    public static SkillManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SkillManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("SkillManager");
                    _instance = singleton.AddComponent<SkillManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    /// <summary>
    /// listManager의 모든 구성요소가 listForCheck 포함되어 있는지 확인
    /// </summary>
    /// <param name="listManager"></param>
    /// <param name="listForCheck"></param>
    /// <returns></returns>
    public bool CompareSkillKeywordLists(List<long> listManager, List<long> listForCheck)
    {
        return !listManager.Except(listForCheck).Any();
    }
    public void CheckTrigger(BaseController user, List<long> keywords, Vector3 triggerPos)
    {
        foreach (TheTrigger trigger in TriggerList)
        {
            if (CompareSkillKeywordLists(trigger.UserTrigger, user.MyTrigger) &&
                CompareSkillKeywordLists(trigger.SkillTrigger, keywords) &&
                trigger.TargetTrigger.Count == 0)
            {
                trigger.ActivateSkill(user, triggerPos);
            }
        }
    }
    public void CheckTrigger(BaseController user, Vector3 triggerPos)
    {
        foreach (TheTrigger trigger in TriggerList)
        {
            if (CompareSkillKeywordLists(trigger.UserTrigger, user.MyTrigger) &&
                trigger.SkillTrigger.Count == 0 &&
                trigger.TargetTrigger.Count == 0)
            {
                trigger.ActivateSkill(user, triggerPos);
            }
        }
    }

    public void CheckTrigger(BaseController user, BaseController target, List<long> keywords, Vector3 triggerPos)
    {
        foreach (TheTrigger trigger in TriggerList)
        {
            if (CompareSkillKeywordLists(trigger.UserTrigger, user.MyTrigger) &&
                CompareSkillKeywordLists(trigger.SkillTrigger, keywords) &&
                CompareSkillKeywordLists(trigger.TargetTrigger, target.MyTrigger))
            {
                trigger.ActivateSkill(user, triggerPos);
            }
        }
    }
    //public void CheckTrigger(BaseController user, List<long> keywords, Vector3 triggerPos)
    //{
    //    if(controllerTriggerDic.ContainsKey(user))
    //    {
    //        foreach(TheTrigger trigger in controllerTriggerDic[user])
    //        {
    //            if (CompareSkillKeywordLists(trigger.UserTrigger, user.MyTrigger) &&
    //                CompareSkillKeywordLists(trigger.SkillTrigger, keywords) &&
    //                trigger.TargetTrigger.Count == 0)
    //            {
    //                trigger.ActivateSkill(user, triggerPos);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        controllerTriggerDic.Add(user, new List<TheTrigger>());
    //    }
    //}
    //public void CheckTrigger(BaseController user, Vector3 triggerPos)
    //{
    //    if (controllerTriggerDic.ContainsKey(user))
    //    {
    //        foreach (TheTrigger trigger in controllerTriggerDic[user])
    //        {
    //            if (CompareSkillKeywordLists(trigger.UserTrigger, user.MyTrigger) && 
    //                trigger.SkillTrigger.Count == 0 && 
    //                trigger.TargetTrigger.Count == 0)
    //            {
    //                trigger.ActivateSkill(user, triggerPos);
    //            }
    //        }
    //    }
    //}

    //public void CheckTrigger(BaseController user, BaseController target, List<long> keywords, Vector3 triggerPos)
    //{
    //    if (controllerTriggerDic.ContainsKey(user))
    //    {
    //        foreach (TheTrigger trigger in controllerTriggerDic[user])
    //        {
    //            if (CompareSkillKeywordLists(trigger.UserTrigger, user.MyTrigger) &&
    //                CompareSkillKeywordLists(trigger.SkillTrigger, keywords) &&
    //                CompareSkillKeywordLists(trigger.TargetTrigger, target.MyTrigger))
    //            {
    //                trigger.ActivateSkill(user, triggerPos);
    //            }
    //        }
    //    }
    //}
    //controllerTriggerDic에 해당 유저가 있는지 확인 후 없으면 추가하여 트리거를 추가한다
    //public void AddTrigger(BaseController user, TheTrigger trigger)
    //{
    //    if (!controllerTriggerDic.ContainsKey(user))
    //    {
    //        controllerTriggerDic.Add(user, new List<TheTrigger>());
    //    }
    //    if(trigger != null && !controllerTriggerDic[user].Contains(trigger))
    //    {
    //        controllerTriggerDic[user].Add(trigger);
    //    }
    //}
    //public void RemoveTrigger(BaseController user, TheTrigger trigger)
    //{
    //    if(controllerTriggerDic != null && controllerTriggerDic[user].Contains(trigger))
    //    {
    //        controllerTriggerDic[user].Remove(trigger);
    //    }
    //}

    public void AddTrigger(BaseController user, TheTrigger trigger)
    {

        if (!TriggerList.Contains(trigger))
        {
            TriggerList.Add(trigger);
        }
    }
    public void RemoveTrigger(BaseController user, TheTrigger trigger)
    {
        if(TriggerList.Contains(trigger))
        {
            TriggerList.Remove(trigger);
        }
    }
}
