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


    public AudioSource BG_AudioSource;
    public AudioSource Effect_AudioSource;
    //public AudioSource Player_Run_AudioSource;

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    enum BGMSound { TitleBGM, SceneBGM, PlayBGM, BossBGM }
    enum EffectSound { Click, Jump, GetCoin } // Enhance, Hit
    public AudioClip Jump_Clip;
    public AudioClip GetItem_Clip;
    public AudioClip Click_Clip;
    public AudioClip Attack_Clip;
    public AudioClip Run_Clip;
    public AudioClip BackGround_Clip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            // 소리 크기 값 읽어오기.
            string[] bgmNames = System.Enum.GetNames(typeof(BGMSound));
            AudioClip audioclip = null;// Resources.Load<AudioClip>()
            for (int i = 0; i < bgmNames.Length; i++)
            {
                string path = string.Format($"Sound/{bgmNames[i]}");
                audioclip = Resources.Load<AudioClip>(path);
                _audioClips.Add(bgmNames[i], audioclip);
            }
            string[] effectNames = System.Enum.GetNames(typeof(EffectSound));
            for (int i = 0; i < effectNames.Length; i++)
            {
                string path = string.Format($"Sound/{effectNames[i]}");
                audioclip = Resources.Load<AudioClip>(path);
                _audioClips.Add(effectNames[i], audioclip);
            }

            BG_AudioSource.loop = true;
            PlayBGM("TitleBGM");
        }
        else
            Destroy(this.gameObject);
    }
    public void PlayBGM(string name)
    {
        BG_AudioSource.Stop();
        AudioClip temp = null;
        _audioClips.TryGetValue(name, out temp);
        BG_AudioSource.clip = temp;
        BG_AudioSource.Play();
    }
    public void PlayEffect(string name)
    {
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
    }
    public void Play(string name, float pitch = 1.0f)
    {
        //audio_Effects[0].Pause();
        //audio_Effects[0].Play();
        //audio_Effects[0].PlayOneShot();
        //audio_Effects[0].
    }
    public void Event_ClickSound()
    {
        //AudioSource audioS = new AudioSource();
        //audioS.PlayOneShot(audioClip);
        Effect_AudioSource.clip = Click_Clip;
        Effect_AudioSource.Play();
    }
    public void Event_JumpSound()
    {
        //Event_RunSound_Stop();

        Effect_AudioSource.clip = Jump_Clip;
        Effect_AudioSource.Play();
    }
    //public void Event_RunSound()
    //{
    //    Player_Run_AudioSource.Play();
    //}
    //public void Event_RunSound_Stop()
    //{
    //    Player_Run_AudioSource.Pause();
    //}
    public void Event_GetItemSound()
    {
        Effect_AudioSource.clip = GetItem_Clip;
        Effect_AudioSource.Play();
    }
    public void Mute_All(bool mute)
    {
        //Mute_BG(mute);
        //Mute_Effect(mute);
    }
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
    //}


}
