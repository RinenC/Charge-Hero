using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : MonoBehaviour
{
    public GameObject go_Player;
    public PlayerStatus player_status;
    public Text text;
    string msg;
    public enum E_Status { HP, ATK, DEF, MAP }
    public E_Status status;
    // Start is called before the first frame update
    void Start()
    {
        go_Player = GameManager.instance.go_Player;
        player_status = go_Player.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(status)
        {
            case E_Status.HP:
                msg = string.Format("HP : " + player_status.HP.ToString());
                text.text = msg;
                break;
            case E_Status.ATK:
                msg = string.Format("ATK : " + player_status.ATK.ToString());
                text.text = msg;
                break;
            case E_Status.DEF:
                msg = string.Format("DEF : " + player_status.DEF.ToString());
                text.text = msg;
                break;
            case E_Status.MAP:
                msg = string.Format("MAP : " + GameManager.instance.map);
                text.text = msg;
                break;
        }
    }
}
