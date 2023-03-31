using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BlockManager : MonoBehaviour
{// UI_StageManager.
    public UI_Block[] list_ChildUI;
    // Start is called before the first frame update
    private void Awake()
    {
        list_ChildUI = GetComponentsInChildren<UI_Block>();
    }
    void Start()
    {
        //list_ChildUI = GetComponentsInChildren<UI_Block>();
    }
    public void Activate_To(int num)
    {
        Deactivate_ALL();
        for (int i = 0; i < list_ChildUI.Length; i++)
        {
            if (i >= num) return;
            list_ChildUI[i].Event_Able();
        }
    }
    public void LinkData(int chapter)
    {
        for (int i =0; i<list_ChildUI.Length; i++)
        {
            int idx = GameManager.instance.GetIndex(chapter, i + 1);
            //Debug.Log("LinkData" + idx);
            list_ChildUI[i].SetData(idx);
        }
    }
    public void Activate_ALL()
    {
        for (int i = 0; i < list_ChildUI.Length; i++)
        {
            list_ChildUI[i].Event_Able();
        }
    }
    void Deactivate_ALL()
    {
        for (int i = 0; i < list_ChildUI.Length; i++)
        {
            list_ChildUI[i].Event_Unable();
        }
    }
}
