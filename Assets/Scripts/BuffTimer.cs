using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffTimer : MonoBehaviour
{
    public float buffTime = 5;
    float timer;
    //public string type;
    public GameObject itemSprite;
    public Text txt_timer;

    public void Init(string type)
    {
        SetSprite(type);
        //buffTime = value;
    }
    private void OnEnable()
    {
        timer = buffTime;
    }
    // Update is called once per frame
    void Update()
    {
        TimerStart();
    }
    public void ResetTimer()
    {
        timer = buffTime;
    }
    void TimerStart()
    {
        txt_timer.text = timer.ToString("F1");

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            GameManager.instance.go_Player.GetComponent<PlayerEffect>().OffEffect();
            gameObject.SetActive(false);
        }

        //if(buffTimer <= 0)
        //{
        //    //buffTimer = buffTime;
        //    //Destroy(this.gameObject);
        //}

        //else if(buffTimer > 0)
        //{
        //    buffTimer -= Time.deltaTime;
        //}
    }

    void SetSprite(string type)
    {
        switch(type)
        {
            case "Invincibility":
                itemSprite.GetComponent<Image>().sprite = DBLoader.Instance.invincibleitem;
                break;
            case "Aviation":
                itemSprite.GetComponent<Image>().sprite = DBLoader.Instance.aviationitem;
                break;
        }
    }
}
