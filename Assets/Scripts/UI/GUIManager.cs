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
    public enum E_Scene { TITLE, CHAPTER, STAGE, PREPARE, PLAY } // class ������ ������.
    E_Scene cur_Scene;
    [Header("_ȭ��_")]
    public List<GameObject> list_Scene;
    public enum E_Window { Setting_Title, Setting_Play, Skill, Clear, Fail, GameOver, Pause }
    [Header("_â_")]
    public List<GameObject> list_Window;

    [Header("_é��&�������� ����_")]
    public UI_BlockManager[] ui_block_Managers;
    
    [Header("_����_")]
    public Button jumpBtn;
    public Button slideBtn;

    [Header("_�÷��̾�_")]
    public Text txt_Atk;
    // UI_HP //
    public UI_List HP_UI;
    // UI_Shield //
    public UI_List SHIELD_UI;
    
    [Header("_���Ǽ�_")]
    public UI_Distance ui_Dist;
    // UI Buff //
    public UI_List BUFF_UI;

    [Header("_��ȭ_")]
    public Text txt_Gold;
    public UI_Enhance[] ui_Enhances;
    
    float f_delay = 0.5f; // Number Counting Animation �ð�

    //public Text txt_Map; // ���� ���� //
    
    #region Main
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitScene();
        }
        else
            Destroy(this.gameObject);
    }
    void InitScene()
    {
        for (int i = 0; i < list_Scene.Count; i++)
        {
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
    }
    void SceneChange(E_Scene scene)
    {
        SoundManager.instance.Event_ClickSound();
        this.cur_Scene = scene;
        switch (scene)
        {
            case E_Scene.CHAPTER:
                int chap = GameManager.instance.ply_Chapter;
                ui_block_Managers[0].Activate_To(chap);
                break;
            case E_Scene.STAGE:
                if (ui_block_Managers[1].LinkData(GameManager.instance.chapter))
                {
                    if (GameManager.instance.ply_Chapter > GameManager.instance.chapter) ui_block_Managers[1].Activate_ALL();
                    else ui_block_Managers[1].Activate_To(GameManager.instance.ply_Stage);
                }
                else
                    SceneChange(E_Scene.CHAPTER);
                break;
            case E_Scene.PREPARE:
                // Gold UI �ʱ�ȭ //
                txt_Gold.text = string.Format("{0:#,##0} G", GameManager.instance.n_Gold);
                
                // Enhance UI �ʱ�ȭ //
                for (int i = 0; i < 3; i++) ui_Enhances[i].Init();
                break;
            case E_Scene.PLAY:
                // Map ���� ���� //
                //txt_Map.text = GameManager.instance.map;
                
                // HP UI �ʱ�ȭ //
                HP_UI.Init(GameManager.instance.status.hp);
                // Shield UI �ʱ�ȭ //
                SHIELD_UI.Init(GameManager.instance.status.def_cnt);

                // ATK UI �ʱ�ȭ //
                txt_Atk.text = string.Format("{0:#,###}", GameManager.instance.status.atk);
                break;
        }
        ShowScene();
    }
    public void SetUI()
    {
        ui_Dist.Set();   
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
        int num = (int)cur_Scene;
        num++;
        SceneChange((E_Scene)num);
    }
    public void Event_NextStage()
    {
        SceneManager.LoadScene(GameManager.instance.map);
        //Event_OffResult();
        SoundManager.instance.Event_ClickSound();
        Event_Quit_Window();
        //Event_Next();
    }
    public void Event_ShowSetting()
    {
        SoundManager.instance.Event_ClickSound();
        list_Window[0].SetActive(!list_Window[0].activeSelf);
    }
    public void Event_PauseSetting()
    {
        SoundManager.instance.Event_ClickSound();
        int idx = (int)E_Window.Setting_Play;
        list_Window[idx].SetActive(!list_Window[idx].activeSelf);
        if (list_Window[idx].activeSelf) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
    public void Event_ShowSkill()
    {
        int idx = (int)E_Window.Skill;
        list_Window[idx].SetActive(!list_Window[idx].activeSelf);
    }
    public void Event_ShowResult(int cnt, int gold, bool clear = true)// bool clear = true / false
    {
        SoundManager.instance.Event_ClickSound();
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
        float duration = f_delay; // ī���ÿ� �ɸ��� �ð� ����. 

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
    //    if (GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP < hpList.Count) //������ �޾��� ��
    //    {
    //        for (int i = 0; i < hpList.Count - GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP; i++)
    //        {
    //            var prefab = hpList[i];
    //            hpList.Remove(hpList[i]);
    //            Destroy(prefab);
    //        }
    //    }

    //    else if (GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP > hpList.Count) //ü�� ȸ������ ��
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
