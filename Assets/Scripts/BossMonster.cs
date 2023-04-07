using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public GameObject goal;
    public float HP;
    GameObject go_Player;
    Animator anim;
    SpriteRenderer sr;
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("BossMonster.Awake");
        goal.transform.position = new Vector3(transform.position.x + 5, -2.0f, transform.position.z);
        StageManager.instance.go_Boss = this.gameObject;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            go_Player = collision.gameObject;
            //go_Player.GetComponent<PlayerControl>().
            if (HP > go_Player.GetComponent<PlayerStatus>().ATK)
            {
                SoundManager.instance.PlayEffect("Attacked");
                // ������ �����ϴ� Animation �߰� //
                go_Player.GetComponent<PlayerControl>().ChangeState(PlayerControl.E_State.Attacked);
                StageManager.instance.kill = false;
            }
            else
            {
                SoundManager.instance.PlayEffect("Attack");
                StageManager.instance.kill = true;
                // ������ ���� ���ϴ� Animation �߰� //
                anim.SetTrigger("doDie");
                sr.color = Color.gray;
            }
            HP -= go_Player.GetComponent<PlayerStatus>().ATK;
            StartCoroutine(GUIManager.instance.bossHP_UI.HPUI((float)HP));
        }
    }
    void Kill_ThePlayer()
    {
        Debug.Log("Kill_ThePlayer");
    }
    void Die()
    {
        Debug.Log("Die");
        //Time.timeScale = 1;
    }
}
