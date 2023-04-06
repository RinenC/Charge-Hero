using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHP : MonoBehaviour
{
   BossMonster bossMonster;
   public RectTransform rect_BossHP;
   public Text txt_HP;
    float maxHP;
    float maxWidth;
    private void Start()
    {
        //
    }
    public void Set()
   {
        bossMonster = StageManager.instance.go_Boss.GetComponent<BossMonster>();
        maxHP = bossMonster.HP;
        txt_HP.text = string.Format("{0:F0} / {1:F0}", maxHP, maxHP);
        //rect_BossHP

    }
    public IEnumerator HPUI(float target)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float current = maxHP;
        float maxWidth = rect_BossHP.sizeDelta.x;

        float offset = (target - current) / duration; // 
        while (current > target)
        {
            current += offset * Time.deltaTime;
            float rat = current / maxHP;
            if (current <= 0) current = 0;
            txt_HP.text = string.Format("{0:F0} / {1:F0}", current, maxHP);
            rect_BossHP.sizeDelta = new Vector2(maxWidth * rat, rect_BossHP.sizeDelta.y);
            yield return null;
        }
    }
}
