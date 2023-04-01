using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum E_Status
{
    HP,
    ATK,
    DEF,
}
public class UI_Enhance : MonoBehaviour
{
    public Text txt_curStat;
    public Text txt_enhance;
    public Text txt_ned_Gold;
    public E_Status type;
    public Enhance enhance;
    // Start is called before the first frame update
    void Start()
    {
        // 프리팹화
        // Status Image 셋팅
        // GetComponent<Image>().sprite = 
    }
    public void Init() //lv 안 받고, GetEnhance(Type) 알아서 lv 에 맞게 가져옴.
    {//GetStatusInfo
        enhance = GameManager.instance.GetEnhanceInfo(type);
        txt_curStat.text = GameManager.instance.GetStatusInfo(type);
        txt_enhance.text = string.Format($"▲ {enhance.stat}");
        txt_ned_Gold.text = string.Format("{0:#,##0}", enhance.need_Gold);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Event_Upgrade()
    {
        if (GameManager.instance.n_Gold >= enhance.need_Gold)
        {
            StartCoroutine(GUIManager.instance.NumberAnimation(GameManager.instance.n_Gold - enhance.need_Gold, GameManager.instance.n_Gold, E_VALUE.GOLD));
            GameManager.instance.n_Gold -= enhance.need_Gold;
            switch (type)
            {
                case E_Status.HP:
                    if (GameManager.instance.status.hp < 10)
                    {
                        GameManager.instance.status.Lv_HP++;
                        GameManager.instance.status.hp += enhance.stat;
                    }
                    break;
                case E_Status.ATK:
                    GameManager.instance.status.Lv_ATK++;
                    GameManager.instance.status.atk += enhance.stat;
                    break;
                case E_Status.DEF:
                    if (GameManager.instance.status.def_cnt < 5)
                    {
                        GameManager.instance.status.Lv_DEF++;
                        GameManager.instance.status.def_cnt += enhance.stat;
                    }
                    break;
            }
        }
        Init();
    }
}
