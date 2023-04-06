using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BUFFTYPE { Invincibility, Aviation };
public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    BuffTimer buffs;// = new BuffTimer[2];
    GameObject prefab;
    //Dictionary<string, BuffTimer> _audioClips = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            
        }
    }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        prefab = Resources.Load<GameObject>("Prefabs/UI/buffTimer");

        //string[] buffNames = System.Enum.GetNames(typeof(E_BUFFTYPE));

        GameObject go = Instantiate(prefab, transform);//new GameObject { name = buffNames[i] };
        //go.name = buffNames[i];
        buffs = go.GetComponent<BuffTimer>();
        go.SetActive(false);
        go.transform.SetParent(transform, false);
    }
    public void Reset()
    {
        buffs.gameObject.SetActive(false);
    }
    public void TurnOnBuffTimer(E_BUFFTYPE buffname)
    {
        int idx = (int)buffname;
        string bufname = System.Enum.GetName(typeof(E_BUFFTYPE), idx);
        buffs.Init(bufname);
        if(buffs.gameObject.activeSelf)
        {
            buffs.ResetTimer();
        }
        else
        {
            buffs.gameObject.SetActive(true);
        }
    }
}
