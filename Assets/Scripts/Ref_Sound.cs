using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ref_Sound
{
    //    AudioSource[] _audioSources = new AudioSource[10]; // 용도를 나누어서 만들어 놓자.
    //    // mp3 audiosource를 담을 배열 선언

    //    // 캐싱 역할을 할 _audioClips 딕션어리 
    //    // 캐싱은 사운드를 재생할때마다 계속 path를 입력받아 사운드를 찾는 방식이 아닌 한번 사용한 사운드는 딕셔너리로 관리하여 보다 빠르게 처리하기 위해서 캐싱용 딕션너리를 사용하는 것이다.
    //    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            //string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

//            for (int i = 0; i < soundNames.Length - 1; i++)
//            {
//                GameObject go = new GameObject { name = soundNames[i] };
//                _audioSources[i] = go.AddComponent<AudioSource>(); // 위에서 만든 _audioSources에 넣어준다.
//                go.transform.parent = root.transform;
//            }
//            // soundName을 돌면서 새로운 GameObject를 만들어준다.

//            _audioSources[(int)Define.Sound.Bgm].loop = true; // Bgm같은 경우에는 루프로 계속 사운드가 나도록 해준다.
        }

    }

//    public void Clear()
//    {
//        foreach (AudioSource audioSource in _audioSources)
//        {
//            audioSource.clip = null;
//            audioSource.Stop();
//        }
//        _audioClips.Clear();
//    }

//    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) // path로 경로를 받아주고 pitch = 소리 속도 조절
//    {
//        AudioClip audioclip = GetOrAddAudioClip(path, type);
//        Play(audioclip, type, pitch);
//    }

//    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) // path로 경로를 받아주고 pitch = 소리 속도 조절
//    {
//        if (audioClip == null)
//        {
//            return;
//        }

//        if (type == Define.Sound.Bgm)
//        {
//            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
//            if (audioSource.isPlaying)
//                audioSource.Stop();

//            audioSource.pitch = pitch;
//            audioSource.clip = audioClip;
//            audioSource.Play();
//        }

//        else // (type == Define.Sound.Effect)
//        {
//            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
//            audioSource.pitch = pitch;
//            audioSource.clip = audioClip;
//            audioSource.PlayOneShot(audioClip);
//        }
//    }


//    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect) // audioClip 반환하는 함수(위에 Dictionary만든 부분에서)
//    {
//        if (path.Contains("Sounds/") == false)
//        {
//            path = $"Sounds/{path}";
//        }

//        AudioClip audioClip = null;

//        if (type == Define.Sound.Bgm)
//        {
//            audioClip = Managers.Resource.Load<AudioClip>(path);
//        }
//        else
//        {
//            if (_audioClips.TryGetValue(path, out audioClip) == false) // 있으면 이렇게 값을 뱉어주고 
//            {
//                audioClip = Managers.Resource.Load<AudioClip>(path);
//                _audioClips.Add(path, audioClip);
//            }
//        }

//        if (audioClip == null)
//        {
//            Debug.Log($"AudioClip Missing ! {path}");
//        }

//        return audioClip;
//    }

}