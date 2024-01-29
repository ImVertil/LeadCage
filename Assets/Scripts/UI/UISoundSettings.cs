using TMPro;
using UnityEngine;

public class UISoundSettings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _masterVolumeText;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [SerializeField] private TextMeshProUGUI _effectsVolumeText;
    private int _masterVolume;
    private int _musicVolume;
    private int _effectsVolume;
    //private int _ambientVolume;

    public void Start()
    {
        _masterVolume = SoundManager.Instance.volume;
        _musicVolume = SoundManager.Instance.bgmVolume;
        _effectsVolume = SoundManager.Instance.sfxVolume;
        //_ambientVolume = SoungManager.Instance.ambientVolume;
    }

    public void ChangeMasterVolume(float val)
    {
        _masterVolumeText.text = val.ToString();
        _masterVolume = (int)val;
    }

    public void ChangeEffectsVolume(float val)
    {
        _effectsVolumeText.text = val.ToString();
        _effectsVolume = (int)val;
    }

    public void ChangeMusicVolume(float val)
    {
        _musicVolumeText.text = val.ToString();
        _musicVolume = (int)val;
    }

    //public void ChangeAmbientVolume(float val) TODO

    public void SaveVolumeSettings()
    {
        SoundManager.Instance.volume = _masterVolume;
        SoundManager.Instance.sfxVolume = _effectsVolume;
        SoundManager.Instance.bgmVolume = _musicVolume;
        SoundEvents.VolumeChanged();
    }
}
