using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public enum E_VALUE
{
    GOLD,
    ATK,
}

public class GUIManager : MonoSingleton<MonoBehaviour>
{
    public static GUIManager instance;
    public enum E_Scene { TITLE, CHAPTER, STAGE, PREPARE, PLAY } // class 밖으로 뺴내기.
    E_Scene cur_Scene;
    [Header("_화면_")]
    public List<GameObject> list_Scene;
    public List<GameObject> list_BackGround;
    public enum E_Window { Setting_Title, Setting_Play, Skill, Clear, Fail, GameOver, Pause }
    [Header("_창_")]
    public List<GameObject> list_Window;

    [Header("_챕터&스테이지 관리_")]
    public UI_BlockManager[] ui_block_Managers;
    
    [Header("_조작_")]
    public Button jumpBtn;
    public Button slideBtn;

    [Header("_플레이어_")]
    public Text txt_Atk;
    // UI_HP //
    public UI_List HP_UI;
    public UI_List HP_Back_UI;
    // UI_Shield //
    public UI_List SHIELD_UI;
    
    [Header("_편의성_")]
    public UI_Distance ui_Dist;
    // UI Buff //
    //public UI_List BUFF_UI;
    public UI_BossHP bossHP_UI;

    [Header("_강화_")]
    public Text txt_Gold;
    public UI_Enhance[] ui_Enhances;
    
    float f_delay = 0.5f; // Number Counting Animation 시간

    //public Text txt_Map; // 제거 예정 //
    
    #region Main
    private void Awake()
    {
        //SetResolutions();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitScene();
        }
        else
            Destroy(this.gameObject);
    }

    public void SetResolutions()
    {
        int setWidth = 3200; // 사용자 설정 너비
        int setHeight = 1440; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);
        // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
    


    void InitScene()
    {
        for (int i = 0; i < list_Scene.Count; i++)
        {
            //SetResolutions();
            list_Scene[i].SetActive(true);
            list_Scene[i].SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneChange(E_Scene.TITLE);
    }

    // Update is called once per frame
    void Update()
    {
        //SceneUpdate();
        //UIUpdate();
    }
    void ShowScene()
    {
        for (int i = 0; i < list_Scene.Count; i++)
        {
            if ((int)cur_Scene == i)
                list_Scene[i].SetActive(true);
            else
                list_Scene[i].SetActive(false);
        }
        ToggleBackGround(cur_Scene == E_Scene.TITLE || cur_Scene == E_Scene.PREPARE);
        //if ()
            
        //else
        //    ToggleBackGround(false);
    }
    void SceneChange(E_Scene scene)
    {
        SoundManager.instance.PlayEffect("Click");
        this.cur_Scene = scene;
        switch (scene)
        {
            case E_Scene.TITLE:
                SoundManager.instance.PlayBGM("TitleBGM");
                break;
            case E_Scene.CHAPTER:
                SoundManager.instance.PlayBGM("SceneBGM");
                //SetResolutions();
                int chap = GameManager.instance.ply_Chapter;
                ui_block_Managers[0].Activate_To(chap);
                //SetResolutions();
                break;
            case E_Scene.STAGE:
                SoundManager.instance.PlayBGM("SceneBGM");
                if (ui_block_Managers[1].LinkData(GameManager.instance.chapter))
                {
                    if (GameManager.instance.ply_Chapter > GameManager.instance.chapter) ui_block_Managers[1].Activate_ALL();
                    else ui_block_Managers[1].Activate_To(GameManager.instance.ply_Stage);
                }
                else
                    SceneChange(E_Scene.CHAPTER);
               // SetResolutions();
                break;
            case E_Scene.PREPARE:
                //SetResolutions();
                // Gold UI 초기화 //
                txt_Gold.text = string.Format("{0:#,##0} G", GameManager.instance.n_Gold);
                
                // Enhance UI 초기화 //
                for (int i = 0; i < 3; i++) ui_Enhances[i].Init();
                //SetResolutions();
                break;
            case E_Scene.PLAY:
                //SoundManager.instance.PlayBGM("PlayBGM");
                break;
        }
        ShowScene();
    }
    void ToggleBackGround(bool val)
    {
        for (int i = 0; i < list_BackGround.Count; i++)
        {
            list_BackGround[i].SetActive(val);
        }
    }
    public void SetUI()
    {
        ui_Dist.Set();
        bossHP_UI.Set();
        // To Do
        HP_UI.Init(GameManager.instance.status.hp);
        HP_Back_UI.Init(GameManager.instance.status.hp);
        // Shield UI 초기화 //
        SHIELD_UI.Init(GameManager.instance.status.def_cnt);

        txt_Atk.text = string.Format("{0:#,###}", GameManager.instance.status.atk);
    }
    public void Event_EnterStage()
    {
        SceneManager.LoadScene(GameManager.instance.map);
        Event_Next();
    }
    public void Event_Back()
    {
        int num = (int)cur_Scene;
        num--;
        SceneChange((E_Scene)num);
    }
    public void Event_GoTo(int num)
    {
        if (SceneManager.GetActiveScene().name != "UI") SceneManager.LoadScene("UI");
        SceneChange((E_Scene)num);
        Event_Quit_Window();
        //Event_OffResult();
    }
    public void Event_Next()
    {
        //SetResolutions();
        int num = (int)cur_Scene;
        num++;
        SceneChange((E_Scene)num);
        //SetResolutions();
    }
    public void Event_NextStage()
    {
        SceneManager.LoadScene(GameManager.instance.map);
        //Event_OffResult();
        SoundManager.instance.PlayEffect("Click");
        Event_Quit_Window();
        //Event_Next();
    }
    public void Event_ShowSetting()
    {
        SoundManager.instance.PlayEffect("Click");
        list_Window[0].SetActive(!list_Window[0].activeSelf);
        if (list_Window[1].activeSelf) list_Window[1].SetActive(false);
    }
    public void Event_PauseSetting()
    {
        SoundManager.instance.PlayEffect("Click");
        if (list_Window[0].activeSelf)
        {
            list_Window[0].SetActive(false);
            list_Window[1].SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            list_Window[0].SetActive(true);
            list_Window[1].SetActive(true);
            Time.timeScale = 0f;
        }

        //int idx = (int)E_Window.Setting_Play;
        //list_Window[idx].SetActive(!list_Window[idx].activeSelf);
        //if (list_Window[idx].activeSelf) Time.timeScale = 0f;
        //else Time.timeScale = 1f;
    }
    public void Event_ShowSkill()
    {
        int idx = (int)E_Window.Skill;
        list_Window[idx].SetActive(!list_Window[idx].activeSelf);
    }
    public void Event_ShowResult(int cnt, int gold, bool clear = true)// bool clear = true / false
    {
        switch (clear)
        {
            case true:
                StartCoroutine(ShowResultWindow((int)E_Window.Clear, cnt, gold));
                break;
            case false:
                list_Window[(int)E_Window.GameOver].SetActive(true);
                StartCoroutine(ShowResultWindow((int)E_Window.Fail, cnt, gold));
                break;
        }
    }
    IEnumerator ShowResultWindow(int idx, int cnt, int gold)
    {
        yield return new WaitForSeconds(1);
        if((E_Window)idx == E_Window.Clear)
        {
            SoundManager.instance.PlayEffect("Clear");
        }
        list_Window[idx].SetActive(true);
        list_Window[idx].GetComponent<UI_Result>().Set(cnt, gold);
    }
    public void Event_Quit_Window()
    {
        for(int i = 0; i < list_Window.Count; i++)
        {
            list_Window[i].SetActive(false);
        }
        Time.timeScale = 1f;
    }
    public IEnumerator NumberAnimation(float target, float current, E_VALUE type)
    {
        float duration = f_delay; // 카운팅에 걸리는 시간 설정. 

        float offset = (target - current) / duration; // 
        while (current > target)
        {
            current += offset * Time.deltaTime;
            switch(type)
            {
                case E_VALUE.GOLD:
                    txt_Gold.text = string.Format("{0:#,##0} G", (int)current);
                    break;
                case E_VALUE.ATK:
                    txt_Atk.text = string.Format("{0:#,##0}", (int)current);
                    break;
            }
            yield return null;
        }
        current = target;
        //Debug.Log("Current/Target" + current + "/" + target);
        switch (type)
        {
            case E_VALUE.GOLD:
                txt_Gold.text = string.Format("{0:#,##0} G", (int)current);
                break;
            case E_VALUE.ATK:
                txt_Atk.text = string.Format("{0:#,##0}", (int)current);
                break;
        }
    }
    //public void HPBar()
    //{
    //    if (GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP < hpList.Count) //데미지 받았을 때
    //    {
    //        for (int i = 0; i < hpList.Count - GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP; i++)
    //        {
    //            var prefab = hpList[i];
    //            hpList.Remove(hpList[i]);
    //            Destroy(prefab);
    //        }
    //    }

    //    else if (GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP > hpList.Count) //체력 회복했을 때
    //    {
    //        for (int i = 0; i < GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP - hpList.Count; i++)
    //        {
    //            var prefabHP = Instantiate(heart, hpLayout.transform);
    //            hpList.Add(prefabHP);
    //        }
    //    }
    //}
    #endregion

    #region Item
    #endregion
}
