using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Distance : MonoBehaviour
{
    public GameObject go_Player;
    public GameObject go_Boss;
    public GameObject go_playerImg;
    public GameObject go_bossImg;
    public float multy;//배율
    //public float dist;
    public float max_Dist;
    public float move_Dist;
    RectTransform rectTr;
    public GameObject go_Dist; //rectTransform 변경
    
    public float percent { get { return move_Dist / max_Dist; } }
    // bool ready;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("UI_Dist Start");
        //go_Player = GameManager.instance.go_Player;
        //go_Boss = GameManager.instance.go_Boss;
        
        //rectTr = go_playerImg.GetComponent<RectTransform>();
        //Vector3 v_Player = go_Player.transform.position;
        //Vector3 v_Boss = go_Boss.transform.position;

        //Vector3 v_dist = v_Player - v_Boss;
        //max_Dist = v_dist.magnitude;

        //multy = 2000 / max_Dist;
    }
    public void Set()
    {
        move_Dist = 0;
        go_Player = StageManager.instance.go_Player;
        go_Boss = StageManager.instance.go_Boss;

        rectTr = go_playerImg.GetComponent<RectTransform>();
        float v_Player = go_Player.transform.position.x;
        float v_Boss = go_Boss.transform.position.x;

        float v_dist = v_Boss - v_Player;
        max_Dist = v_dist;

        multy = 2000 / max_Dist;
        //ready = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            float v_Player = go_Player.transform.position.x;
            float v_Boss = go_Boss.transform.position.x;

            float dist = v_Boss- v_Player;// v_dist.magnitude;
            move_Dist = max_Dist - dist;  // s 플레이어 이동 거리

            rectTr = go_playerImg.GetComponent<RectTransform>();//.localPosition += Vector3.right;// * Time.deltaTime;
            float x = move_Dist * multy;
            float rat = x / 2000;
            RectTransform temp = go_Dist.GetComponent<RectTransform>();
            temp.sizeDelta = new Vector2(x + 50, temp.sizeDelta.y);
            if (temp.sizeDelta.x >= 2000) temp.sizeDelta = new Vector2(2000, temp.sizeDelta.y);
            rectTr.anchoredPosition = new Vector2(x, rectTr.anchoredPosition.y);
            if (rectTr.anchoredPosition.x >= 1800)
            {
                rectTr.anchoredPosition = new Vector2(1800, rectTr.anchoredPosition.y);
                return;
            }
            //Debug.Log("UI_DIST_UPDATE" + rt.anchoredPosition.x);
        }
    }
}
