using UnityEngine;

public class GunManager : MonoBehaviour {


    private GunPlayEvents _gpe;
    private Gun _currentGun;

    private Camera _mainCamera;

    
    private bool _haveGun;
    private bool _gunPulledOut;
    private float _cooldownCounter;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _gpe = GunPlayEvents.Instance;
        _gpe.OnGunEquip += GunEquip;
        InputManager.current.UnsheatheAction.performed += SheatheUnsheatheGun;
    }

    private void Update()
    {
        if(_haveGun.false)
            return;
        if (_inputManager.Fire && Time.time >= _cooldownCounter && _gunPulledOut)
        {
           _cooldownCounter = Time.time + 1f / _currentGun.GetFireRate();
           _currentGun.Shoot();
        }
    }

    private void GunEquip(Gun gun)
    {
        _currentGun = gun;
    }

    private void SheatheUnsheatheGun()
    {
        if(!_currentGun)
            return;
        _currentGun.SheatheUnsheathe();
    }

}