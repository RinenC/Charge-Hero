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
    // stage �� Ŭ������ �����ؼ� 
    // stage �� bossHP �� BossMonster �� �Ҵ��ص� �Ǵ°�?
    // ������ ���� ��
    public int rescue;

    // ���� ���̱�
    public bool kill;

    // �ǰ� Ƚ��
    public int attacked_Cnt;
    
    public Quest[] quests;
    
    private void Awake()
    {
        Debug.Log("StageManager_Awake");
        if (instance == null)
        {
            instance = this;
            ui_Dist = GUIManager.instance.ui_Dist;
            //quests = new Quest[3];
            stage = GameManager.instance.GetStageData();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Boss ü�� ����
        go_Boss.GetComponent<BossMonster>().HP = stage.bossHP;

        quests = new Quest[3];

        // Quest �޾ƿ��� //
        quests[0] = QuestManager.instance.GetQuest(0);
        quests[1] = QuestManager.instance.GetQuest(1);
        quests[2] = QuestManager.instance.GetQuest(4);
    }
    // DB �� ���� Quest ���� �о����.
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
                case E_Quest.Run:
                    if(ui_Dist.percent >= quests[i].value) clear_Cnt++;
                    break;

                case E_Quest.HP: // Boss Kill ����
                    float rat = go_Player.GetComponent<PlayerStatus>().HP / GameManager.instance.status.hp;
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
        stage.Update_Info(clear_Cnt, ui_Dist.percent, kill);
        GUIManager.instance.Event_ShowResult(stage.getStar,reward_Gold, kill);
    }
}
