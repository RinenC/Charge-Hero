using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Block : MonoBehaviour
{
    // Stage //
    // 별 갯수 //
    // enum { Chapter, Stage }
    // 진행률 //
    public GameObject go_Block;
    Button my_Btn;
    public UI_Star ui_Stars;
    public Stage stage;
    public Text txt_Percent;
    public Text txt_Stage;

    public Color[] colors;
    // public Text txt_Percent;
    // public G
    // Start is called before the first frame update
    private void Awake()
    {
        my_Btn = GetComponent<Button>();
        my_Btn.enabled = false;
    }
    public void Event_Able()
    {
        //Debug.Log(this.gameObject.name);
        go_Block.SetActive(false);
        my_Btn.enabled = true;
    }
    public void Event_Unable() // Init
    {
        go_Block.SetActive(true);
        my_Btn.enabled = false;
    }
    public bool SetData(int idx)
    {
        try
        {
            stage = GameManager.instance.stages[idx];
            if (txt_Stage) txt_Stage.text =
                    string.Format($"<b><color=white>{stage.name}</color></b>");
            ui_Stars.ShowStar(stage.getStar);
            SetPercentText();
            return true;
        }
        catch
        {
            Debug.Log("아직 구현되지 않은 지역 입니다.");
            return false;
        }
    }
    void SetPercentText()
    {
        float percent = stage.percent;
        Color color;
        if (percent < 0.5) color = colors[0];
        else if (percent < 0.8) color = colors[1];
        else if (percent < 0.9) color = colors[2];
        else if (percent < 1.0) color = colors[3];
        else color = colors[4];
        txt_Percent.color = color;
        txt_Percent.text = string.Format("{0:#0} %", percent * 100);
    }
}
