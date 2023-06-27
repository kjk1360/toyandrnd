using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    public const int NORTH = 0;
    public const int NORTHEAST = 1;
    public const int EAST = 2;
    public const int SOUTHEAST = 3;
    public const int SOUTH = 4;
    public const int SOUTHWEST = 5;
    public const int WEST = 6;
    public const int NORTHWEST = 7;

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

        Vector2 velocity = myBody.velocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        // Ranges are chosen based on the 8 sectors centered on each direction.
        if (angle > -22.5f && angle <= 22.5f)
        {
            movetype = EAST;
        }
        else if (angle > 22.5f && angle <= 67.5f)
        {
            movetype = NORTHEAST;
        }
        else if (angle > 67.5f && angle <= 112.5f)
        {
            movetype = NORTH;
        }
        else if (angle > 112.5f && angle <= 157.5f)
        {
            movetype = NORTHWEST;
        }
        else if (angle > 157.5f || angle <= -157.5f)
        {
            movetype = WEST;
        }
        else if (angle > -157.5f && angle <= -112.5f)
        {
            movetype = SOUTHWEST;
        }
        else if (angle > -112.5f && angle <= -67.5f)
        {
            movetype = SOUTH;
        }
        else if (angle > -67.5f && angle <= -22.5f)
        {
            movetype = SOUTHEAST;
        }

        return movetype;
    }
    //public int GetMyBodyMoveType()
    //{
    //    if (myBody == null)
    //    {
    //        myBody = GetComponent<Rigidbody2D>();
    //    }

    //    //float horizontal = myBody.velocity.x;
    //    //float vertical = myBody.velocity.y;

    //    //if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
    //    //{
    //    //    if (horizontal > 0)
    //    //    {
    //    //        // 오른쪽으로 움직임
    //    //        movetype = 2;
    //    //    }
    //    //    else
    //    //    {
    //    //        // 왼쪽으로 움직임
    //    //        movetype = 1;
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    if (vertical > 0)
    //    //    {
    //    //        // 위로 움직임
    //    //        movetype = 3;
    //    //    }
    //    //    else
    //    //    {
    //    //        // 아래로 움직임
    //    //        movetype = 0;
    //    //    }
    //    //}

    //    float horizontal = myBody.velocity.x;
    //    if (horizontal > 0)
    //    {
    //        // 오른쪽으로 움직임
    //        movetype = 1;
    //    }
    //    else if(horizontal < 0)
    //    {
    //        // 왼쪽으로 움직임
    //        movetype = 0;
    //    }

    //    return movetype;
    //}

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
