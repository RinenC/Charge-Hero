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
    [Header("_�� ���� ����_")]
    public EnemyType curType;

    [Header("_���� ����_")]
    public GameObject patrolLeftPoint;
    public GameObject patrolRightPoint;
    public GameObject targetPoint;

    [Header("_���� ���� ���� ����_")]
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
        Vector3 vDist = targetPoint.transform.position - transform.position;//��ġ�� ���̸� �̿��� �Ÿ����ϱ�
        Vector3 vDir = vDist.normalized;//�ι�ü������ ����(����ȭ-�Ÿ����� �̵���) //< normalized = ���̰� 1�� ���� ( ���� 1�̰� ���⸸ ����.) ���·� �������.
        float fDist = vDist.magnitude; //�ι�ü������ �Ÿ�(��Į��-�����̵���)

        if (fDist > speed * Time.deltaTime)//���������� �̵��Ÿ����� Ŭ���� �̵��Ѵ�.
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