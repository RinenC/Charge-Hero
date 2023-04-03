using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBLoader : MonoSingleton<DBLoader>
{
    public Sprite healitem;
    public Sprite damageitem;
    public Sprite invincibleitem;
    public Sprite aviationitem;
    public Sprite changeCoinitem;
    public Sprite protruding;
    public Sprite patrol;

    // Start is called before the first frame update
    void Start()
    {
        LoadSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Quest { Type, Title, info, value }
    public void LoadSprite()
    {
        healitem = Resources.Load<Sprite>("Image/HP Item");
        damageitem = Resources.Load<Sprite>("Image/Damage Item");
        invincibleitem = Resources.Load<Sprite>("Image/Invincible");
        aviationitem = Resources.Load<Sprite>("Image/flight");
        changeCoinitem = Resources.Load<Sprite>("Image/ChangeCoin");
        protruding = Resources.Load<Sprite>("Image/Frog");
        patrol = Resources.Load<Sprite>("Image/Dino");
    }
}
