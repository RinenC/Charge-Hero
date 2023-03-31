using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Block : MonoBehaviour
{
    // Stage //
    // º° °¹¼ö //
    // ÁøÇà·ü //
    public GameObject go_Block;
    Button my_Btn;
    public UI_Star ui_Stars;
    public Stage stage;
    public Text txt_Percent;
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
    public void SetData(int idx)
    {
        stage = GameManager.instance.stages[idx];
        ui_Stars.ShowStar(stage.getStar);
        txt_Percent.text = string.Format("{0:#0}%", stage.percent * 100);
    }
}
