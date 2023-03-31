using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerEffect : MonoBehaviour
{
    PlayerControl control;
    SpriteRenderer sr;

    public enum E_effect { Avitaton, ItemInvin, AttackedInvin }
    public bool[] b_Effected = new bool[2]; // Effect Ȱ��ȭ ����
    public GameObject[] go_Effects;         // Effect ������Ʈ    
    
    public float f_Magnetic_Rad;            // �ڼ� ����
    public float f_Invic_time;              // �ǰ� ���� �ð�
    public Color[] colors;                  // �ǰ� ȿ�� ����

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<PlayerControl>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(b_Effected[0]) Avitation();// �ڼ� �� ����
    }
    public void Activate_Effect(E_effect type, float duration = 0)
    {
        switch (type)
        { 
            case E_effect.Avitaton:
                StartCoroutine(SetAvitaton(duration));
                break;
            case E_effect.ItemInvin:
                StartCoroutine(SetInvincible(duration));
                break;
            case E_effect.AttackedInvin:
                StartCoroutine(SetAttacked());
                break;
        }
    }

    IEnumerator SetAvitaton(float time)
    {
        control.ChangeState(PlayerControl.E_State.Aviation);
        for(int i =0; i< 2; i++)
        {
            b_Effected[i] = true;
            go_Effects[i].SetActive(true);
        }
        this.gameObject.layer = 9;
        yield return new WaitForSeconds(time);
        for (int i = 0; i < 2; i++)
        {
            b_Effected[i] = false;
            go_Effects[i].SetActive(false);
        }
        control.ChangeState(PlayerControl.E_State.Run);
        this.gameObject.layer = 0;
    }
    IEnumerator SetInvincible(float time)
    {
        b_Effected[(int)E_effect.ItemInvin] = true;
        go_Effects[(int)E_effect.ItemInvin].SetActive(true);
        this.gameObject.layer = 9;
        yield return new WaitForSeconds(time);
        b_Effected[(int)E_effect.ItemInvin] = false;
        go_Effects[(int)E_effect.ItemInvin].SetActive(false);
        this.gameObject.layer = 0;
    }
    IEnumerator SetAttacked()
    {
        Debug.Log("Invincibility_Start");
        this.gameObject.layer = 9;
        float sec = f_Invic_time / 10;
        for (int i = 0; i < 10; i++)
        {
            sr.color = colors[i % 2];
            yield return new WaitForSeconds(sec);
        }
        this.gameObject.layer = 0;
        Debug.Log("Invincibility_End");
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
