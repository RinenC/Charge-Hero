using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCoin : MonoBehaviour
{
    public float range;
    public List<GameObject> obstructionList = new List<GameObject>();
    public List<GameObject> changeList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Transform();
    }

    void Transform()
    {
        if (GameManager.instance.changeCoin)
        {
            for (int i = 0; i < obstructionList.Count; i++)
            {
                float dist = Vector3.Distance(GameManager.instance.go_Player.transform.position, obstructionList[i].transform.position);
                if (dist < range)
                    changeList.Add(obstructionList[i]);
            }

            for (int i = 0; i < changeList.Count; i++)
            {
                changeList[i].GetComponent<Obstruction>().obstruction.SetActive(false);
                for (int j = 0; j < changeList[i].GetComponent<Obstruction>().gold.Count; j++)
                    changeList[i].GetComponent<Obstruction>().gold[j].SetActive(true);
            }
        }
        changeList.Clear();
        GameManager.instance.changeCoin = false;
    }
}
