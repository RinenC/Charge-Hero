using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct Status
{
    public int hp;
    public int atk;
    public int def_cnt;
}
// chapter cnt �� ���� ���� Define
[Serializable]
public class Stage
{
    public int idx;
    public string name;
    public int getStar;
    public float percent;
    public bool repeat;
    public bool isClear;
    // public int clearGold
    public int first_Gold;
    public int repeat_Gold;
    public int bossHP;
    public int questType;
    // �����ڸ� ������.
    public void Init(int idx, int getStar, float per, bool clear, int gold, int HP, int questType)
    {
        this.idx = idx;
        this.getStar = getStar;
        this.percent = per;
        this.isClear = clear;
        this.first_Gold = gold;
        this.bossHP= HP;
        this.questType = questType;

        this.name = string.Format($"{(idx / 5) + 1}-{idx % 5 + 1}");
        // idx �� ����ؼ� �Ҵ��ϴ� �� ����
        // �׳� String ���� �о� ���°� �� ������?
        this.repeat_Gold = first_Gold - 1000;
    }
    public void Update_Info(int cnt, float percent, bool kill)
    {
        repeat = true;
        if (cnt > getStar) this.getStar = cnt;
        if (isClear != kill) isClear = true;
        if (kill) this.percent = 1;
        else if (percent > this.percent) this.percent = percent;     
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("�ٷΰ���")]
    public followCamera followCam;
    public GameObject player_prefab;
    public GameObject go_Player;
    public GameObject go_Boss;
    
    [Header("�÷��̾� �⺻ �ɷ�ġ")]
    public Status status;
    public int n_Gold;

    [Header("��")]
    public Stage[] stages;  // List ����
    public int ply_Chapter; // clear Chapter?
    public int ply_Stage; // clear Stage?
    public int chapter;
    public int stage;
    //public UI_Distance ui_dist;
    public string map
    {
        get { return string.Format($"{chapter}-{stage}"); }
    }
    public Func<int, int, int> GetIndex = (ch, stg) => ((ch - 1)* 5) + (stg - 1);
    //public PlusStatus plusStat = new PlusStatus();
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("GameManager_Awake");
            instance = this;
            player_prefab = Resources.Load<GameObject>("Prefabs/Player");
            Init_StageInfo();

            SceneManager.sceneLoaded += LoadScene;
            SceneManager.sceneUnloaded += UnLoadScene;

            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    void Init_StageInfo()
    {
        // ������ �о�´�.
        // �� ������ Ȯ���Ѵ�.
        // stages = new Stage[lines];
        stages = new Stage[5];
        // �����͸� �Ҵ��Ѵ�.
        stages[0] = new Stage();
        stages[0].Init(0, 0, 0, false, 2000, 500, 1);
        
        stages[1] = new Stage();
        stages[1].Init(1, 0, 0, false, 2200, 1000, 1);
        
        stages[2] = new Stage();
        stages[2].Init(2, 0, 0, false, 2400, 1500, 1);
        
        stages[3] = new Stage();
        stages[3].Init(3, 0, 0, false, 2600, 2000, 1);
        
        stages[4] = new Stage();
        stages[4].Init(4, 0, 0, false, 2800, 2500, 1);
    }
    private void Start()
    {
        Debug.Log("GameManager_Start");

        //invincibleTimer = invincibleTime;
        //aviationTimer = aviationTime;
        //magneticTimer = magneticTime;
    }
    public void UpgradeStatus(int type)
    {
        switch(type)
        {
            case 0:
                if (n_Gold >= 100)
                {
                    StartCoroutine(GUIManager.instance.NumberAnimation(n_Gold - 100, n_Gold, E_VALUE.GOLD));
                    n_Gold -= 100;
                    status.hp++;
                }
                break;
            case 1:
                status.atk++;
                break;
            case 2:
                status.def_cnt++;
                break;
        }
    }
    public void GetChapter(int _chapter)
    {
        chapter = _chapter;
    }
    public void GetStage(int _stage)
    {
        stage = _stage;
    }
    public void LoadScene(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + "�� Load �մϴ�.");
        switch (scene.name)//Load �ϴ� Scene �̸�.
        {
            case "UI":
                //plusStat.Init();
                break;
            //case "Stage":
            default:
                SetPlayer();
                //SetGame();//
                GUIManager.instance.SetUI();
                break;
        }
    }
    public void UnLoadScene(Scene scene)//, Scene nextScene)
    {
        //Debug.Log(scene.name + "�� UnLoad �մϴ�.");
        switch (scene.name)//Load �ϴ� Scene �̸�.
        {
            case "UI":

                break;
            default:
                
                break;
        }
    }
    public void SetPlayer()
    {
        // Player Prefab ���� �� ��ġ.
        go_Player = Instantiate(player_prefab);
        Debug.Log("Creat Player in " + SceneManager.GetActiveScene().name);
        followCam.go_Player = go_Player;
        go_Player.GetComponent<PlayerControl>().cam = followCam;

        // ���׷��̵� �� �ɷ�ġ Prefab�� ����
        go_Player.GetComponent<PlayerStatus>().InitStatus(status);
    }
    public void StageClear() // GameFinish GameEnd
    {
        //GUIManager.instance.Event_ShowResult();
        StageManager.instance.CheckQuest();
        OpenStage();
    }
    public void GameOver()
    {
        //Time.timeScale = 0; // --> �����ϸ� �ٽ� 1�� ���� --> �̷��� �ϴϱ� â�� �ȶ�.
        // �ٽ��ϱ� ��ư�� �ʿ��ѵ� �׳� ���ӿ����� ���â�� ���θ���°�..
        //GUIManager.instance.Event_ShowResult(false);

        //StageManager.stage.CheckQuest();
        // ��ȯ�� int ������ �޴´�.
        // stage ���� ���� �ش� stage �� �� ������ �߰� or ���� �Ѵ�.
        StageManager.instance.CheckQuest();
        OpenStage();
    }
    void OpenStage()
    {
        if(stage != 5 && (ply_Chapter == chapter) && stage == ply_Stage)
        {
            ply_Stage++;
        }
    }
    public Stage GetStageData()
    {
        // map(string ) �� int������ �����ؼ� �ѱ��.
        string[] data = map.Split('-');
        int chap = int.Parse(data[0]);
        int stage = int.Parse(data[1]);
        int idx = (chap - 1) * 5 - (stage - 1);
        return stages[idx];
    }

    //public void Invincible()
    //{
    //    if (invincible)
    //    {
    //        if (invincibleTimer > 0)
    //        {
    //            invincibleTimer -= Time.deltaTime;
    //        }
    //        if (invincibleTimer <= 0)
    //        {
    //            invincibleTimer = invincibleTime;
    //            invincible = false;
    //        }
    //    }
    //}

    //public void Aviation()
    //{
    //    if (aviation)
    //    {
    //        if (aviationTimer > 0)
    //        {
    //            aviationTimer -= Time.deltaTime;
    //        }
    //        if (aviationTimer <= 0)
    //        {
    //            aviationTimer = aviationTime;
    //            if (effected)
    //            {
    //                go_Player.GetComponent<PlayerControl>().f_Speed -= 2;
    //                effected = false;
    //            }
    //            aviation = false;
    //        }
    //    }
    //}

    //public void Magnetic()
    //{
    //    if (magnetic)
    //    {
    //        if (magneticTimer > 0)
    //        {
    //            magneticTimer -= Time.deltaTime;
    //        }
    //        if (magneticTimer <= 0)
    //        {
    //            magneticTimer = magneticTime;
    //            magnetic = false;
    //        }
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //Invincible();
        //Aviation();
    }
}
