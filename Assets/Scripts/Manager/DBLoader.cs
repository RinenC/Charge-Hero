using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DBLoader : MonoSingleton<DBLoader>
{
    private const string jsonHPEnhanceFilePath = "HPEnhance";
    private const string jsonAtkEnhanceFilePath = "AtkEnhance";
    private const string jsonDefEnhanceFilePath = "DefEnhance";
    private const string jsonStageDBFilePath = "StageDB";
    private const string jsonQuestDBFilePath = "QuestDB";

    static string UserInfoFilePath;

    public Sprite healitem;
    public Sprite damageitem;
    public Sprite invincibleitem;
    public Sprite aviationitem;
    public Sprite changeCoinitem;
    public Sprite protruding;
    public Sprite patrol;

    [Header("DB 불러올 정보칸")]
    public Enhance[] hpenhance;
    public Enhance[] atkenhance;
    public Enhance[] defenhance;
    public Stage[] stagedb;
    public Quest[] questdb;

    [Header("세이브 공간")]
    public Stage[] savedata;

    void Awake()
    {
        m_sSaveFileDirectory = Application.persistentDataPath + "/Save/";
        UserInfoFilePath = m_sSaveFileDirectory + "/" + m_sSaveFileName;

        CreateDirectory();

        var jsonHPEnhanceFile = Resources.Load<TextAsset>("Database/HPEnhance");
        hpenhance = JsonConvert.DeserializeObject<Enhance[]>(jsonHPEnhanceFile.ToString());

        var jsonAtkEnhanceFile = Resources.Load<TextAsset>("Database/AtkEnhance");
        atkenhance = JsonConvert.DeserializeObject<Enhance[]>(jsonAtkEnhanceFile.ToString());

        var jsonDefEnhanceFile = Resources.Load<TextAsset>("Database/DefEnhance");
        defenhance = JsonConvert.DeserializeObject<Enhance[]>(jsonDefEnhanceFile.ToString());

        var jsonStageDBFile = Resources.Load<TextAsset>("Database/StageDB");
        stagedb = JsonConvert.DeserializeObject<Stage[]>(jsonStageDBFile.ToString());

        var jsonQuestDBFile = Resources.Load<TextAsset>("Database/QuestDB");
        questdb = JsonConvert.DeserializeObject<Quest[]>(jsonQuestDBFile.ToString());

        LoadSprite();
    }

    void Start()
    {
        //LoadSprite();
    }

    // Quest { Type, Title, info, value }
    public void LoadSprite()
    {
        healitem = Resources.Load<Sprite>("Image/HP Item");
        damageitem = Resources.Load<Sprite>("Image/PowerUpItem");
        invincibleitem = Resources.Load<Sprite>("Image/Invincible");
        aviationitem = Resources.Load<Sprite>("Image/flight");
        changeCoinitem = Resources.Load<Sprite>("Image/ChangeCoin");
        protruding = Resources.Load<Sprite>("Image/Frog");
        patrol = Resources.Load<Sprite>("Image/Dino");
    }

    #region 저장불러오기
    [Header("저장파일")]
    public string m_sSaveFileDirectory;  // 저장할 폴더 경로
    public string m_sSaveFileName = "GameData.json"; // 파일 이름

    public void CreateDirectory()
    {
        if (!Directory.Exists(m_sSaveFileDirectory)) // 해당 경로가 존재하지 않는다면
            Directory.CreateDirectory(m_sSaveFileDirectory); // 폴더 생성(경로 생성)
    }

    //public void _reset()
    //{
    //    //string jdata = JsonConvert.SerializeObject(gData, Formatting.Indented);
    //    //File.WriteAllText(Application.persistentDataPath + "/czSaveData.json", jdata);
    //    string filecheck = m_sSaveFileDirectory + "/" + m_sSaveFileName;
    //    File.Delete(filecheck);
    //}

    [Button]
    public void SaveTest()
    {
        SaveUserInfo(GameManager.instance.status, GameManager.instance.stages);
    }

    [Button]
    public void LoadTest()
    {
        var userSaveData = LoadUserData();
        Debug.Log(JsonConvert.SerializeObject(userSaveData, Formatting.Indented));

        if(userSaveData != null) SetUserData(userSaveData);
    }

    [Button]
    public void LoadSoundTest()
    {
        var userSaveData = LoadUserData();
        Debug.Log(JsonConvert.SerializeObject(userSaveData, Formatting.Indented));

        if (userSaveData != null) SetSoundData(userSaveData);
    }

    public void SaveUserInfo(Status userStatus, params Stage[] stages)
    {
        var userInfo = new SaveInfo();

        #region GameManager
        userInfo.n_Gold = GameManager.instance.n_Gold;
        userInfo.ply_Chapter = GameManager.instance.ply_Chapter;
        userInfo.ply_Stage = GameManager.instance.ply_Stage;
        #endregion

        #region SoundManager
        userInfo.masterVolume = SoundManager.instance.MasterSlider.value;
        userInfo.bgmVolume = SoundManager.instance.BGM_Slider.value;
        userInfo.effectVolume = SoundManager.instance.EffectSlider.value;
        #endregion

        #region Status
        userInfo.Lv_HP = userStatus.Lv_HP;
        userInfo.Lv_ATK = userStatus.Lv_ATK;
        userInfo.Lv_DEF = userStatus.Lv_DEF;
        userInfo.hp = userStatus.hp;
        userInfo.atk = userStatus.atk;
        userInfo.def_cnt = userStatus.def_cnt;
        #endregion

        #region Stage

        //userInfo.userStageInfoList = GameManager.instance.stages.Select(x =>
        //{
        //    var userStageInfo = new SaveInfo.UserStageInfo();
        //    userStageInfo.getStar = x.getStar;
        //    userStageInfo.percent = x.percent;
        //    userStageInfo.isClear = x.isClear;

        //    return userStageInfo;
        //}).ToList();

        foreach (var stage in stages)
        {
            var userStageInfo = new SaveInfo.UserStageInfo();
            userStageInfo.idx = stage.idx;
            userStageInfo.getStar = stage.getStar;
            userStageInfo.percent = stage.percent;
            userStageInfo.isClear = stage.isClear;
            userStageInfo.repeat = stage.repeat;

            userInfo.userStageInfoList.Add(userStageInfo);
        }
        #endregion

        string jdata = JsonConvert.SerializeObject(userInfo, Formatting.Indented);
        File.WriteAllText(UserInfoFilePath, jdata);
    }

    public SaveInfo LoadUserData()
    {
        Debug.Log("DBLoader : SaveData Load");
        //JObject
        //Debug.Log(File.Exists(filecheck) +"   " + jdata);

        if (!File.Exists(UserInfoFilePath))
        {
            Debug.LogError("세이브 파일 없음.");
            return null;
        }

        Debug.Log("DBLoader : Checking Files . . .");

        string jdata = File.ReadAllText(UserInfoFilePath);
        var userInfo = JsonConvert.DeserializeObject<SaveInfo>(jdata);
        //GameManager.instance.getSaveLoad().gData = gData;
        //Debug.Log("파일 불러오기");

        Debug.Log("DBLoader : Loading Complete");

        return userInfo;
    }

    public void SetUserData(SaveInfo userSaveData)
    {
        GameManager.instance.status.Lv_HP = userSaveData.Lv_HP;
        GameManager.instance.status.Lv_ATK = userSaveData.Lv_ATK;
        GameManager.instance.status.Lv_DEF = userSaveData.Lv_DEF;
        GameManager.instance.status.hp = userSaveData.hp;
        GameManager.instance.status.atk = userSaveData.atk;
        GameManager.instance.status.def_cnt = userSaveData.def_cnt;
        GameManager.instance.n_Gold = userSaveData.n_Gold;
        GameManager.instance.ply_Chapter = userSaveData.ply_Chapter;
        GameManager.instance.ply_Stage = userSaveData.ply_Stage;

        //userInfo.userStageInfoList = GameManager.instance.stages.Select
        for (int i = 0; i < userSaveData.userStageInfoList.Count; i++)
        {
            var userStageInfo = userSaveData.userStageInfoList[i];
            var tmp = Array.Find(GameManager.instance.stages, x => x.idx == userStageInfo.idx);
            if (tmp == null)
                continue;
            Debug.Log($"Set {i}");
            tmp.getStar = userStageInfo.getStar;
            tmp.percent = userStageInfo.percent;
            tmp.isClear = userStageInfo.isClear;
            tmp.repeat = userStageInfo.repeat;
        }
    }

    public void SetSoundData(SaveInfo userSaveData)
    {
        SoundManager.instance.MasterSlider.value = userSaveData.masterVolume;
        SoundManager.instance.BGM_Slider.value = userSaveData.bgmVolume;
        SoundManager.instance.EffectSlider.value = userSaveData.effectVolume;
    }

    [System.Serializable]
    public class SaveInfo
    {
        public class UserStageInfo
        {
            public int idx;
            public int getStar;
            public float percent;
            public bool isClear;
            public bool repeat;
        }

        #region GameManager
        public int n_Gold;
        public int ply_Chapter;
        public int ply_Stage;
        #endregion

        #region SoundManager
        public float masterVolume;
        public float bgmVolume;
        public float effectVolume;
        #endregion

        #region Status
        public int Lv_HP;
        public int Lv_ATK;
        public int Lv_DEF;
        public int hp;
        public int atk;
        public int def_cnt;
        #endregion

        #region Stage
        public List<UserStageInfo> userStageInfoList = new List<UserStageInfo>();
        #endregion
    }
    #endregion
}
