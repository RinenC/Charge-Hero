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
    public enum E_Scene { TITLE, CHAPTER, STAGE, PREPARE, PLAY }
    public E_Scene scene;
    public List<GameObject> list_Scene;
    public enum E_Window { Setting_Title, Setting_Play, Skill, Clear, Fail, GameOver, Pause }
    public List<GameObject> list_Window;
    public UI_BlockManager[] ui_block_Managers;
    public UI_Distance ui_Dist;
    [Header("GameUI")]
    //public TMP_Text damage;
    public TMP_Text rescue;
    //public TMP_Text gold;
    public Button jumpBtn;
    public Button slideBtn;

    [Header("PlayerUI")]
    public GameObject hpLayout;
    public GameObject heart;

    public Text txt_Gold;
    public float f_delay;
    public Text txt_Map;
    public Text txt_Atk;
    
    [Header("List")]
    public List<GameObject> hpList = new List<GameObject>();
    // Setting / Scene / Result

    #region Main
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitScene();
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
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
        //HPBarInit();
    }

    // Update is called once per frame
    void Update()
    {
        //SceneUpdate();
        UIUpdate();
    }
    void ShowScene()
    {
        for (int i = 0; i < list_Scene.Count; i++)
        {
            if ((int)scene == i)
                list_Scene[i].SetActive(true);
            else
                list_Scene[i].SetActive(false);
        }
    }
    void SceneChange(E_Scene scene)
    {
        this.scene = scene;
        switch (scene)
        {
            case E_Scene.CHAPTER:
                int chap = GameManager.instance.ply_Chapter;
                ui_block_Managers[0].Activate_To(chap);
                break;
            case E_Scene.STAGE:
                ui_block_Managers[1].LinkData(GameManager.instance.chapter);
                if (GameManager.instance.ply_Chapter > GameManager.instance.chapter) ui_block_Managers[1].Activate_ALL();
                else ui_block_Managers[1].Activate_To(GameManager.instance.ply_Stage);
                break;
            case E_Scene.PREPARE:
                string format = string.Format("{0:#,##0} G", GameManager.instance.n_Gold);
                txt_Gold.text = format;
                break;
            case E_Scene.PLAY:
                txt_Map.text = GameManager.instance.map;
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
        //DontDestroyOnLoad(GameManager.instance.gameObject);
    }
    public void Event_Upgrade(int type)
    {
        GameManager.instance.UpgradeStatus(type);
    }
    public void Event_Back()
    {
        int num = (int)scene;
        num--;
        SceneChange((E_Scene)num);
    }
    public void Event_GoTo(int num)
    {
        SceneChange((E_Scene)num);
        Event_Quit_Window();
        //Event_OffResult();
    }
    public void Event_Next()
    {
        int num = (int)scene;
        num++;
        SceneChange((E_Scene)num);
    }
    public void Event_NextStage()
    {
        SceneManager.LoadScene(GameManager.instance.map);
        //Event_OffResult();
        Event_Quit_Window();
        //Event_Next();
    }
    public void Event_ShowSetting()
    {
        list_Window[0].SetActive(!list_Window[0].activeSelf);
    }
    public void Event_PauseSetting()
    {
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
    public void Event_ShowResult(int cnt, bool clear = true)// bool clear = true / false
    {
        switch (clear)
        {
            case true:
                StartCoroutine(ShowResultWindow((int)E_Window.Clear, cnt));
                break;
            case false:
                list_Window[(int)E_Window.GameOver].SetActive(true);
                StartCoroutine(ShowResultWindow((int)E_Window.Fail, cnt));
                break;
        }
    }
    IEnumerator ShowResultWindow(int idx, int cnt)
    {
        yield return new WaitForSeconds(1);
        list_Window[idx].SetActive(true);
        list_Window[idx].GetComponent<UI_Result>().Set(cnt);
    }
    public void Event_Quit_Window()
    {
        //GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        //GameObject go_Parent = selectedObject.transform.parent.gameObject;
        //go_Parent.SetActive(false);
        for(int i = 0; i < list_Window.Count; i++)
        {
            list_Window[i].SetActive(false);
        }
        Time.timeScale = 1f;
    }
    public void Event_OffResult()
    {
        list_Window[2].SetActive(false);
    }
    public void Event_StopGame()
    {
        int idx = (int)E_Window.Pause;
        list_Window[idx].SetActive(!list_Window[idx].activeSelf);
        if (list_Window[idx].activeSelf) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    //public void HPBarInit()
    //{
    //    for (int i = 0; i < GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP; i++)
    //    {
    //        var prefabHP = Instantiate(heart, hpLayout.transform);
    //        hpList.Add(prefabHP);
    //    }
    //}

    public void UIUpdate()
    {
        if (GameManager.instance.go_Player)
        {
            HPBar();
            //RescueUI();
        }
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
    public void HPBar()
    {
        if (GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP < hpList.Count) //데미지 받았을 때
        {
            for (int i = 0; i < hpList.Count - GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP; i++)
            {
                var prefab = hpList[i];
                hpList.Remove(hpList[i]);
                Destroy(prefab);
            }
        }

        else if (GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP > hpList.Count) //체력 회복했을 때
        {
            for (int i = 0; i < GameManager.instance.go_Player.GetComponent<PlayerStatus>().HP - hpList.Count; i++)
            {
                var prefabHP = Instantiate(heart, hpLayout.transform);
                hpList.Add(prefabHP);
            }
        }
    }

    public void DamageUI()
    {
        //damage.text = ($"Damage : {GameManager.instance.go_Player.GetComponent<PlayerStatus>().ATK}");
    }

    public void RescueUI()
    {
        //rescue.text = ($"Rescue : {GameManager.instance.rescue}");
    }

    public void GoldUI()
    {
        //gold.text = ($"Gold : {GameManager.instance.n_Gold}");
    }
    #endregion

    #region Item
    #endregion
}
