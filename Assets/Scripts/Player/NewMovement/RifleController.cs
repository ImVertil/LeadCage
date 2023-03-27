
using System.Collections;
using System.Numerics;
using Player.Weapons;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RifleController : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _controller;
    private RigBuilder _rigBuilder;
    private Rig _hipsRig;
    private Rig _handsRig;
    private Rig _aimRig;
    private Rig _weaponPullRig;
    private Rig _kickbackRig;
    private TwoBoneIKConstraint _rightHandConstraint;
    private TwoBoneIKConstraint _leftHandConstraint;
    private Camera _mainCamera;
    private MultiPositionConstraint _hipMPC;
    private MultiPositionConstraint _aimMPC;


    private int _riflePulledOutHash;
    private int _aimingHash;

    private bool _riflePulledOut = false;
    private bool _aiming = false;
    private bool _waiting = false;

    private float _startSensitivity;
    private Vector3 _startGunPos;
    private Vector3 _desiredGunPos;


    private float _desiredHipsRigWeight = 0f;
    private float _desiredHandsRigWeight = 0f;
    private float _desiredAimRigWeight = 0f;
    private float _desiredPullRigWeight = 0f;
    private float _desiredKickbackRigWeight = 0f;

    private float _hipRigWeightVelocity = 0f;
    private float _handRigWeightVelocity = 0f;
    private float _aimRigWeightVelocity = 0f;
    private float _pullRigweightVelocity = 0f;
    private float _kickbackRigWeightVelocity = 0f;

    private float _putSpeed = 0.5f;
    private float _glueSpeed = 0.2f;

    [SerializeField] private float ArmLayerWeightDelay = 0.5f;
    [SerializeField] private GameObject WeaponPivot;
    [SerializeField] private GameObject RightHandIK;
    [SerializeField] private GameObject LeftHandIK;
    [SerializeField] private LayerMask AimMask;
    [SerializeField] private Transform DebugTransform;

    [SerializeField] private GameObject BulletHolePrefab;
    
    [SerializeField] private float _range = 100f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _recoilX = 2f;
    [SerializeField] private float _recoilY = 2f;
    [SerializeField] private float _recoilZ = 0.35f;

    [SerializeField] private float WeaponKickbackSpeed;
    [SerializeField] private float WeaponKickbackForce;
    [SerializeField] private float WeaponReturnSpeed;
    
    public GameObject bulletOrigin;
    
    private float _cooldownCounter = 0f;

    private void Start()
    {
        
        _animator = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
        _rigBuilder = GetComponent<RigBuilder>();

        _rightHandConstraint = RightHandIK.GetComponent<TwoBoneIKConstraint>();
        _leftHandConstraint = LeftHandIK.GetComponent<TwoBoneIKConstraint>();
        
        _mainCamera = Camera.main;

        
        

        _startSensitivity = _controller.MouseSensitivity;
        //_startGunPos = _gunMesh.localPosition;

        _hipsRig = _rigBuilder.layers[0].rig;
        _handsRig = _rigBuilder.layers[4].rig;
        _aimRig = _rigBuilder.layers[1].rig;
        _weaponPullRig = _rigBuilder.layers[2].rig;
        _kickbackRig = _rigBuilder.layers[3].rig;

        _riflePulledOutHash = Animator.StringToHash("RiflePulledOut");
        _aimingHash = Animator.StringToHash("Aiming");
    }

    // Update is called once per frame
    void Update()
    {
       SheatheUnsheatheRifle();
       TakeAim();
       
       _kickbackRig.weight = Mathf.SmoothDamp(_kickbackRig.weight, _desiredKickbackRigWeight, ref _kickbackRigWeightVelocity, 1/(_fireRate*2f));

       
       //Vector3.Slerp(_gunMesh.transform.localPosition, _desiredGunPos, WeaponKickbackSpeed * Time.deltaTime);

       
       
       if (Input.GetButton(Controls.FIRE) && Time.time >= _cooldownCounter && _riflePulledOut)
       {
           _cooldownCounter = Time.time + 1f / _fireRate;
           Shoot();
       }

       Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
       Ray ray = _mainCamera.ScreenPointToRay(screenCenter);

       DebugTransform.position = ray.GetPoint(2f);

       /*if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, AimMask))
       {
           DebugTransform.position = raycastHit.point;
           raycastHit.dir
       }*/
    }

    private void TakeAim()
    {
        if (Input.GetButton(Controls.AIM) && _riflePulledOut)
        {
            _aiming = true;
            _desiredAimRigWeight = 1f;
            _controller.MouseSensitivity = _startSensitivity * 0.5f;
        }
        else
        {
            _aiming = false;
            _desiredAimRigWeight = 0f;
            _controller.MouseSensitivity = _startSensitivity;
        }

        _aimRig.weight = Mathf.SmoothDamp(_aimRig.weight, _desiredAimRigWeight, ref _aimRigWeightVelocity, 0.2f);
        _animator.SetBool(_aimingHash, _aiming);
    }

    private void SheatheUnsheatheRifle()
    {
        if (Input.GetButtonDown(Controls.WEAPON) && !_waiting)
        {
            _animator.SetBool(_riflePulledOutHash, !_riflePulledOut);
            _riflePulledOut = !_riflePulledOut;
            if (_riflePulledOut)
            {
                WeaponPivot.SetActive(true);
                _putSpeed = 0.5f;
                _glueSpeed = 0.5f;
                _animator.SetLayerWeight(1, 1);
                _desiredHipsRigWeight = 1;
                _desiredHandsRigWeight = 1;
                _weaponPullRig.weight = 1;
                _desiredPullRigWeight = 0;
            }
            else
            {
                _putSpeed = 0.5f;
                _glueSpeed = 0.2f;
                _desiredHipsRigWeight = 0;
                _desiredHandsRigWeight = 0;
                _desiredPullRigWeight = 1;
                StartCoroutine(WaitToChangeWeight());
            }
        }

        _handsRig.weight = Mathf.SmoothDamp(_handsRig.weight, _desiredHandsRigWeight, ref _handRigWeightVelocity, _putSpeed);
        _hipsRig.weight = Mathf.SmoothDamp(_hipsRig.weight, _desiredHipsRigWeight, ref _hipRigWeightVelocity, _putSpeed);
        _weaponPullRig.weight = Mathf.SmoothDamp(_weaponPullRig.weight, _desiredPullRigWeight, ref _pullRigweightVelocity, _glueSpeed);
    }

    IEnumerator WaitToChangeWeight()
    {
        _waiting = true;
        yield return new WaitForSeconds(ArmLayerWeightDelay);
        _waiting = false;
        WeaponPivot.SetActive(false);
        _animator.SetLayerWeight(1, 0);
    }
    
    private void Shoot()
    {
        StartCoroutine(Kickback());

        RaycastHit hit;
        if (Physics.Raycast(bulletOrigin.transform.position, bulletOrigin.transform.forward, out hit, _range, AimMask))
        {
            var obj = Instantiate(BulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
            obj.transform.position += obj.transform.forward/1000f;
        }
        
        GunPlayEvents.Instance.GunRecoil(_recoilX,_recoilY,_recoilZ);

    }

    IEnumerator Kickback()
    {
        _desiredKickbackRigWeight = 0.5f;
        yield return new WaitForSeconds(1 / (_fireRate*2f));
        _desiredKickbackRigWeight = 0f;
    }

}
