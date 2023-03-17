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
    }
    #endregion

    [SerializeField] private int _simultaneousSoundsAmount;
    public SoundClip[] sounds;
    private ObjectPool<GameObject> _pool;

    private void Initialize()
    {
        _pool = new ObjectPool<GameObject>(CreateSoundObject, OnGetSoundObject, OnReturnSoundObject, OnDestroySoundObject, true, _simultaneousSoundsAmount, _simultaneousSoundsAmount + 10);
    }

    public void PlaySound(Sound sound, Vector3 position) // rename to PlaySFX when remaking the other PlaySound
    {
        GameObject soundObject = _pool.Get();
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        soundObject.transform.position = position;
        audioSource.clip = GetAudioClip(sound);
        audioSource.Play();
        StartCoroutine(WaitAndRelease(soundObject, audioSource.clip.length));
    }

    public void PlaySound(Sound sound) // this will be remade into PlayBGM later on 
    {
        GameObject soundObject = _pool.Get();
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        AudioClip clip = GetAudioClip(sound);
        audioSource.PlayOneShot(clip);
        StartCoroutine(WaitAndRelease(soundObject, clip.length));
    }

    public AudioClip GetAudioClip(Sound sound)
    {
        return Array.Find(sounds, s => s.name == sound).audioClip;
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
        return obj;
    }

    private void OnGetSoundObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnSoundObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroySoundObject(GameObject obj)
    {
        Destroy(obj);
    }
    #endregion
}