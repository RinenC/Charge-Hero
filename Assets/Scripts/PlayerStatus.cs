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

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<PlayerControl>();
        effect = GetComponent<PlayerEffect>();
    }

    public void InitStatus(Status status)
    {
        this.status = status;
    }
    public void Damaged(float dmg)
    {
        //if (!effect.INVICIBILLITY)
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

            if (HP <= 0)
            {
                control.anim.SetTrigger("doDie");
                control.ChangeState(PlayerControl.E_State.End);
            }
            else effect.Call_InvincibleMode();
        }
    }
}
