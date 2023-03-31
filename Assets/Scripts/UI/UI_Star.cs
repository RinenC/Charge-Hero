using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Star : MonoBehaviour
{
    public GameObject[] Stars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowStar(int cnt)
    {
        //Debug.Log("ShowStar" + cnt);
        for (int i = 0; i < 3; i++)
        {
            if (i < cnt) Stars[i].SetActive(true);
            else Stars[i].SetActive(false);
        }
    }
}
