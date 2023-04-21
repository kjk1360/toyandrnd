using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    public override void InitializeController()
    {
        base.InitializeController();
        ChangeState(new PlayerAttackState());
    }

    
    //public void GetHit(float dmg)
    //{
    //    // 피해를 받으면 dmg 만큼 runtimeValue 에서 차감      
    //    floatHP.runtimeValue -= dmg;
    //    if (floatHP.runtimeValue <= 0)
    //    {
    //        floatHP.runtimeValue = 0;
    //    }
    //    // 이벤트 발생 시킴
    //    gameEvtHP.Raise();
    //}

    //public void Heal(float heal)
    //{
    //    // 치유를 받으면 heal 만큼 runtimeValue 증가
    //    floatHP.runtimeValue += heal;
    //    if(floatHP.runtimeValue >= 100)
    //    {
    //        floatHP.runtimeValue = 100;
    //    }

    //    // 이벤트 발생 시킴
    //    gameEvtHP.Raise();
    //}

    
}
