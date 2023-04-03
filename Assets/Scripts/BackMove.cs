using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove : MonoBehaviour
{
    public float speed = 0.5f;
    float offset;
    public Renderer rend;
    private void Update()
    {
        offset += Time.deltaTime * speed;
        rend.material.mainTextureOffset += Vector2.right * Time.deltaTime * speed;
    }
}
