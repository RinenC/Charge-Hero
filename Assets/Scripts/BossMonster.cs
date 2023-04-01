using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public GameObject goal;
    public float HP;
    GameObject go_Player;
    // Start is called before the first frame update
    private void Awake()
    {
        //Debug.Log("BossMonster.Awake");
        goal.transform.position = new Vector3(goal.transform.position.x, -1.3f, transform.position.z);
        StageManager.instance.go_Boss = this.gameObject;
    }
    void Start()
    {
        //
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
                // 보스의 공격하는 Animation 추가 //
                go_Player.GetComponent<PlayerControl>().ChangeState(PlayerControl.E_State.Attacked);
                StageManager.instance.kill = false;
            }
            else
            {
                StageManager.instance.kill = true;
                // 보스의 공격 당하는 Animation 추가 //
            }
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
