using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct Status
{
    public int Lv_HP;
    public int Lv_ATK;
    public int Lv_DEF;
    public int hp;
    public int atk;
    public int def_cnt;
}
[Serializable]
public struct Enhance
{
    public int stat;
    public int need_Gold;
    public void Set(int stat, int gold)
    {
        this.stat = stat;
        this.need_Gold = gold;
    }
}
[Serializable]
public class Chapter
{
    int stars;
    int need_Cnt = 10;
    public int total
    {
        get { for (int i = 0; i < stages.Count; i++) stars += stages[i].getStar; return stars; }
    }
    public bool isEnough { get { return (total >= need_Cnt); } }
    public List<Stage> stages;
    public void Link_with(Stage stage)
    {
        if(stages == null) stages = new List<Stage>();
        stages.Add(stage);
        //Debug.Log("Link_with . . ." + stage.name);
    }
}

// chapter cnt 에 대한 변수 Define
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
    // 생성자를 만들자.
    public void Init(int idx, string stage, int getStar, float per, bool clear, int gold, int HP, int questType)
    {
        this.idx = idx;
        this.getStar = getStar;
        this.percent = per;
        this.isClear = clear;
        this.first_Gold = gold;
        this.bossHP= HP;
        this.questType = questType;

        this.name = stage;
        // idx 로 계산해서 할당하는 것 보다
        // 그냥 String 값을 읽어 오는게 더 좋을까?
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
    [Header("바로가기")]
    public followCamera followCam;
    public GameObject player_prefab;
    public GameObject go_Player;
    public GameObject go_Boss;
    
    [Header("플레이어 기본 능력치")]
    public Status status;
    public int n_Gold;
    public bool changeCoin;

    public Enhance[] en_HP;
    public Enhance[] en_ATK;
    public Enhance[] en_DEF;
    // 강화를 누른다 > 능력치 종류 확인한다 > 플레이어의 해당 능력치 Lv 을 호출한다.
    // Lv(idx) 값에 따른 비용(gold) 및 수치(stat)을 가져온다.

    [Header("맵")]
    public Chapter[] chapters;
    public Stage[] stages;  // List 변경
    public int ply_Chapter; // clear Chapter?
    public int ply_Stage; // clear Stage?
    public int chapter;
    public int stage;
    //public UI_Distance ui_dist;
    public string map
    {
        get { return string.Format($"{chapter}-{stage}"); }
    }

    // inline 함수 //
    public Func<int, int, int> GetIndex = (ch, stg) => ((ch - 1) * 5) + (stg - 1);
    //public static Func<int, int, int> TestFunc = (ch, stg) => ((ch - 1) * 5) + (stg - 1);

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("GameManager_Awake");
            instance = this;
            player_prefab = Resources.Load<GameObject>("Prefabs/Player");
            
            Init_StageInfo();
            Init_EnhanceInfo();
            // 플레이어 정보 읽어오기

            SceneManager.sceneLoaded += LoadScene;
            //SceneManager.sceneUnloaded += UnLoadScene;

            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    void Init_StageInfo()
    {
        // 파일을 읽어온다.
        // 몇 줄인지 확인한다.
        // stages = new Stage[lines];
        stages = new Stage[5];
        // 데이터를 할당한다.
        stages[0] = new Stage();
        stages[0].Init(0, "1-1", 0, 0, false, 2000, 500, 1);
        
        stages[1] = new Stage();
        stages[1].Init(1, "1-2", 0, 0, false, 2200, 1000, 1);
        
        stages[2] = new Stage();
        stages[2].Init(2, "1-3", 0, 0, false, 2400, 1500, 1);
        
        stages[3] = new Stage();
        stages[3].Init(3, "1-4", 0, 0, false, 2600, 2000, 1);
        
        stages[4] = new Stage();
        stages[4].Init(4, "1-5", 0, 0, false, 2800, 2500, 1);

        chapters = new Chapter[1];//3
        for(int i =0; i < chapters.Length; i++)
        {
            chapters[i] = new Chapter();
            for (int j = 0; j < 5; j++)
            {
                chapters[i].Link_with(stages[i * 5 + j]);
            }
        }
    }
    void Init_EnhanceInfo()
    {
        // HP //
        en_HP = new Enhance[5];
        en_HP[0].Set(1, 2000);
        en_HP[1].Set(1, 4000);
        en_HP[2].Set(1, 6000);
        en_HP[3].Set(1, 8000);
        en_HP[4].Set(1, 10000);
        // ATK //
        en_ATK = new Enhance[5];
        en_ATK[0].Set(100, 200);
        en_ATK[1].Set(100, 350);
        en_ATK[2].Set(200, 550);
        en_ATK[3].Set(200, 850);
        en_ATK[4].Set(200, 1000);
        // DEF //
        en_DEF = new Enhance[5];
        en_DEF[0].Set(1, 5000);
        en_DEF[1].Set(1, 10000);
        en_DEF[2].Set(1, 15000);
        en_DEF[3].Set(1, 20000);
        en_DEF[4].Set(1, 25000);
    }
    public Enhance GetEnhanceInfo(E_Status type)
    {
        Enhance en = new Enhance();
        en.Set(0, 0);
        switch (type)
        {
            case E_Status.HP:
                if(status.Lv_HP < en_HP.Length)
                    en = en_HP[status.Lv_HP];
                break;
            case E_Status.ATK:
                if (status.Lv_ATK < en_ATK.Length)
                    en = en_ATK[status.Lv_ATK];
                break;
            case E_Status.DEF:
                if (status.Lv_DEF < en_DEF.Length)
                    en = en_DEF[status.Lv_DEF];
                break;
        }
        return en;
    }
    public string GetStatusInfo(E_Status type)
    {
        string info = null;
        switch (type)
        {
            case E_Status.HP:
                info = string.Format($"Lv.{status.Lv_HP} HP {status.hp}");
                break;
            case E_Status.ATK:
                info = string.Format($"Lv.{status.Lv_ATK} ATK {status.atk}");
                break;
            case E_Status.DEF:
                info = string.Format($"Lv.{status.Lv_DEF} DEF {status.def_cnt}");
                break;
        }
        return info;
    }
    private void Start()
    {
        Debug.Log("GameManager_Start");
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
        Debug.Log(scene.name + "를 Load 합니다.");
        switch (scene.name)//Load 하는 Scene 이름.
        {
            case "UI":
                //plusStat.Init();
                break;
            default:
                SetPlayer();// 굳이 동적 할당 해야할까? //
                StageManager.instance.Set();
                break;
        }
    }
    //public void UnLoadScene(Scene scene)//, Scene nextScene)
    //{
    //    //Debug.Log(scene.name + "를 UnLoad 합니다.");
    //    switch (scene.name)//Load 하는 Scene 이름.
    //    {
    //        case "UI":

    //            break;
    //        default:

    //            break;
    //    }
    //}
    public void SetPlayer()
    {
        // Player Prefab 생성 및 배치.
        go_Player = Instantiate(player_prefab);
        Debug.Log("Creat Player in " + SceneManager.GetActiveScene().name);
        followCam.go_Player = go_Player;
        go_Player.GetComponent<PlayerControl>().cam = followCam;

        // 업그레이드 된 능력치 Prefab에 적용
        go_Player.GetComponent<PlayerStatus>().InitStatus(status);
    }
    public void GameEnd()
    {
        StageManager.instance.CheckQuest();
        OpenStage();
    }
    void OpenStage()
    {
        if (stage < 5)
        {
            stage++;
            if (ply_Chapter == chapter) ply_Stage = stage;
        }
        if (chapters[ply_Chapter - 1].isEnough)
        {
            ply_Chapter++;
            ply_Stage = 1;
            chapter = ply_Chapter;
        }
        //if(stage != 5 && (ply_Chapter == chapter) && stage == ply_Stage)
        //{
        //    stage++; // NextStage 버튼을 눌러도 다음 스테이지로 넘어가지 않음.
        //    ply_Stage++;
        //}
        //if (chapters[ply_Chapter - 1].isEnough) ply_Chapter++;
        // TotalStar[Chapter].isEnough --> ply_Chatper++;
    }
    public Stage GetStageData()
    {
        // map(string ) 을 int형으로 변경해서 넘긴다.
        string[] data = map.Split('-');
        int chap = int.Parse(data[0]);
        int stage = int.Parse(data[1]);
        return stages[GetIndex(chap, stage)];
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
