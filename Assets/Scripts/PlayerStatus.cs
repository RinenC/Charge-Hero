using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : MonoBehaviour
{
    public Status status;
    public int HP { get { return status.hp; } set { status.hp = value; } }
    public int ATK { get { return status.atk; } set { status.atk = value; } }
    public int DEF { get { return status.def_cnt; } set { status.def_cnt = value; } } // 무적 Item 지속시간 증가
    
    PlayerControl control;
    PlayerEffect effect;

    //[Header("아이템 이펙트")]
    //float gravity = 0;
    //public GameObject invincibleSprite;
    //public GameObject aviationSprite;

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<PlayerControl>();
        effect = GetComponent<PlayerEffect>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitStatus(Status status)
    {
        this.status = status;
    }
    public void Damaged(float dmg)
    {
        if (!effect.INVICIBILLITY)
        {
            StageManager.instance.attacked_Cnt++;
            if (DEF > 0)
            {
                DEF -= (int)dmg;
                GUIManager.instance.SHIELD_UI.Remove();
            }
            else
            {
                HP -= (int)dmg;
                GUIManager.instance.HP_UI.Remove();
            }

            if (HP <= 0) control.ChangeState(PlayerControl.E_State.DIE);
            else effect.Activate_Effect(PlayerEffect.E_effect.AttackedInvin);
        }
    }
    //public void Upgrade(PlusStatus plusStat)
    //{
    //    HP += plusStat.hp;
    //    ATK += plusStat.atk;
    //    DEF += plusStat.def_cnt;
    //}

    //public void InvincibleItemEffect()
    //{
    //    if (!GameManager.instance.aviation)
    //    {
    //        if (!GameManager.instance.invincible)
    //            invincibleSprite.SetActive(false);
    //        else
    //            invincibleSprite.SetActive(true);
    //    }
    //}

    //public void AviationItemEffect()
    //{
    //    if (GameManager.instance.aviation)
    //    {
    //        if (GameManager.instance.aviationTimer > 0)
    //        {
    //            GetComponent<Rigidbody2D>().gravityScale = 0;
    //            aviationSprite.SetActive(true);
    //        }
    //    }
    //    if (!GameManager.instance.aviation)
    //    {
    //        GetComponent<Rigidbody2D>().gravityScale = gravity;
    //        aviationSprite.SetActive(false);
    //    }
    //}
}
