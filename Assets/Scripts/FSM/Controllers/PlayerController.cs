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
    //    // ���ظ� ������ dmg ��ŭ runtimeValue ���� ����      
    //    floatHP.runtimeValue -= dmg;
    //    if (floatHP.runtimeValue <= 0)
    //    {
    //        floatHP.runtimeValue = 0;
    //    }
    //    // �̺�Ʈ �߻� ��Ŵ
    //    gameEvtHP.Raise();
    //}

    //public void Heal(float heal)
    //{
    //    // ġ���� ������ heal ��ŭ runtimeValue ����
    //    floatHP.runtimeValue += heal;
    //    if(floatHP.runtimeValue >= 100)
    //    {
    //        floatHP.runtimeValue = 100;
    //    }

    //    // �̺�Ʈ �߻� ��Ŵ
    //    gameEvtHP.Raise();
    //}

    
}
