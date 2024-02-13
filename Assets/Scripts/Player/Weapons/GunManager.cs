using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using Player.Weapons;

public class GunManager : MonoBehaviour {


    [SerializeField] Transform Target;
    [SerializeField] GameObject Carbine;
    [SerializeField] GameObject Rifle;

    private Gun _currentGun;

    private Camera _mainCamera;

    public enum WeaponSlot
    {
        Primary,
        Secondary
    }

    public WeaponSlot CurrentWeaponSlot;



    private bool _haveGun;
    private float _cooldownCounter;

    private bool _canShoot = true;

    private void Start()
    {
        _mainCamera = Camera.main;
        GunPlayEvents.OnGunEquip += GunEquip;
        InputManager.current.UnsheatheAction.performed += SheatheUnsheatheGun;
        InputManager.current.PrimaryWeaponAction.performed += SwitchToPrimary;
        InputManager.current.SecondaryWeaponAction.performed += SwitchToSecondary;
        CurrentWeaponSlot = WeaponSlot.Primary;

        InputManager.DisableShooting += DisableGun;
        InputManager.EnableShooting += EnableGun;

        InputManager.ForceGunAway += ForceGunPutAway;
    }

    private void Update()
    {
        Target.position = _mainCamera.transform.position + _mainCamera.transform.forward*10;
        if(!_haveGun || !_canShoot)
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

    void GunUnequip()
    {
        _haveGun = false;
        _currentGun = null;
    }

    private void SheatheUnsheatheGun(InputAction.CallbackContext ctx)
    {
        if(!_haveGun || !_canShoot)
            return;
        _currentGun.SheatheUnsheathe();
    }

    private void SwitchToPrimary(InputAction.CallbackContext ctx)
    {
        Item weapon = Inventory.Instance.GetPrimaryWeapon();

        if(weapon == null || !_canShoot)
            return;

        if(weapon.id == 3)
        {
            Rifle.SetActive(true);
            Carbine.SetActive(false);
        }
        if(weapon.id == 4)
        {
            Rifle.SetActive(false);
            Carbine.SetActive(true);
        }
        CurrentWeaponSlot = WeaponSlot.Primary;
    }

    private void SwitchToSecondary(InputAction.CallbackContext ctx)
    {
        Item weapon = Inventory.Instance.GetSecondaryWeapon();

        if(weapon == null)
            return;

        if(weapon.id == 3)
        {
            Rifle.SetActive(true);
            Carbine.SetActive(false);
        }
        if(weapon.id == 4)
        {
            Rifle.SetActive(false);
            Carbine.SetActive(true);
        }
        CurrentWeaponSlot = WeaponSlot.Secondary;
    }

    private void DisableGun()
    {
        _canShoot = false;
    }

    private void EnableGun()
    {
        _canShoot = true;
    }

    private void ForceGunPutAway()
    {
        if(_haveGun && _currentGun.GunPulledOut)
        { 
            _currentGun.SheatheUnsheathe();
        }
    }

}