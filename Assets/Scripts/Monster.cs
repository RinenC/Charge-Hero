using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Protruding,
    Patrol,
}

public class Monster : MonoBehaviour
{
    [Header("_적 유형 선택_")]
    public EnemyType curType;

    [Header("_순찰 지역_")]
    public GameObject patrolLeftPoint;
    public GameObject patrolRightPoint;
    public GameObject targetPoint;

    [Header("_돌출 몬스터 반응 범위_")]
    public float range;

    public float speed;
    public float Damage;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = patrolLeftPoint;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyTypeSet(curType);
    }

    private void EnemyTypeSet(EnemyType enemyType)
    {
        SpriteChange(enemyType);
        switch (enemyType)
        {
            case EnemyType.Protruding:
                ProturdingEnemy();
                break;
            case EnemyType.Patrol:
                PatrolEnemy();
                break;
        }
    }

    private void ProturdingEnemy()
    {
        patrolLeftPoint.SetActive(false);
        patrolRightPoint.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<Animator>().enabled = false;
        float dist = Vector3.Distance(GameManager.instance.go_Player.transform.position, transform.position);

        if(dist <= range)
        {
            SpriteChange(curType);
        }
    }

    private void PatrolEnemy()
    {
        Vector3 vDist = targetPoint.transform.position - transform.position;//위치의 차이를 이용한 거리구하기
        Vector3 vDir = vDist.normalized;//두물체사이의 방향(평준화-거리를뺀 이동량) //< normalized = 길이가 1인 백터 ( 힘이 1이고 방향만 있음.) 상태로 만들어줌.
        float fDist = vDist.magnitude; //두물체사이의 거리(스칼라-순수이동량)

        if (fDist > speed * Time.deltaTime)//한프레임의 이동거리보다 클때만 이동한다.
        {
            transform.position += vDir * speed * Time.deltaTime;
        }

        if (targetPoint == patrolLeftPoint)
        {
            if (fDist <= 0.5f)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                targetPoint = patrolRightPoint;
            }
        }
        else if (targetPoint == patrolRightPoint)
        {
            if (fDist <= 0.5f)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                targetPoint = patrolLeftPoint;
            }
        }
    }

    private void SpriteChange(EnemyType enemyType)
    {
        switch (curType)
        {
            case EnemyType.Protruding:
                GetComponent<SpriteRenderer>().sprite = DBLoader.Instance.protruding;
                break;
            case EnemyType.Patrol:
                GetComponent<SpriteRenderer>().sprite = DBLoader.Instance.patrol;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enemy_Collision");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStatus>().Damaged(Damage);
        }
    }
}