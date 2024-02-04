using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    #region SINGLETON
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(this);
        }
        SoundEvents.OnVolumeChanged += UpdateSoundVolume;
    }
    #endregion
    [SerializeField] private int _simultaneousSoundsAmount;
    private ObjectPool<GameObject> _pool;
    private List<GameObject> _activeFromPool = new List<GameObject>();
    private GameObject _oneShotSoundObject;
    private GameObject _bgmSoundObject;
    public int volume = 50;
    public int sfxVolume = 100;
    public int bgmVolume = 100;
    public int ambientVolume = 100;
    public SoundClip[] sounds;

    private void Initialize()
    {
        _pool = new ObjectPool<GameObject>(CreateSoundObject, OnGetSoundObject, OnReturnSoundObject, OnDestroySoundObject, true, _simultaneousSoundsAmount, _simultaneousSoundsAmount + 10);
        if(_oneShotSoundObject == null)
        {
            _oneShotSoundObject = new GameObject("Sound (One Shot)");
            _bgmSoundObject = new GameObject("Sound (BGM)");
            AudioSource as1 = _oneShotSoundObject.AddComponent<AudioSource>();
            AudioSource as2 = _bgmSoundObject.AddComponent<AudioSource>();
            as1.bypassReverbZones = true;
            as2.bypassReverbZones = true;
        }
    }

    // PlaySound - this one spawns the sound object on the transform and plays the sound with given additional parameters
    public void PlaySound(Sound sound, Transform transform, bool loop, float? spatial=null, float? pitch=null) // if loop is set to true remember to use StopSound to turn the clip off
    {
        GameObject soundObject = _pool.Get();
        _activeFromPool.Add(soundObject);
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        soundObject.transform.position = Vector3.zero;
        soundObject.transform.SetParent(transform, false);
        audioSource.clip = GetAudioClip(sound);
        audioSource.loop = loop;
        audioSource.spatialBlend = (float)(spatial == null ? 1.0f : spatial);
        audioSource.pitch = (float)(pitch == null ? 1.0f : pitch);
        audioSource.volume = (volume / 100f) * (GetSoundType(sound) == SoundType.SFX ? (sfxVolume / 100f) : (ambientVolume / 100f));
        audioSource.Play();
        if (!loop)
            StartCoroutine(WaitAndRelease(soundObject, audioSource.clip.length));
    }

    // PlaySound - this one plays the sound as OneShot object (use for 2D space)
    public void PlaySound(Sound sound)
    {
        AudioSource audioSource = _oneShotSoundObject.GetComponent<AudioSource>();
        AudioClip clip = GetAudioClip(sound);
        //audioSource.volume = (volume / 100f) * (GetSoundType(sound) == SoundType.SFX ? (sfxVolume / 100f) : (ambientVolume / 100f));
        audioSource.volume = (volume / 100f) * (sfxVolume / 100f);
        audioSource.PlayOneShot(clip);
    }

    // PlayMusic - plays a looped sound in the background (use for BGM)
    public void PlayMusic(Sound sound)
    {
        AudioSource audioSource = _bgmSoundObject.GetComponent<AudioSource>();
        AudioClip clip = GetAudioClip(sound);
        audioSource.volume = (volume / 100f) * (bgmVolume / 100f);
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
        //StartCoroutine(SmoothMusicChange(2));
    }
    
    // StopSound - stops all instances of a sound on given transform
    public void StopSound(Sound sound, Transform transform) 
    {
        AudioSource[] audioSources = transform.GetComponentsInChildren<AudioSource>();
        foreach(AudioSource audioSource in audioSources)
        {
            Sound clipSound = GetSound(audioSource.clip);
            if (audioSource.isPlaying && clipSound == sound)
            {
                audioSource.Stop();
                StartCoroutine(WaitAndRelease(audioSource.gameObject, 0));
            }
        }
    }

    public void UpdateSoundVolume()
    {
        // Update both objects for music and 2d sfx
        _bgmSoundObject.GetComponent<AudioSource>().volume = (volume / 100f) * (bgmVolume / 100f);
        _oneShotSoundObject.GetComponent<AudioSource>().volume = (volume / 100f) * (sfxVolume / 100f);

        // Update all active objects from pools to change their sound volume while they're playing
        // not tested btw xD
        foreach (var obj in _activeFromPool)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            SoundType type = GetSoundType(audioSource.clip);
            audioSource.volume = (volume / 100f) * (type == SoundType.SFX ? (sfxVolume / 100f) : (ambientVolume / 100f));
        }
    }

    public AudioClip GetAudioClip(Sound sound)
    {
        return Array.Find(sounds, s => s.name == sound).audioClip;
    }

    public Sound GetSound(AudioClip clip)
    {
        return Array.Find(sounds, s => s.audioClip == clip).name;
    }

    public SoundType GetSoundType(AudioClip clip)
    {
        return Array.Find(sounds, s => s.audioClip == clip).type;
    }

    public SoundType GetSoundType(Sound sound)
    {
        return Array.Find(sounds, s => s.name == sound).type;
    }

    private IEnumerator WaitAndRelease(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        _activeFromPool.Remove(obj);
        _pool.Release(obj);
    }

    // draft
    private IEnumerator SmoothMusicChange(float time)
    {
        AudioSource audioSource = _bgmSoundObject.GetComponent<AudioSource>();
        float volume = audioSource.volume;
        audioSource.volume = 0;
        var test = time / 100;
        while(audioSource.volume != volume)
        {
            audioSource.volume += test;
            yield return new WaitForSeconds(test);
        }
    }
    #region OBJECT_POOL_METHODS
    private GameObject CreateSoundObject()
    {
        GameObject obj = new GameObject("Sound");
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        obj.AddComponent<VolumeHandler>();
        audioSource.spatialBlend = 1;
        audioSource.dopplerLevel = 0.1f;
        audioSource.volume = volume / 100f;
        return obj;
    }

    private void OnGetSoundObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnSoundObject(GameObject obj)
    {
        AudioSource audioSource = obj.GetComponent<AudioSource>();
        obj.transform.SetParent(null);
        audioSource.loop = false;

        obj.SetActive(false);  
    }

    private void OnDestroySoundObject(GameObject obj)
    {
        Destroy(obj);
    }
    #endregion
}