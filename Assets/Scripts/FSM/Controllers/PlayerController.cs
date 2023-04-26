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
                // ���������� ������
                movetype = 2;
            }
            else
            {
                // �������� ������
                movetype = 1;
            }
        }
        else
        {
            if (vertical > 0)
            {
                // ���� ������
                movetype = 3;
            }
            else
            {
                // �Ʒ��� ������
                movetype = 0;
            }
        }

        return movetype;
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
