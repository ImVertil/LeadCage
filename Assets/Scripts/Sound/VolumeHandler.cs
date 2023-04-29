using UnityEngine;

public class VolumeHandler : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        SoundEvents.OnVolumeChanged += UpdateVolume;
        _audioSource = GetComponent<AudioSource>();
    }

    public void UpdateVolume()
    {
        Debug.Log(_audioSource.clip);
        SoundType type = SoundManager.Instance.GetSoundType(_audioSource.clip);
        _audioSource.volume = (SoundManager.Instance.volume / 100.0f) * (type == SoundType.SFX ? (SoundManager.Instance.sfxVolume / 100.0f) : (SoundManager.Instance.bgmVolume / 100.0f));
    }
}
