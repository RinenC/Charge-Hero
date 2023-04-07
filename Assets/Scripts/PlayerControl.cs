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
    public E_State state;               // �÷��̾� ����
    public float GRAVITY = 3.5f;        // �⺻ �߷°� ����
    public float DownGraviry;           // �ϰ��� �� �߷� �� ����
    public float y_base = -1.25f;       // �÷��̾��� �⺻ y ��
    public float max_Height;            // �÷��̾ �ִ�� ������ �� �ִ� ����
    public float y_offset;              // �÷��̾ �浹�� ������ offset �Ʒ� �������� Ȯ�ο�

    [Header("_MOVE_")]
    public float f_Speed;               // �ִ�ӵ�
    public float f_Avitation_Accel_X;   // Avitaion ���� �� ���ӵ�(x)
    public float f_Avitation_Accel_Y;   // Avitaion ���� �� ���ӵ�(y) --> 2���� �ʿ��ұ�
    float SPEED;                        // ����Ǵ� �ӵ�
    Vector3 v_moveDir;                  // ��/�� �̵� ����
    public Collider2D[] colliders;
    float y_velocity;

    [Header("_JUMP_")]
    //public float deltaY;                // LastJump �� �̵� �ӵ�
    public float[] jumpPower = new float[2];// �÷��̾��� ���� ���ӵ�(1��, 2��)
    public int jumpCnt;                 // ���� ���� ��
    
    [Header("_FIND_")]
    GameObject go_Target;               // �߰��� BOSS �� �����ϱ� ���� ����
    float m_fDetect_Dist = 20;        // Ray �߻� �Ÿ�
    //public float m_fDetect_Height;      // Ray �߻� ����
    bool b_Find;                        // BOSS �߰� ����

    [Header("_ATTACK")]
    float f_RunUp_Dist = 5;          // ����ݱ� ����
    float dist;                         // ����ݱ� �Ÿ�
    Vector3 landPosition;               // BOSS ���� �� ������ ����
    float f_Attacked_Power= 20;      // ���� ���� �� ƨ�ܳ����� ��
    float f_StartToFinishTime = 0.5f;   // ������ ���۰� ���� �ɸ��� �ð�. float 
    float f_Accel;                      // ���� ������ �����ϴ� �ӵ�

    [Header("_ETC_")]
    public followCamera cam;
    public Vector3 v_BackPos;           // ������ ���� --> �Լ��� ����
    
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
        //cam �� GM ���� ���� �Ҵ�ޱ�.
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

            case E_State.LastJump:// x �������� 3��ŭ �̵� �� ����.
                transform.position += v_moveDir * jumpPower[0] * Time.deltaTime;
                if (transform.position.y >= max_Height) ChangeState(E_State.Stay);
                break;

            case E_State.Aviation:
                //y_velocity = isJumpBtnDown == true ? 1 : -1;
                //if (transform.position.y >= max_Height) y_velocity = -1;
                //else if (transform.position.y < y_base) y_velocity = 0;
                //y_velocity *= f_Avitation_Accel_Y * 2;
                //rb.velocity = new Vector2(1 * SPEED, y_velocity);
                rb.velocity = new Vector2(1 * SPEED, 0);
                v_moveDir = isJumpBtnDown == true ? Vector3.up : Vector3.down;// * 2;
                if (transform.position.y >= max_Height && v_moveDir == Vector3.up) v_moveDir = Vector3.zero;// * 2;
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
        //Debug.Log("ChangeState to " + state);
        this.state = state;
        switch (state)
        {
            case E_State.Run:
                // �÷��̾ �ٴ��� �մ°� ����
                if(transform.position.y <y_base)
                    transform.position = new Vector3(transform.position.x, y_base, transform.position.z);
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

            case E_State.RunUp:
                GUIManager.instance.bossHP_UI.gameObject.SetActive(true);
                rb.gravityScale = GRAVITY;
                SPEED = f_Speed;
                anim.SetBool("isLand", false);
                break;

            case E_State.LastJump:
                rb.velocity = new Vector2(SPEED, 0);//Last Scene Error ��
                anim.SetTrigger("doLastJump");
                gameObject.GetComponent<PlayerEffect>().OffEffect();
                rb.gravityScale = 0;
                v_moveDir = Vector3.up * 8;
                v_moveDir.Normalize();
                cam.ChangeCamType(followCamera.E_type.closeup);
                break;

            case E_State.Aviation:
                anim.SetBool("isSlide", false);
                rb.bodyType = RigidbodyType2D.Kinematic;
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
            SoundManager.instance.PlayEffect("Jump");
            //Debug.Log("State(" + state + ")_Jump" + jumpCnt);
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
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ground")
        {
            if (transform.position.y + y_offset > collision.contacts[0].point.y)
            {
                if (state == E_State.Attacked) { ChangeState(E_State.End); }
                // �̰� �� �־���?
                //else if ((int)state < (int)E_State.Aviation) ChangeState(E_State.Run);
                else if (state == E_State.JumpDown)
                {
                    jumpCnt = 0;
                    ChangeState(E_State.Run);
                }
                else if (state == E_State.Stay)
                {
                    jumpCnt = 0;
                }
            }
        }
    }
    public void Back() // ��Ȱ
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
    public IEnumerator ReRun() // �ٽ� �޸���
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
