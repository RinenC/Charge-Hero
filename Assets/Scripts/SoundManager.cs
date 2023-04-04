using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public SoundManager instance;

    [Header("오디오")]
    [SerializeField] private AudioListener _master;
    [SerializeField] private Slider m_volume;
    [SerializeField] private AudioSource _back;
    [SerializeField] private Slider b_volume;
    [SerializeField] private AudioSource _effect;
    [SerializeField] private Slider e_volume;

    public AudioSource[] audio_BackGrounds;
    public AudioSource[] audio_Effects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            // 소리 크기 값 읽어오기.
        }
        else
            Destroy(this.gameObject);
    }
    public void Play(string name, float pitch =1.0f)
    {
        
    }
    public void Mute_All(bool mute)
    {
        Mute_BG(mute);
        Mute_Effect(mute);
    }
    public void Mute_BG(bool mute)
    {
        for(int i = 0; i<audio_BackGrounds.Length; i++)
        {
            audio_BackGrounds[i].mute = mute;
        }
    }
    public void Mute_Effect(bool mute)
    {
        for (int i = 0; i < audio_Effects.Length; i++)
        {
            audio_Effects[i].mute = mute;
        }
    }
    public void SETVOLUME(int idx)
    {
        switch (idx)
        {
            case 0:
                AudioListener.volume = m_volume.value;
                break;
            case 1:
                _back.volume = b_volume.value;
                break;
            case 2:
                _effect.volume = e_volume.value;
                break;
        }        
    }


}
