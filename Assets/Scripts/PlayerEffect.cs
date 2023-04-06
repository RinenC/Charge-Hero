using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerEffect : MonoBehaviour
{
    PlayerControl control;
    SpriteRenderer sr;

    public enum E_effect { None, ItemInvin, Avitaton }// �����̶� ���� ����
    public bool[] b_Effected = new bool[2]; // Effect Ȱ��ȭ ����
    public GameObject[] go_Effects;         // Effect ������Ʈ    
    E_effect curEffect;
    public float f_Magnetic_Rad;            // �ڼ� ����
    public float f_Invic_time;              // �ǰ� ���� �ð�
    public Color[] colors;                  // �ǰ� ȿ�� ����
    public bool INVICIBILLITY { get { return (b_Effected[0] || b_Effected[1]); } }
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<PlayerControl>();
        sr = GetComponent<SpriteRenderer>();
        curEffect = E_effect.None;
    }

    // Update is called once per frame
    void Update()
    {
        if(b_Effected[1]) Avitation();// �ڼ� �� ����
    }
    public void Activate_Effect(E_effect type, float duration = 0)
    {
        ChangeEffect(type);
        //if (curEffect == E_effect.None)
        //{
        //    ChangeEffect(type);
        //}
        //else if(curEffect == type)
        //{
        //    ChangeEffect(curEffect);
        //}
        //else if((int)curEffect > (int)type) // ���� ���� �� ���� ȹ��
        //{
        //    ChangeEffect(curEffect);
        //}
        //else if((int)curEffect < (int)type) // ���� �� ����
        //{
        //    ChangeEffect(type);
        //}
    }
    public void Call_InvincibleMode()
    {
        StartCoroutine(SetAttacked());
    }
    void ChangeEffect(E_effect effect)
    {
        curEffect = effect;
        switch (curEffect)
        {
            case E_effect.None:
                if (control.state == PlayerControl.E_State.Aviation) control.ChangeState(PlayerControl.E_State.Run);
                this.gameObject.layer = 0;
                break;
            case E_effect.Avitaton:
                OnAvitation();
                BuffManager.instance.TurnOnBuffTimer(E_BUFFTYPE.Aviation);
                break;
            case E_effect.ItemInvin:
                OnInvincibility();
                BuffManager.instance.TurnOnBuffTimer(E_BUFFTYPE.Invincibility);
                break;
        }
    }
    void ResetEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            b_Effected[i] = false;
            go_Effects[i].SetActive(false);
        }
    }
    void OnAvitation()
    {
        ResetEffect();
        control.ChangeState(PlayerControl.E_State.Aviation);
        b_Effected[1] = true;
        go_Effects[1].SetActive(true);
        this.gameObject.layer = 11; // 9 ���� ��� ������
    }
    void OnInvincibility()
    {
        ResetEffect();
        b_Effected[0] = true;
        go_Effects[0].SetActive(true);
        this.gameObject.layer = 9;
    }
    public void OffEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            b_Effected[i] = false;
            go_Effects[i].SetActive(false);
        }
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ChangeEffect(E_effect.None);
    }
    IEnumerator SetAttacked()
    {
        //Debug.Log("Invincibility_Start");
        this.gameObject.layer = 9;
        float sec = f_Invic_time / 10;
        for (int i = 0; i < 10; i++)
        {
            sr.color = colors[i % 2];
            yield return new WaitForSeconds(sec);
        }
        if (curEffect == E_effect.None) this.gameObject.layer = 0;
        //Debug.Log("Invincibility_End");
    }
    void Avitation()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, f_Magnetic_Rad);
        foreach(Collider2D collider in colliders)
        {
            if(collider.GetComponent<Item>())
            {
                collider.GetComponent<Item>().moved = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, f_Magnetic_Rad);
    }
}
