using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UISettingsSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textObject;
    [SerializeField] private TextMeshProUGUI _volume;
    private int _value;

    private void Start()
    {
        if (_volume.text.Equals("Effects")) _value = SoundManager.Instance.sfxVolume;
        else if (_volume.text.Equals("Music")) _value = SoundManager.Instance.bgmVolume;
        else _value = SoundManager.Instance.volume;
    }

    public void SetText(float value)
    {
        _textObject.text = value.ToString();
    }

    public void SetVolume(float value)
    {
        _value = (int)value;
    }

    public void SaveSoundSettings()
    {
        if (_volume.text.Equals("Effects")) SoundManager.Instance.sfxVolume = _value;
        else if (_volume.text.Equals("Music")) SoundManager.Instance.bgmVolume = _value;
        else SoundManager.Instance.volume = _value;
    }
}
