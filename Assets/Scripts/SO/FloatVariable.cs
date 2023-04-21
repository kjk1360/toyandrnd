using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
    // 초기 설정 값
    public float initValue;

    // 실제 게임 실행 시 사용할 값
    // NonSerialized 이기 때문에 runtimeValue 는
    // 디스크에 저장되지 않습니다.
    [System.NonSerialized]
    public float runtimeValue;

    // 디스크에 저장된 값을 역직렬화로 initValue 로 가져온 후   
    public void OnAfterDeserialize()
    {
        runtimeValue = initValue;
    }

    public void OnBeforeSerialize() { }
}