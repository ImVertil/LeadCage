using UnityEngine;

public class VolumeHandler : MonoBehaviour
{
    private AudioSource[] _audioSources;

    void Start()
    {
        SoundEvents.OnVolumeChanged += UpdateVolume;
        _audioSources = GetComponents<AudioSource>();
    }

    public void UpdateVolume()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            SoundType type = SoundManager.Instance.GetSoundType(audioSource.clip);
            audioSource.volume = (SoundManager.Instance.volume / 100.0f) * (type == SoundType.SFX ? (SoundManager.Instance.sfxVolume / 100.0f) : (SoundManager.Instance.bgmVolume / 100.0f));
        }
    }
}
