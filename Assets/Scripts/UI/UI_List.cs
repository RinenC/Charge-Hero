using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_List : MonoBehaviour
{
    public GameObject go_Prefab;
    public List<GameObject> go_list = new List<GameObject>();
    public void Init(int cnt)
    {
        for (int i = 0; i < go_list.Count; i++) Destroy(go_list[i]);
        go_list.Clear();
        for (int i = 0; i < cnt; i++) Add();
    }
    public void Add(string type = null, int value = 0)
    {
        GameObject _prefab = Instantiate(go_Prefab, transform);
        go_list.Add(_prefab);

        BuffTimer bufftimer = _prefab.GetComponent<BuffTimer>();
        if (bufftimer) bufftimer.Init(type, value);
    }
    public void Remove()
    {
        Destroy(go_list[go_list.Count - 1]);
        go_list.RemoveAt(go_list.Count - 1);

        //go_list.Remove(null);
    }
}
