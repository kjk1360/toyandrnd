using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    // Image 객체
    public Image fill;

    // ScriptableObject로 만든 FloatVariable
    //public FloatVariable hp;
    public BaseController me;
    // 수치 표기
    public Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        //me = GetComponent<BaseController>();
        // HPBar 의 초기 설정
        fill.fillAmount = me.MaxHP / me.MaxHP;
        hpText.text = me.MaxHP + "/" + me.MaxHP;
    }

    // GameEventListener 에 연결되는 이벤트 함수
    public void HPChanged()
    {
        fill.fillAmount = me.HP / me.MaxHP;
        hpText.text = me.HP + "/" + me.MaxHP;
    }
}