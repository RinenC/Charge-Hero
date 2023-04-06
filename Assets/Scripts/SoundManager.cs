using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Slider MasterSlider;
    public Slider BGM_Slider;
    public Slider EffectSlider;
    public AudioMixer audioMixer;

    // 초기 값 //
    public float masterVolume;
    public float bgmVolume;
    public float effectVolume;

    public AudioSource BG_AudioSource;
    public AudioSource Effect_AudioSource;

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    enum BGMSound { TitleBGM, SceneBGM, PlayBGM, BossBGM }
    enum EffectSound { Click, Jump, GetCoin, Enhance, Attack, Attacked, Fail, Clear } 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            AudioClip audioclip = null;

            // BGM AudioClip Set //
            string[] bgmNames = System.Enum.GetNames(typeof(BGMSound));            
            for (int i = 0; i < bgmNames.Length; i++)
            {
                string path = string.Format($"Sound/{bgmNames[i]}");
                audioclip = Resources.Load<AudioClip>(path);
                _audioClips.Add(bgmNames[i], audioclip);
            }
            BG_AudioSource.loop = true;

            // Effect AudioClip Set //
            string[] effectNames = System.Enum.GetNames(typeof(EffectSound));
            for (int i = 0; i < effectNames.Length; i++)
            {
                string path = string.Format($"Sound/{effectNames[i]}");
                audioclip = Resources.Load<AudioClip>(path);
                _audioClips.Add(effectNames[i], audioclip);
            }
        }
        else
            Destroy(this.gameObject);
    }
    private void Start()
    {
        // Volume 초기값 설정 //
        // DB 에서 바로 읽기
        // 저장할 때 각 Slider.value 저장.
        masterVolume = -20f;
        bgmVolume = -20f;
        effectVolume = -20f;

        MasterSlider.value = masterVolume;
        MasterAudioControl();
        BGM_Slider.value = bgmVolume;
        BGMAudioControl();
        EffectSlider.value = effectVolume;
        EffectAudioControl();

        DBLoader.Instance.LoadSoundTest();
    }
    public void PlayBGM(string name)
    {
        BG_AudioSource.Stop();
        AudioClip temp = null;
        _audioClips.TryGetValue(name, out temp);
        BG_AudioSource.clip = temp;
        BG_AudioSource.Play();
    }
    public void OffBGM()
    {
        BG_AudioSource.Stop();
    }    
    public void PlayEffect(string name)
    {
        //Debug.Log("Play[" + name + "]Effect");
        AudioClip temp = null;
        _audioClips.TryGetValue(name, out temp);
        //Effect_AudioSource.clip = temp;
        Effect_AudioSource.PlayOneShot(temp);
    }
    // Slider value 읽고 쓰기 // 
    public void MasterAudioControl()
    {   // 일단 Slide 별로 함수 만들고 나중에 병합.
        float sound = MasterSlider.value;
        if (sound == -40f) audioMixer.SetFloat("Master", -80f);
        else audioMixer.SetFloat("Master", sound);
        audioMixer.GetFloat("Master", out sound);
    }
    public void BGMAudioControl()
    {   // 일단 Slide 별로 함수 만들고 나중에 병합.
        float sound = BGM_Slider.value;

        if (sound == -40f) audioMixer.SetFloat("BGM", -80f);
        else audioMixer.SetFloat("BGM", sound);
    }
    public void EffectAudioControl()
    {   // 일단 Slide 별로 함수 만들고 나중에 병합.
        float sound = EffectSlider.value;

        if (sound == -40f) audioMixer.SetFloat("Effect", -80f);
        else audioMixer.SetFloat("Effect", sound);
    }
    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    //}
    //public void Play(string name, float pitch = 1.0f)
    //{
    //    //audio_Effects[0].Pause();
    //    //audio_Effects[0].Play();
    //    //audio_Effects[0].PlayOneShot();
    //    //audio_Effects[0].
    //}
    //public void Event_ClickSound()
    //{
    //    //AudioSource audioS = new AudioSource();
    //    //audioS.PlayOneShot(audioClip);
    //    Effect_AudioSource.clip = Click_Clip;
    //    Effect_AudioSource.Play();
    //}
    //public void Event_JumpSound()
    //{
    //    //Event_RunSound_Stop();

    //    Effect_AudioSource.clip = Jump_Clip;
    //    Effect_AudioSource.Play();
    //}
    ////public void Event_RunSound()
    ////{
    ////    Player_Run_AudioSource.Play();
    ////}
    ////public void Event_RunSound_Stop()
    ////{
    ////    Player_Run_AudioSource.Pause();
    ////}
    //public void Event_GetItemSound()
    //{
    //    Effect_AudioSource.clip = GetItem_Clip;
    //    Effect_AudioSource.Play();
    //}
    //public void Mute_All(bool mute)
    //{
        //Mute_BG(mute);
        //Mute_Effect(mute);
    //}
    //public void Mute_BG(bool mute)
    //{
    //    for(int i = 0; i<audio_BackGrounds.Length; i++)
    //    {
    //        audio_BackGrounds[i].mute = mute;
    //    }
    //}
    //public void Mute_Effect(bool mute)
    //{
    //    for (int i = 0; i < audio_Effects.Length; i++)
    //    {
    //        audio_Effects[i].mute = mute;
    //    }
    //}
    //public void SETVOLUME(int idx)
    //{
    //    switch (idx)
    //    {
    //        case 0:
    //            AudioListener.volume = m_volume.value;
    //            break;
    //        case 1:
    //            _back.volume = b_volume.value;
    //            break;
    //        case 2:
    //            _effect.volume = e_volume.value;
    //            break;
    //    }        
    }
}
