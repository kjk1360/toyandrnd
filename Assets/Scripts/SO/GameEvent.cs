using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    // 발생 시킨 이벤트를 수신할 listener 목록
    private List<GameEventListener> listeners = new List<GameEventListener>();

    // 이벤트 발생
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }

    // listener 등록
    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    // 등록된 listener 해제
    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}