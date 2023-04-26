using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    public Rigidbody2D myBody;
    float horizontal;
    float vertical;
    int movetype = 0;
    public override void InitializeController()
    {
        base.InitializeController();
        ChangeState(new PlayerAttackState());
    }

    public int GetMyBodyMoveType()
    {
        if (myBody == null)
        {
            myBody = GetComponent<Rigidbody2D>();
        }

        float horizontal = myBody.velocity.x;
        float vertical = myBody.velocity.y;

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            if (horizontal > 0)
            {
                // 오른쪽으로 움직임
                movetype = 2;
            }
            else
            {
                // 왼쪽으로 움직임
                movetype = 1;
            }
        }
        else
        {
            if (vertical > 0)
            {
                // 위로 움직임
                movetype = 3;
            }
            else
            {
                // 아래로 움직임
                movetype = 0;
            }
        }

        return movetype;
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
