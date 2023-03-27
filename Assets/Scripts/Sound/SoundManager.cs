using System;
using System.Collections;
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
    }
    #endregion

    [SerializeField] private int _simultaneousSoundsAmount;
    [SerializeField] private int _volume = 65;
    public SoundClip[] sounds;
    private ObjectPool<GameObject> _pool;
    private GameObject _oneShotSoundObject;

    private void Initialize()
    {
        _pool = new ObjectPool<GameObject>(CreateSoundObject, OnGetSoundObject, OnReturnSoundObject, OnDestroySoundObject, true, _simultaneousSoundsAmount, _simultaneousSoundsAmount + 10);
        if(_oneShotSoundObject == null)
        {
            _oneShotSoundObject = new GameObject("Sound (One Shot)");
            _oneShotSoundObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(Sound sound, Transform transform, bool loop) // if loop is set to true remember to use StopSound to turn the clip off
    {
        GameObject soundObject = _pool.Get();
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        soundObject.transform.position = Vector3.zero;
        soundObject.transform.SetParent(transform, false);
        audioSource.clip = GetAudioClip(sound);
        audioSource.loop = loop;
        audioSource.Play();
        if (!loop)
            StartCoroutine(WaitAndRelease(soundObject, audioSource.clip.length));
    }

    public void PlaySound(Sound sound)
    {
        AudioSource audioSource = _oneShotSoundObject.GetComponent<AudioSource>();
        AudioClip clip = GetAudioClip(sound);
        audioSource.PlayOneShot(clip);
    }

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

    public AudioClip GetAudioClip(Sound sound)
    {
        return Array.Find(sounds, s => s.name == sound).audioClip;
    }

    public Sound GetSound(AudioClip clip)
    {
        return Array.Find(sounds, s => s.audioClip == clip).name;
    }

    private IEnumerator WaitAndRelease(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        _pool.Release(obj);
    }

    #region OBJECT_POOL_METHODS
    private GameObject CreateSoundObject()
    {
        GameObject obj = new GameObject("Sound");
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.dopplerLevel = 0.1f;
        audioSource.volume = _volume / 100.0f;
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