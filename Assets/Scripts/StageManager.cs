using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public GameObject go_Player;
    public GameObject go_Boss;
    public Stage stage;
    UI_Distance ui_Dist;

    // 구출한 동료 수
    public int rescue;

    // 보스 죽이기
    public bool kill;

    // 피격 횟수
    public int attacked_Cnt;
    
    public Quest[] quests;
    public int[] questidx;

    private void Awake()
    {
        Debug.Log("StageManager_Awake");
        if (instance == null)
        {
            instance = this;
            ui_Dist = GUIManager.instance.ui_Dist;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    public void Set()
    {
        //8,8,8
        BuffManager.instance.Reset();// 버프 먹고 나간 뒤 다시 들어가면 타이머가 남아있는 현상
        stage = GameManager.instance.GetStageData();
        Init();

        go_Boss.GetComponent<BossMonster>().HP = stage.bossHP;
        GUIManager.instance.SetUI();

        string[] tmp = stage.questType.Split(',');
        questidx = System.Array.ConvertAll<string, int>(tmp, int.Parse);
        quests = new Quest[3];

        for (int i = 0; i < quests.Length; i++)
        {
            quests[i] = Array.Find(QuestManager.instance.Arr_Quest, x => x.idx == questidx[i]);
        }
    }
    void Init()
    {
        rescue = 0;
        kill = false;
        attacked_Cnt = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("StageManager_Start");
        // Boss 체력 설정
        
    }
    public void CheckQuest()
    {
        int clear_Cnt = 0;
        for(int i =0; i < quests.Length; i++)
        {
            switch (quests[i].type)
            {
                case E_Quest.Run:
                    if(ui_Dist.percent >= quests[i].value) clear_Cnt++;
                    break;

                case E_Quest.HP: // Boss Kill 조건
                    float rat = (float)go_Player.GetComponent<PlayerStatus>().HP / (float)GameManager.instance.status.hp;
                    if (rat >= quests[i].value) clear_Cnt++;
                    break;

                case E_Quest.Rescue:
                    if (rescue >= quests[i].value)
                    {
                        //quests[i].clear = true;
                        clear_Cnt++;
                    }
                    break;

                case E_Quest.Attacked:
                    if (kill)
                    {
                        if (attacked_Cnt <= quests[i].value)
                        {
                            //quests[i].clear = true;
                            clear_Cnt++;
                        }
                    }
                    break;

                case E_Quest.BOSS:
                    if (kill)
                    {
                        //quests[i].clear = true;
                        clear_Cnt++;
                    }
                    break;
            }
        }
        
        int reward_Gold = stage.repeat ? (int)(stage.repeat_Gold * ui_Dist.percent) : stage.first_Gold;
        GameManager.instance.n_Gold += reward_Gold;
        stage.Update_Info(clear_Cnt, ui_Dist.percent, kill);
        GUIManager.instance.Event_ShowResult(stage.getStar,reward_Gold, kill);
    }
}
