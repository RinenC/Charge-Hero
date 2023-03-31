using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_Quest { Run, HP, Rescue, Attacked, BOSS }
[System.Serializable]
public struct Quest
{
    public E_Quest type;
    public string title;
    public float value;
    //public bool clear;

    // DB로 불러온 데이터 할당하기.
    public void Set(E_Quest type, string title, float value)
    {   // E_QuestType 나중에 int 형으로 변경
        this.type = type;// (E_Quest)questType;
        this.title = title;
        this.value = value;
    }
}
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public Quest[] Arr_Quest;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Arr_Quest = new Quest[5];

            Arr_Quest[0].Set(E_Quest.BOSS, "보스 죽이기", 0);
            Arr_Quest[1].Set(E_Quest.Run, "25% 이상 진행하기", 0.25f);
            Arr_Quest[2].Set(E_Quest.Rescue, "동료 3회 구출", 3);
            Arr_Quest[3].Set(E_Quest.Attacked, "3회 이하로 피격당하고 클리어", 3);
            Arr_Quest[4].Set(E_Quest.HP, "HP 50% 이상으로 클리어", 0.5f);
        }
        else
            Destroy(this.gameObject);
    }
    public Quest GetQuest(int idx)
    {
        return Arr_Quest[idx];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
