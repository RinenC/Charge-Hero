using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffTimer : MonoBehaviour
{
    public float buffTimer;
    public string type;
    public GameObject itemSprite;
    public Text timer;

    public void Init(string type, int value)
    {
        SetSprite(type);
        buffTimer = value;
    }
    // Update is called once per frame
    void Update()
    {
        TimerStart();
    }

    void TimerStart()
    {
        timer.text = buffTimer.ToString("F1");

        if(buffTimer <= 0)
        {
            //buffTimer = buffTime;
            Destroy(this.gameObject);
        }

        else if(buffTimer > 0)
        {
            buffTimer -= Time.deltaTime;
        }
    }

    void SetSprite(string type)
    {
        switch(type)
        {
            case "Invincible":
                itemSprite.GetComponent<Image>().sprite = DBLoader.Instance.invincibleitem;
                break;
            case "Aviation":
                itemSprite.GetComponent<Image>().sprite = DBLoader.Instance.aviationitem;
                break;
        }
    }
}
