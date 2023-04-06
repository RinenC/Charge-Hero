using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Heal,
    Damage,
    Invincibility,
    Aviation,
    ChangeCoin,
    Gold,
}

public class Item : MonoBehaviour
{
    public ItemType curType;
    public int value;
    public bool moved;
    public float speed;

    public GameObject buffTimer;
    //public float duration;
    //public float timer;

    GameObject go_Collider;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (moved) Move_To_Player();
    }
    void Move_To_Player()
    {
        Vector3 v_Dir = GameManager.instance.go_Player.transform.position - transform.position;
        v_Dir.z = 0;
        v_Dir.Normalize();
        transform.position += v_Dir * speed * Time.deltaTime;
    }
    void UseItem()
    {
        switch (curType)
        {
            case ItemType.Heal: // 힐
                if (go_Collider.GetComponent<PlayerStatus>().HP < 10)
                {
                    go_Collider.GetComponent<PlayerStatus>().HP += value;
                    GUIManager.instance.HP_UI.Add();
                }
                break;

            case ItemType.Damage:// 파워업
                int atk = go_Collider.GetComponent<PlayerStatus>().ATK;
                StartCoroutine(GUIManager.instance.NumberAnimation(atk + value, atk, E_VALUE.ATK));
                go_Collider.GetComponent<PlayerStatus>().ATK += value;
                break;

            case ItemType.Invincibility:// 무적
                go_Collider.GetComponent<PlayerEffect>().Activate_Effect(PlayerEffect.E_effect.ItemInvin, value);
                break;

            case ItemType.Aviation:// 비행 + 무적 + 자석(골드만?)
                go_Collider.GetComponent<PlayerEffect>().Activate_Effect(PlayerEffect.E_effect.Avitaton, value);
                break;

            case ItemType.ChangeCoin:
                GameManager.instance.changeCoin = true;
                break;

            case ItemType.Gold: // 골드
                SoundManager.instance.PlayEffect("GetCoin");
                GameManager.instance.n_Gold += value;
                break;
        }

        Destroy(this.gameObject);
    }
    //void CreateTimer(ItemType itemType)
    //{
    //    scriptBT = buffTimer.GetComponent<BuffTimer>();
    //    scriptBT.buffTime = value;
    //    switch (itemType)
    //    {
    //        case ItemType.Invincible:
    //            scriptBT.type = "invincible";
    //            break;
    //        case ItemType.Aviation:
    //            scriptBT.type = "aviation";
    //            break;
    //    }
    //    var prefabBuffTimer = Instantiate(buffTimer, GUIManager.instance.buffBar.transform);
    //}
    //public void SetType(ItemType itemtype)
    //{
    //    switch (itemtype)
    //    {
    //        case ItemType.Heal:
    //            HealItem();
    //            break;
    //        case ItemType.Damage:
    //            DamageItem();
    //            break;
    //        case ItemType.Invincible:
    //            InvincibleItem();
    //            break;
    //        case ItemType.Aviation:
    //            AviationItem();
    //            break;
    //        case ItemType.ChangeCoin:
    //            break;
    //    }
    //    //SpriteChange(curType);
    //}

    //public void HealItem()
    //{
    //    float dist = Vector3.Distance(GameManager.instance.go_Player.transform.position, transform.position);

    //    if(dist < 1.5f)
    //    {
    //        GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP += 1;
    //        //go_Player.GetComponent<PlayerStatus>().HP
    //        Destroy(this.gameObject);
    //        GUIManager.instance.HPBar();
    //    }
    //}

    //public void DamageItem()
    //{
    //    float dist = Vector3.Distance(GameManager.instance.go_Player.transform.position, transform.position);

    //    if(dist < 1.5f)
    //    {
    //        GameManager.instance.go_Player.GetComponent<PlayerStatus>().ATK += 10;
    //        Destroy(this.gameObject);
    //    }
    //}

    //public void InvincibleItem()
    //{// Collider ?
    //    float dist = Vector3.Distance(GameManager.instance.go_Player.transform.position, transform.position);

    //    if(dist < 1.5f)
    //    {
    //        GameManager.instance.invincible = true;
    //        Destroy(this.gameObject);
    //    }
    //}

    //public void AviationItem()
    //{
    //    float dist = Vector3.Distance(GameManager.instance.go_Player.transform.position, transform.position);

    //    if (dist < 1.5f)
    //    {
    //        GameManager.instance.aviation = true;
    //        GameManager.instance.invincible = true;
    //        GameManager.instance.magnetic = true;
    //        if (!GameManager.instance.effected)
    //        {
    //            GameManager.instance.go_Player.GetComponent<PlayerControl>().f_Speed += 2;
    //            GameManager.instance.effected = true;
    //        }
    //        Destroy(this.gameObject);
    //    }
    //}

    //public void ChangeCoinItem()
    //{

    //}

    //public void SpriteChange(ItemType itemtype)
    //{
    //    // string itemName = itemtype.ToString();
    //    // GetComponent<SpriteRenderer>().sprite = DBLoader.instance.itemName;
    //    // --> Dictionary?
    //    switch (itemtype)
    //    {
    //        case ItemType.Heal:
    //            GetComponent<SpriteRenderer>().sprite = DBLoader.Instance.healitem;
    //            break;
    //        case ItemType.Damage:
    //            GetComponent<SpriteRenderer>().sprite = DBLoader.Instance.damageitem;
    //            break;
    //        case ItemType.Invincible:
    //            GetComponent<SpriteRenderer>().sprite = DBLoader.Instance.invincibleitem;
    //            break;
    //        case ItemType.Aviation:
    //            GetComponent<SpriteRenderer>().sprite = DBLoader.Instance.aviationitem;
    //            break;
    //        case ItemType.ChangeCoin:
    //            break;
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            go_Collider = collision.gameObject;
            UseItem();
        }
    }
}