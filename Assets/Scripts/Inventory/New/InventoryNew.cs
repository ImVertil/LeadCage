using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class InventoryNew : MonoBehaviour
{
    #region SINGLETON
    public static InventoryNew Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion
    [SerializeField] private int _poolSize = 20;
    private ObjectPool<GameObject> _itemPool;
    // Start is called before the first frame update
    void Start()
    {
        _itemPool = new ObjectPool<GameObject>(CreateSoundObject, OnGetSoundObject, OnReturnSoundObject, OnDestroySoundObject, true, _poolSize, _poolSize + 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region OBJECT_POOL_METHODS
    private GameObject CreateSoundObject()
    {
        GameObject gameObject = new GameObject("A");
        return gameObject;
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
