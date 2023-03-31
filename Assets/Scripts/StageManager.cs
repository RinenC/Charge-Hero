using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Quest { Rescue, Attacked, BOSS }
[System.Serializable]
public struct Quest
{
    public E_Quest type;
    public string title;
    public int value;
    public bool clear;
}
public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public GameObject go_Player;
    public GameObject go_Boss;
    public Stage stage;
    // stage 를 클래스로 변경해서 
    // stage 의 bossHP 를 BossMonster 에 할당해도 되는가?
    // 구출한 동료 수
    public int rescue;

    // 보스 죽이기
    public bool kill;

    // 피격 횟수
    public int attacked_Cnt;
    
    public Quest[] quests;
    
    private void Awake()
    {
        Debug.Log("StageManager_Awake");
        if (instance == null)
        {
            instance = this;
            //quests = new Quest[3];
            stage = GameManager.instance.GetStageData();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Boss 체력 설정
        go_Boss.GetComponent<BossMonster>().HP = stage.bossHP;

        quests = new Quest[3];
        
        quests[0].type = E_Quest.Rescue;
        quests[0].title = "동료 3명 이상 구하기";
        quests[0].value = 3;

        quests[1].type = E_Quest.Attacked;
        quests[1].title = "3회 이하로 피격당하지 않고 클리어";
        quests[1].value = 4;

        quests[2].type = E_Quest.BOSS;
        quests[2].title = "보스 죽이기";
        quests[2].value = 0;
    }
    // DB 로 부터 Quest 종류 읽어오기.
    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckQuest()
    {
        int clear_Cnt = 0;
        for(int i =0; i < quests.Length; i++)
        {
            switch (quests[i].type)
            {
                case E_Quest.Rescue:
                    if (rescue >= quests[i].value)
                    {
                        quests[i].clear = true;
                        clear_Cnt++;
                    }
                    break;
                case E_Quest.Attacked:
                    if (kill)
                    {
                        if (attacked_Cnt < quests[i].value)
                        {
                            quests[i].clear = true;
                            clear_Cnt++;
                        }
                    }
                    break;
                case E_Quest.BOSS:
                    if (kill)
                    {
                        quests[i].clear = true;
                        clear_Cnt++;
                    }
                    break;
            }
        }

        GUIManager.instance.Event_ShowResult(clear_Cnt,kill);
        if (kill) stage.percent = 1;
        else stage.percent = GUIManager.instance.ui_Dist.percent;
        stage.SetStar(clear_Cnt);
    }
}
