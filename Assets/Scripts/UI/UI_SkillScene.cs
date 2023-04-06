using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillScene : MonoBehaviour
{
    //public GameObject go_Scene;
    public float m_fSpeed;
    public int direction;
    public float m_fTime;
    public float m_fStopTime;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.transform.localPosition = new Vector3(-Screen.width, 0, 0);
    }
    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case 0:
                rectTransform.transform.localPosition += Vector3.right * m_fSpeed * Time.deltaTime;
                if (rectTransform.transform.localPosition.x >= 0)
                {
                    rectTransform.transform.localPosition = Vector3.zero;
                    direction = 1;
                }
                break;
            case 1:
                m_fTime += Time.deltaTime;
                if (m_fTime >= m_fStopTime) direction = 2;
                break;
            case 2:
                rectTransform.transform.localPosition += Vector3.right * m_fSpeed * Time.deltaTime;
                if (rectTransform.transform.localPosition.x >= Screen.width)
                {
                    GameManager.instance.followCam.ChangeCamType(followCamera.E_type.closedown);
                    m_fTime = 0;
                    direction = 0;
                    this.gameObject.SetActive(false);
                    rectTransform.transform.localPosition = new Vector3(-Screen.width, 0, 0);
                }
                break;
        }
    }
}
