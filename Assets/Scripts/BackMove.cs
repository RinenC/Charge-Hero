using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove : MonoBehaviour
{
    [SerializeField] LayerMask layer;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer ==layer)
        {
            Vector3 pos = transform.position;
            pos.x += 46;
            transform.position = pos;
        }
    }
}
