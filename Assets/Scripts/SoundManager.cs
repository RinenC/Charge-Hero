using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("¿Àµð¿À")]
    [SerializeField] private AudioListener _master;
    [SerializeField] private Slider m_volume;
    [SerializeField] private AudioSource _back;
    [SerializeField] private Slider b_volume;
    [SerializeField] private AudioSource _effect;
    [SerializeField] private Slider e_volume;

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
