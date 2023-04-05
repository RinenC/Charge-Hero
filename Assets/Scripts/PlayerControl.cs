using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public enum E_State { Run, JumpUP, JumpDown, Aviation, RunUp, LastJump, Stay, Attack, End, Attacked};

    [Header("_Button_")]
    JumpButton jumpBtn;
    SlideButton slideBtn;
    bool isJumpBtnDown;
    bool isSlideBtnDown;

    [Header("_Set_")]
    public E_State state;               // 플레이어 상태
    public float GRAVITY = 3.5f;        // 기본 중력값 설정
    public float DownGraviry;           // 하강할 때 중력 값 설정
    public float y_base = -1.25f;       // 플레이어의 기본 y 값
    public float max_Height;            // 플레이어가 최대로 도달할 수 있는 높이
    public float y_offset;              // 플레이어가 충돌한 지점이 offset 아래 지점인지 확인용

    [Header("_MOVE_")]
    public float f_Speed;               // 최대속도
    public float f_Avitation_Accel_X;   // Avitaion 상태 시 가속도(x)
    public float f_Avitation_Accel_Y;   // Avitaion 상태 시 가속도(y) --> 2개나 필요할까
    float SPEED;                        // 적용되는 속도
    Vector3 v_moveDir;                  // 상/하 이동 방향
    public Collider2D[] colliders;

    [Header("_JUMP_")]
    //public float deltaY;                // LastJump 시 이동 속도
    public float[] jumpPower = new float[2];// 플레이어의 점프 가속도(1단, 2단)
    public int jumpCnt;                 // 연속 점프 수
    
    [Header("_FIND_")]
    GameObject go_Target;               // 발견한 BOSS 를 참조하기 위한 변수
    float m_fDetect_Dist = 20;        // Ray 발사 거리
    //public float m_fDetect_Height;      // Ray 발사 높이
    bool b_Find;                        // BOSS 발견 여부

    [Header("_ATTACK")]
    float f_RunUp_Dist = 5;          // 도움닫기 길이
    float dist;                         // 도움닫기 거리
    Vector3 landPosition;               // BOSS 공격 후 도착할 지점
    float f_Attacked_Power= 20;      // 공격 실패 시 튕겨나가는 힘
    float f_StartToFinishTime = 0.5f;   // 공격의 시작과 끝에 걸리는 시간. float 
    float f_Accel;                      // 공격 지점에 도달하는 속도

    [Header("_ETC_")]
    public followCamera cam;
    public Vector3 v_BackPos;           // 리스폰 지점 --> 함수로 변경
    
    float f_timer;
    Rigidbody2D rb;
    public Animator anim;
    SpriteRenderer sr;
    

    void Awake()
    {
        //GameManager.SM
        jumpBtn = GUIManager.instance.jumpBtn.GetComponent<JumpButton>();
        jumpBtn.SetCb((flag) => isJumpBtnDown = flag);
        slideBtn = GUIManager.instance.slideBtn.GetComponent<SlideButton>();
        slideBtn.SetCb((flag) => isSlideBtnDown = flag);

        f_Accel = 1 / f_StartToFinishTime;
        StageManager.instance.go_Player = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        v_moveDir = Vector3.up;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = GRAVITY;
        ChangeState(E_State.Run);
        colliders[0].enabled = true;
        colliders[1].enabled = false;
        //cam 을 GM 으로 부터 할당받기.
        go_Target = StageManager.instance.go_Boss;
        landPosition = go_Target.GetComponent<BossMonster>().goal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        InputControl();
        UpdateState();
        DetectBoss();
    }
    public void InputControl()
    {
        //if(state != E_State.Aviation)
        if (Input.GetKeyDown(KeyCode.Space)) DoJump();// Jump_Input();
        //isSlideBtnDown = Input.GetKey(KeyCode.D);
        //Debug.Log("Slide Key" + Input.GetKey(KeyCode.D));
        if(Input.GetKeyDown(KeyCode.D)) isSlideBtnDown = true;
        else if(Input.GetKeyUp(KeyCode.D)) isSlideBtnDown = false;

        if (Input.GetKeyDown(KeyCode.Space)) isJumpBtnDown = true;
        else if (Input.GetKeyUp(KeyCode.Space)) isJumpBtnDown = false;
    }
    void UpdateState()
    {
        rb.velocity = new Vector2(1 * SPEED, rb.velocity.y);

        switch (state)
        {
            case E_State.Run:
                DoSlide();
                break;

            case E_State.JumpUP:
                if (rb.velocity.y <= 0) ChangeState(E_State.JumpDown);
                break;

            case E_State.JumpDown:
                //if (transform.position.y <= y_base) ChangeState(E_State.Run);
                break;

            case E_State.RunUp:
                dist += Vector3.right.x * SPEED * Time.deltaTime;
                if (dist >= f_RunUp_Dist)
                {
                    ChangeState(E_State.LastJump);
                }
                break;

            case E_State.LastJump:// x 방향으로 3만큼 이동 후 점프.
                transform.position += v_moveDir * jumpPower[0] * Time.deltaTime;
                if (transform.position.y >= max_Height) ChangeState(E_State.Stay);
                break;

            case E_State.Aviation:
                rb.velocity = new Vector2(1 * SPEED, 0);
                v_moveDir = isJumpBtnDown == true ? Vector3.up : Vector3.down;// * 2;
                if (transform.position.y >= max_Height) v_moveDir = Vector3.down;// * 2;
                transform.position += v_moveDir * f_Avitation_Accel_Y * 2 * Time.deltaTime;
                if (transform.position.y < y_base)
                    transform.position = new Vector3(transform.position.x, y_base, transform.position.z);
                break;

            case E_State.Stay:
                
                break;

            case E_State.Attack:
                if (transform.position.y >= y_base)
                {
                    f_timer += Time.deltaTime;
                }
                else
                {
                    ChangeState(E_State.End);// f_Speed = 0;
                    Debug.Log("Land Time : " + f_timer);
                }
                break;
        }
    }
    public void ChangeState(E_State state)
    {
        this.state = state;
        switch (state)
        {
            case E_State.Run:
                SoundManager.instance.Event_RunSound();
                anim.SetBool("isLand", false);
                SPEED = f_Speed;
                rb.gravityScale = GRAVITY;
                break;

            case E_State.JumpUP:
                rb.gravityScale = GRAVITY;
                break;

            case E_State.JumpDown:
                anim.SetBool("isLand", true);
                rb.gravityScale = DownGraviry;
                break;

            case E_State.LastJump:
                anim.SetTrigger("doLastJump");
                rb.gravityScale = 0;
                v_moveDir = Vector3.up * 8;
                v_moveDir.Normalize();
                cam.ChangeCamType(followCamera.E_type.closeup);
                break;

            case E_State.Aviation:
                anim.SetBool("isSlide", false);
                rb.gravityScale = 0;
                SPEED = f_Avitation_Accel_X;
                //v_moveDir = Vector3.zero;
                break;

            case E_State.Stay:
                SPEED = 0;
                rb.velocity = Vector2.zero;
                break;

            case E_State.Attack:
                anim.SetInteger("attackIndex", 1);
                f_timer = 0;
                Vector3 temp = landPosition - transform.position;
                SPEED = temp.x * f_Accel;
                rb.velocity = new Vector2(rb.velocity.x, temp.y * f_Accel);
                break;

            case E_State.Attacked:
                anim.SetInteger("attackIndex", 2);
                SPEED = -1 * f_Attacked_Power;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 3.5f;
                rb.AddForce(new Vector2(-1, 1) * f_Attacked_Power, ForceMode2D.Impulse);
                this.gameObject.layer = 9;
                break;

            case E_State.End:
                SPEED = 0;
                rb.velocity = Vector2.zero;
                this.gameObject.layer = 9;
                GameManager.instance.GameEnd();
                break;
        }
    }
    public void DoJump()
    {
        if (jumpCnt < 2 && ((int)state <= (int)E_State.JumpDown))
        {
            SoundManager.instance.Event_JumpSound();
            Debug.Log("State(" + state + ")_Jump");
            anim.SetBool("isSlide", false);
            switch(jumpCnt)
            {
                case 0:
                    anim.SetTrigger("doJump");
                    break;
                case 1:
                    anim.SetBool("isLand", false);
                    anim.SetTrigger("doDoubleJump");
                    break;
            }
            ChangeState(E_State.JumpUP);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower[jumpCnt], ForceMode2D.Impulse);
            jumpCnt++;
        }
    }
    public void DoSlide()
    {
        anim.SetBool("isSlide", isSlideBtnDown);
        colliders[0].enabled = !isSlideBtnDown;
        colliders[1].enabled = isSlideBtnDown;
    }
    void DetectBoss()
    {
        // UI_Distance -> Update()

        if (!b_Find)
        {
            float dist = go_Target.transform.position.x - transform.position.x;
            if(dist <= m_fDetect_Dist)
            {
                b_Find = true;
                ChangeState(E_State.RunUp);
            }
            //Vector3 temp = transform.position;// + Vector3.up;
            //Vector2 vPos = new Vector2(temp.x, temp.y);
            //RaycastHit2D hit = Physics2D.Raycast(vPos, Vector3.right, m_fDetect_Dist, 1 << LayerMask.NameToLayer("Monster"));
            //if (hit)
            //{
            //    Debug.Log(hit.collider.gameObject.name);
            //    if (hit.collider.gameObject.tag == "BOSS")
            //    {
            //        b_Find = true;
            //        go_Target = hit.collider.gameObject;
            //        landPosition = go_Target.GetComponent<BossMonster>().goal.transform.position;
            //        ChangeState(E_State.LastJump);
            //        ChangeState(E_State.RunUp);
            //    }
            //}
        }
    }
    private void OnDrawGizmos()
    {
        switch (state)
        {
            case E_State.Run:
                Vector3 vPos = transform.position + Vector3.up;
                Debug.DrawRay(vPos, Vector3.right * m_fDetect_Dist, Color.red);
                break;
            case E_State.Stay:
            case E_State.LastJump:
            case E_State.Attack:
            case E_State.End:
                Vector3 goal = landPosition - transform.position;
                Debug.DrawRay(transform.position, goal, Color.blue);
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (transform.position.y + y_offset > collision.contacts[0].point.y)
            {
                jumpCnt = 0;
                if (state == E_State.Attacked) { ChangeState(E_State.End); }

                // 이걸 왜 넣었지?
                //else if ((int)state < (int)E_State.Aviation) ChangeState(E_State.Run);
                else if (state == E_State.JumpDown) ChangeState(E_State.Run);
            }
            //Debug.Log("Collision [Player : " + transform.position + "/contacts : " + collision.contacts[0].point + "]");
        }
    }
    public void Back() // 부활
    {
        if (state != E_State.End)
        {
            Debug.Log("Player back");
            transform.position = v_BackPos;
            ChangeState(E_State.Stay);
            GameManager.instance.followCam.ChangeCamType(followCamera.E_type.set);
        }
        //StartCoroutine(ReRun());
    }
    public IEnumerator ReRun() // 다시 달리기
    {
        yield return new WaitForSeconds(1f);
        ChangeState(E_State.Run);
    }
    IEnumerator AnimSlash()
    {
        yield return new WaitForSeconds(0.8f);
        anim.SetTrigger("Slash");
    }
    IEnumerator AnimFinish()
    {
        //cam.ChangeCamType(followCamera.E_type.final);
        yield return new WaitForSeconds(1.6f);
        anim.SetTrigger("IDle");
        yield return new WaitForSeconds(1f);
        //fin.SetActive(true);
    }
    IEnumerator AnimFinish2()
    {
        yield return new WaitForSeconds(0.61f);
        anim.speed = 0;
        yield return new WaitForSeconds(1f);
        //fin.SetActive(true);
    }
}
