using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public GameObject player;

    public float speed;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.go_Player;
    }

    // Update is called once per frame
    void Update()
    {
        //GetGold();
        //Magnetic();
    }

    //public void Magnetic()
    //{
    //    if (GameManager.instance.magnetic)
    //    {
    //        float dist = Vector3.Distance(player.transform.position, transform.position);
    //        if (dist < range)
    //        {
    //            Vector3 vDist = player.transform.position - this.transform.position;
    //            Vector3 vDir = Vector3.Normalize(vDist);
    //            float fDist = vDist.magnitude;

    //            if (fDist > speed * Time.deltaTime)
    //                transform.position += vDir * speed * Time.deltaTime;
    //        }
    //    }
    //}

    //public void GetGold()
    //{
    //    float dist = Vector3.Distance(player.transform.position, transform.position);
    //    if (dist < 1.5f)
    //    {
    //        //GameManager.instance.gold += 1;
    //        Destroy(this.gameObject);
    //    }
    //}
}
