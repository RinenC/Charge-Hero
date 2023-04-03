using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction : MonoBehaviour
{
    public GameObject obstruction;
    public List<GameObject> gold = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    void Init()
    {
        obstruction.SetActive(true);
        for(int i = 0; i < gold.Count; i++)
        {
            gold[i].SetActive(false);
        }
    }
}
