using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using Player.Weapons;

public class GunManager : MonoBehaviour {


    [SerializeField] Transform Target;

    private GunPlayEvents _gpe;
    private Gun _currentGun;

    private Camera _mainCamera;

    
    private bool _haveGun;
    private float _cooldownCounter;

    private void Start()
    {
        _mainCamera = Camera.main;
        _gpe = GunPlayEvents.Instance;
        _gpe.OnGunEquip += GunEquip;
        InputManager.current.UnsheatheAction.performed += SheatheUnsheatheGun;
    }

    private void Update()
    {
        Target.position = _mainCamera.transform.position + _mainCamera.transform.forward*10;
        if(!_haveGun)
            return;
        if(InputManager.current.Fire && Time.time >= _cooldownCounter && _currentGun.GunPulledOut)
        {
           _cooldownCounter = Time.time + 1f / _currentGun.FireRate;
           _currentGun.Shoot();
        }
        if(InputManager.current.Aim && _currentGun.GunPulledOut)
        {
            _currentGun.TakeAim();
        }
        else
        {
            _currentGun.StopAim();
        }
        
    }

    void GunEquip(Gun gun)
    {
        _haveGun = true;
        _currentGun = gun;
        _currentGun.RigBuilder = GetComponent<RigBuilder>();
        _currentGun.Animator = GetComponent<Animator>();
    }

    private void SheatheUnsheatheGun(InputAction.CallbackContext ctx)
    {
        if(!_haveGun)
            return;
        _currentGun.SheatheUnsheathe();
    }

}