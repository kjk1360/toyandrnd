using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
    // �ʱ� ���� ��
    public float initValue;

    // ���� ���� ���� �� ����� ��
    // NonSerialized �̱� ������ runtimeValue ��
    // ��ũ�� ������� �ʽ��ϴ�.
    [System.NonSerialized]
    public float runtimeValue;

    // ��ũ�� ����� ���� ������ȭ�� initValue �� ������ ��   
    public void OnAfterDeserialize()
    {
        runtimeValue = initValue;
    }

    public void OnBeforeSerialize() { }
}