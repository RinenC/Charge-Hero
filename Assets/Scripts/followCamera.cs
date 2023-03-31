using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    public GameObject go_Player;
    public Vector3 v_offset;
    public UI_SkillScene skill_Scene;
    public float m_fZoom;
    public float m_fZoomOut;
    Camera cam;
    public float deltahieght = 2;
    public Vector3 v_Goal;
    public Vector3 v_Origin;
    public enum E_type { normal, closeup, delay, closedown, set, final }// IDLE 
    public E_type type;
    public float m_fSpeed;
    Vector3 v_moveDir;
    public float fDist;
    public float ss;
    //Vector3 v_targetPosition;
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("follow Camera Awake");
        GameManager.instance.followCam = this;
        //go_Player = GameManager.instance.go_Player;
    }
    void Start()
    {
        Debug.Log("follow Camera Start");
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            ChangeCamType(E_type.final);

        Vector3 v_pos;
        switch (type)
        {
            case E_type.normal:
                v_pos = go_Player.transform.position + v_offset;
                v_pos.y = 0;
                transform.position = v_pos;
                break;
            case E_type.closeup:
                // 현재 위치에서 캐릭터의 x,y 위치 까지 이동 및 z -2 까지 이동.
                //if ((transform.position - v_Goal).magnitude >= 0.3f)
                //{
                //    Debug.Log("Dist:" + (transform.position - v_Goal).magnitude);
                //    transform.position += v_moveDir * m_fSpeed * 1.3f * Time.deltaTime;
                //    cam.orthographicSize -= m_fZoom * 1.2f * Time.deltaTime;

                //}
                //else if ((transform.position - v_Goal).magnitude < 0.3f
                //    && go_Player.transform.position.y >= go_Player.GetComponent<PlayerControl>().max_Height)
                //{
                //    skill_Scene.OnEventSkillScene();
                //    ChangeCamType(E_type.delay);
                //}
                cam.orthographicSize -= m_fZoom * 1.2f * Time.deltaTime;
                if(cam.orthographicSize <= 4.0f)
                {
                    cam.orthographicSize = 4.0f;
                    //skill_Scene.OnEventSkillScene();
                    GUIManager.instance.Event_ShowSkill();
                    ChangeCamType(E_type.delay);
                }
                break;
            case E_type.delay:

                break;
            case E_type.closedown:
                if ((transform.position - v_Origin).magnitude >= 0.2f)
                {
                    //Debug.Log("(transform.position - v_Goal).magnitude> " + (transform.position - v_Origin).magnitude);
                    //Debug.Log("Sqrt(29):" + Mathf.Sqrt(29) + ", Dist:" + (transform.position - go_Player.transform.position).magnitude);
                    //transform.position += v_moveDir * m_fSpeed / m_fZoomOut * Time.deltaTime;
                    cam.orthographicSize += m_fZoom * 1.2f *Time.deltaTime;
                    if (cam.orthographicSize >= 5.0f)
                    {
                        cam.orthographicSize = 5.0f;
                    }
                    //Debug.Log("orthographicSize" + cam.orthographicSize);
                    //Debug.Log("sensorSize" + cam.sensorSize);
                }
                else
                {
                    transform.position = v_Origin;

                    //ChangeCamType(E_type.final); // 플레이어가 바닥에 닿으면 
                    //ChangeCamType(E_type.final);
                    //cam.orthographicSize = 5;
                    //ChangeCamType(E_type.normal);
                }
                break;
            case E_type.set:
                // if() 거리 == "값" --> ChangeType(normal)
                transform.position += Vector3.right * m_fSpeed * Time.deltaTime;
                if (transform.position.x - go_Player.transform.position.x >= 9) ChangeCamType(E_type.normal);
                break;
            case E_type.final:
                if (Mathf.Abs(transform.position.x - go_Player.transform.position.x) >= fDist)
                {
                    Debug.Log("Dist: " + (transform.position - go_Player.transform.position).magnitude);
                    transform.position += v_moveDir * m_fSpeed * Time.deltaTime;
                }
                if (cam.orthographicSize >= 2.0f)
                    cam.orthographicSize -= m_fZoom * ss * Time.deltaTime;
                break;
        }
    }
    public void ChangeCamType(E_type type)
    {
        this.type = type;
        //Debug.Log("Change Camera Type : " + type);
        switch (type)
        {
            case E_type.closeup:
                //v_Origin = transform.position;
                //float t = go_Player.transform.position.x;
                //float s = go_Player.GetComponent<PlayerControl>().max_Height / go_Player.GetComponent<PlayerControl>().deltaY;
                //t = t + go_Player.GetComponent<PlayerControl>().f_Speed * s * Time.deltaTime;//0.5, // 1.1
                ////Debug.Log("Tims(s):" + s + "Goal.x:" + t);//-5.5
                //float height = go_Player.GetComponent<PlayerControl>().max_Height + deltahieght;
                //v_Goal = new Vector3(-5.5f, height, -5);// 10.0f
                //v_moveDir = v_Goal - transform.position;
                //v_moveDir.Normalize();
                break;
            case E_type.closedown:
                //go_Player.GetComponent<Control>().ChangeCharacterState(Control.E_STATE.Attack);
                go_Player.GetComponent<PlayerControl>().ChangeState(PlayerControl.E_State.Attack);
                v_moveDir = v_Origin - transform.position;// - v_Origin;
                v_moveDir.Normalize();
                break;
            case E_type.delay:

                break;
            case E_type.final:
                Vector3 v = new Vector3(go_Player.transform.position.x, 1.7f, go_Player.transform.position.z);
                v_moveDir = v - transform.position;// transform.position;
                v_moveDir.z = 0;
                v_moveDir.Normalize();
                break;
        }
    }
}
