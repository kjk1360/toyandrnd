using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    // 스크립트가 활성화 되면 GameEvent 에 등록합니다.
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    // 스크립트가 비활성화 되면 GameEvent 에서 해제합니다.
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    // 유니티 에디터에서 UnityEvent에 등록한 함수를 호출합니다.
    public void OnEventRaised()
    {
        Response.Invoke();
    }
}