using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    public GameObject player;

    public float a = 0;

    private void Start()
    {
        player = GameManager.instance.go_Player;
    }

    private void Update()
    {
        Moving();
        GetCompanion();
    }

    public void Moving()
    {
        a += 1 * Time.deltaTime;
        if (a > 1)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            a = 0;
        }
        else if(a > 0.5)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void GetCompanion()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        //Debug.Log(dist);
        if(dist < 2.2f)
        {
            int atk = player.GetComponent<PlayerStatus>().ATK;
            StartCoroutine(GUIManager.instance.NumberAnimation(atk + 5, atk, E_VALUE.ATK));
            player.GetComponent<PlayerStatus>().ATK += 5;
            //PlusStatus.instance.atk += 5;
            StageManager.instance.rescue += 1;
            Destroy(this.gameObject);
        }
    }
}
