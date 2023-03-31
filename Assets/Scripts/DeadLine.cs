using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            Debug.Log("DeadLine Contact");
            collision.gameObject.GetComponent<PlayerControl>().Back();
            collision.gameObject.GetComponent<PlayerStatus>().Damaged(1);
            collision.gameObject.GetComponent<PlayerControl>().ChangeState(PlayerControl.E_State.Stay);
            GameManager.instance.followCam.ChangeCamType(followCamera.E_type.set);
        }
    }
}
