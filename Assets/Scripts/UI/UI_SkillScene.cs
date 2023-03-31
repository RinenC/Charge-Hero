using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
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
                    this.gameObject.SetActive(false);
                }
                break;
        }
    }
    void Test()
    {
        //switch (type)
        //{
        //    case E_Type.Move:
        //        rectTransform.transform.localPosition = new Vector3(-Screen.width, 0, 0);
        //        break;
        //    case E_Type.InOut:
        //        img.color = new Color(1, 1, 1, 0);
        //        break;
        //}

        //if (go_Scene.activeSelf)
        //{
        //    switch (type)
        //    {
        //        case E_Type.Move:
        //            switch (direction)
        //            {
        //                case 0:
        //                    rectTransform.transform.localPosition += Vector3.right * m_fSpeed * Time.deltaTime;
        //                    if (rectTransform.transform.localPosition.x >= 0)
        //                    {
        //                        rectTransform.transform.localPosition = Vector3.zero;
        //                        direction = 1;
        //                    }
        //                    break;
        //                case 1:
        //                    m_fTime += Time.deltaTime;
        //                    if (m_fTime >= m_fStopTime) direction = 2;
        //                    break;
        //                case 2:
        //                    rectTransform.transform.localPosition += Vector3.right * m_fSpeed * Time.deltaTime;
        //                    if (rectTransform.transform.localPosition.x >= Screen.width)
        //                    {
        //                        go_Scene.SetActive(false);
        //                        folCam.ChangeCamType(followCamera.E_type.closedown);
        //                    }
        //                    break;
        //            }
        //            break;
        //        case E_Type.InOut:
        //            switch (direction)
        //            {
        //                case 0:
        //                    m_fAlpha += m_fBrightSpeed * Time.deltaTime;
        //                    img.color = new Color(1, 1, 1, m_fAlpha);
        //                    if (img.color.a >= 1)
        //                    {
        //                        img.color = new Color(1, 1, 1, 1);
        //                        direction = 1;
        //                    }
        //                    break;
        //                case 1:
        //                    m_fAlpha -= m_fBrightSpeed * Time.deltaTime;
        //                    img.color = new Color(1, 1, 1, m_fAlpha);
        //                    if (img.color.a <= 0)
        //                    {
        //                        img.color = new Color(1, 1, 1, 0);
        //                        go_Scene.SetActive(false);
        //                        direction = 0;
        //                        folCam.ChangeCamType(followCamera.E_type.closedown);
        //                    }
        //                    break;
        //            }
        //            break;
        //    }
        //}
    }
    public void OnEventSkillScene()
    {
        Debug.Log("OnEventSkillScene");
        //go_Scene.SetActive(true);
    }
}
