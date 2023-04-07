using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_Quest { BOSS, Run, Rescue, Attacked, HP }
[System.Serializable]
public struct Quest
{
    public int idx;
    public E_Quest type;
    public string title;
    public float value;
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
            Arr_Quest = DBLoader.Instance.questdb;
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
