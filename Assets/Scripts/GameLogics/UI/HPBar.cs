using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    // Image ��ü
    public Image fill;

    // ScriptableObject�� ���� FloatVariable
    //public FloatVariable hp;
    public BaseController me;
    // ��ġ ǥ��
    public Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        //me = GetComponent<BaseController>();
        // HPBar �� �ʱ� ����
        fill.fillAmount = me.MaxHP / me.MaxHP;
        hpText.text = me.MaxHP + "/" + me.MaxHP;
    }

    // GameEventListener �� ����Ǵ� �̺�Ʈ �Լ�
    public void HPChanged()
    {
        fill.fillAmount = me.HP / me.MaxHP;
        hpText.text = me.HP + "/" + me.MaxHP;
    }
}