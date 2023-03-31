using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Result : MonoBehaviour
{
    public Text Quest_Title;
    public UI_Star ui_Stars;
    public Text txt_Gold;
    public void Set(int stars, int gold)
    {
        Quest_Title.text = "";
        for (int i = 0; i < 3; i++)
        {
            Quest_Title.text += string.Format($"{i + 1}_ {StageManager.instance.quests[i].title}\n");
        }
        ui_Stars.ShowStar(stars);
        txt_Gold.text = string.Format("x {0:#,##0}", gold);
    }
}
