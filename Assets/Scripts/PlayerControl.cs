using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public enum E_State { Run, Aviation, LastJump, Stay, Attack, Finish, Attacked, DIE };

    [Header("_Button_")]
    [SerializeField] JumpButton jumpBtn;
    [SerializeField] SlideButton slideBtn;
    [SerializeField] bool isJumpBtnDown;
    [SerializeField] bool isSlideBtnDown;

    [Header("_Set_")]
    public E_State state;               // �÷��̾� ����
    public float GRAVITY = 3.5f;        // �⺻ �߷°� ����
    public float y_base = -1.25f;       // �÷��̾��� �⺻ y ��
    public float max_Height;            // �÷��̾ �ִ�� ������ �� �ִ� ����
    public float y_offset;              // �÷��̾ �浹�� ������ offset �Ʒ� �������� Ȯ�ο�

    [Header("_MOVE_")]
    public float f_Speed;               // �ִ�ӵ�
    public float f_Avitation_Accel_X;   // Avitaion ���� �� ���ӵ�(x)
    public float f_Avitation_Accel_Y;   // Avitaion ���� �� ���ӵ�(y) --> 2���� �ʿ��ұ�
    float SPEED;                        // ����Ǵ� �ӵ�
    public Vector3 v_moveDir;                  // ��/�� �̵� ����

    [Header("_JUMP_")]
    //public float deltaY;                // LastJump �� �̵� �ӵ�
    public float[] jumpPower = new float[2];// �÷��̾��� ���� ���ӵ�(1��, 2��)
    public int jumpCnt;                 // ���� ���� ��
    
    [Header("_FIND_")]
    GameObject go_Target;               // �߰��� BOSS �� �����ϱ� ���� ����
    public float m_fDetect_Dist;        // Ray �߻� �Ÿ�
    public float m_fDetect_Height;      // Ray �߻� ����
    bool b_Find;                        // BOSS �߰� ����

    [Header("_ATTACK_")]
    public float f_RunUp_Dist;          // ����ݱ� ����
    float dist;                         // ����ݱ� �Ÿ�
    Vector3 landPosition;               // BOSS ���� �� ������ ����
    public float f_Attacked_Power;      // ���� ���� �� ƨ�ܳ����� ��
    public float f_StartToFinishTime;   // ������ ���۰� ���� �ɸ��� �ð�. float 
    float f_Accel;                      // ���� ������ �����ϴ� �ӵ�

    [Header("_ETC_")]
    public followCamera cam;
    public Vector3 v_BackPos;           // ������ ����
    
    float f_timer;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    

    void Awake()
    {
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
        //cam �� GM ���� ���� �Ҵ�ޱ�.
    }

    // Update is called once per frame
    void Update()
    {
        InputControl();
        UpdateState();
        DetectBoss();
    }
    
    public void Jump_Input()
    {
        switch (state)
        {
            case E_State.Run:
                DoJump();
                break;
            case E_State.Aviation:
                
                break;
            case E_State.LastJump:

                break;
            default:
                
                break;
        }
    }
    public void InputControl()
    {
        //if(state != E_State.Aviation)
        if (Input.GetKeyDown(KeyCode.Space)) Jump_Input();
        
        // Slide ���߿� ���� �߰� //
        

        /*if (!GameManager.instance.aviation)
        //{
        //     // ���� Test �� // 
        //    if (isSlideBtnDown)
        //    {
        //        transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        //    }
        //    else if (!isSlideBtnDown)
        //    {
        //        transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        //    }
        //}
        //if (GameManager.instance.aviation)
        //{
        //    //if (isJumpBtnDown)
        //    //{
        //    //    Vector3 test = Vector3.up * aviationSpeed * Time.deltaTime;
        //    //    transform.position += test;
        //    //}
        //    //else if (!isJumpBtnDown)
        //    //{
        //    //    Vector3 test = Vector3.down * aviationSpeed * Time.deltaTime;
        //    //    transform.position += test;
        //    //}
        //    if (isSlideBtnDown)
        //    {
        //        transform.position += Vector3.down * aviationSpeed * Time.deltaTime;
        //    }
        //}*/
    }
    void UpdateState()
    {
        rb.velocity = new Vector2(1 * SPEED, rb.velocity.y);

        switch (state)
        {
            case E_State.Run:
                if (isSlideBtnDown)
                {
                    transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                }
                else if (!isSlideBtnDown)
                {
                    transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                }
                break;

            case E_State.LastJump:// x �������� 3��ŭ �̵� �� ����.
                dist += Vector3.right.x * SPEED * Time.deltaTime;
                if (dist >= f_RunUp_Dist)
                {
                    transform.position += v_moveDir * jumpPower[0] * Time.deltaTime;
                    if (transform.position.y >= max_Height) ChangeState(E_State.Stay);
                }
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
                    ChangeState(E_State.Finish);// f_Speed = 0;
                    Debug.Log("Land Time : " + f_timer);
                }
                break;

            case E_State.Finish:
                break;

            case E_State.DIE:
                break;
        }
    }
    public void ChangeState(E_State state)
    {
        this.state = state;
        switch (state)
        {
            case E_State.Run:
                SPEED = f_Speed;
                rb.gravityScale = GRAVITY;
                break;

            case E_State.LastJump:
                rb.gravityScale = 0;
                v_moveDir = Vector3.up * 8;
                v_moveDir.Normalize();
                cam.ChangeCamType(followCamera.E_type.closeup);

                break;
            case E_State.Aviation:
                rb.gravityScale = 0;
                SPEED += f_Avitation_Accel_X;
                //v_moveDir = Vector3.zero;
                break;

            case E_State.Stay:
                SPEED = 0;
                rb.velocity = Vector2.zero;
                break;

            case E_State.Attack:
                f_timer = 0;
                Vector3 temp = landPosition - transform.position;
                SPEED = temp.x * f_Accel;
                rb.velocity = new Vector2(rb.velocity.x, temp.y * f_Accel);
                break;

            case E_State.Attacked:
                SPEED = -1 * f_Attacked_Power;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 3.5f;
                rb.AddForce(new Vector2(-1, 1) * f_Attacked_Power, ForceMode2D.Impulse);
                break;

            case E_State.Finish:
                SPEED = 0;
                rb.velocity = Vector2.zero;
                GameManager.instance.StageClear();
                break;

            case E_State.DIE:
                SPEED = 0;
                rb.velocity = Vector2.zero;
                this.gameObject.layer = 9;
                GameManager.instance.GameOver();
                break;
        }
    }
    void DoJump()
    {
        if (jumpCnt < 2)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower[jumpCnt], ForceMode2D.Impulse);
            jumpCnt++;
        }
    }
    void DetectBoss()
    {
        if (!b_Find)
        {
            Vector3 temp = transform.position + Vector3.up * m_fDetect_Height;
            Vector2 vPos = new Vector2(temp.x, temp.y);
            RaycastHit2D hit = Physics2D.Raycast(vPos, Vector3.right, m_fDetect_Dist, 1 << LayerMask.NameToLayer("Monster"));
            if (hit)
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.tag == "BOSS")
                {
                    b_Find = true;
                    go_Target = hit.collider.gameObject;
                    landPosition = go_Target.GetComponent<BossMonster>().goal.transform.position;
                    ChangeState(E_State.LastJump);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        switch (state)
        {
            case E_State.Run:
                Vector3 vPos = transform.position + Vector3.up * m_fDetect_Height;
                Debug.DrawRay(vPos, Vector3.right * m_fDetect_Dist, Color.red);
                break;
            case E_State.Stay:
            case E_State.LastJump:
            case E_State.Attack:
            case E_State.Finish:
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
                if(E_State.Attacked == state) { ChangeState(E_State.DIE); }
            }
            //Debug.Log("Collision [Player : " + transform.position + "/contacts : " + collision.contacts[0].point + "]");
        }
    }
    public void Back() // ��Ȱ
    {
        transform.position = v_BackPos;
        StartCoroutine(ReRun());
        Debug.Log("Player back");
    }
    IEnumerator ReRun() // �ٽ� �޸���
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
