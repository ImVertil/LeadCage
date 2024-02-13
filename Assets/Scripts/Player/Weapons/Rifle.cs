using UnityEngine;
using System.Collections;
using Player.Weapons;
using UnityEngine.Animations.Rigging;

public class Rifle : MonoBehaviour, Gun {

    [SerializeField] Transform bulletOrigin;
    [SerializeField] private GameObject BulletHolePrefab;
    [SerializeField] private LayerMask AimMask;
    [SerializeField] GameObject WeaponPivot;
    [SerializeField] float fireRate;
    [SerializeField] float range;
    [SerializeField] float _recoilX;
    [SerializeField] float _recoilY;
    [SerializeField] float _recoilZ;
    [SerializeField] [Range(0,1)] float _kickbackStrength;
    [SerializeField] float _kickbackSpeed;
    [SerializeField] PlayerController _pc;
    [SerializeField] float damage;

    public float FireRate {get {return fireRate;}}
    public bool GunPulledOut {get {return _riflePulledOut;} set {_riflePulledOut = value;}}

    private bool _waiting = false;
    private bool _riflePulledOut = false;
    private float _startSensitivity;

    private int _riflePulledOutHash;
    
    public RigBuilder RigBuilder{get; set;}
    public Animator Animator{get; set;}
    private Rig _hipsRig;
    private Rig _handsRig;
    private Rig _aimRig;
    private Rig _weaponPullRig;
    private Rig _kickbackRig;

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

    private float _putSpeed= 0.5f;
    private float _glueSpeed= 0.2f;
    private float ArmLayerWeightDelay= 0.5f;

    private Camera _mainCamera;
    private float _fov;
    [SerializeField] [Range(0,160)] private float aimFov;
    private float _fovVelocity;
    private float _desiredFov;

    private void Start()
    {
        _startSensitivity = 50; // i have no idea but it has to be like this rn
        _riflePulledOutHash = Animator.StringToHash("RiflePulledOut");
        _hipsRig = RigBuilder.layers[0].rig;
        _handsRig = RigBuilder.layers[4].rig;
        _aimRig = RigBuilder.layers[1].rig;
        _weaponPullRig = RigBuilder.layers[2].rig;
        _kickbackRig = RigBuilder.layers[3].rig;

        _mainCamera = Camera.main;
        _fov = _mainCamera.fieldOfView;
    }

    private void OnEnable()
    {
        GunPlayEvents.GunEquip(this);
    }

    private void Update()
    {
        _handsRig.weight = Mathf.SmoothDamp(_handsRig.weight, _desiredHandsRigWeight, ref _handRigWeightVelocity, _putSpeed);
        _hipsRig.weight = Mathf.SmoothDamp(_hipsRig.weight, _desiredHipsRigWeight, ref _hipRigWeightVelocity, _putSpeed);
        _weaponPullRig.weight = Mathf.SmoothDamp(_weaponPullRig.weight, _desiredPullRigWeight, ref _pullRigweightVelocity, _glueSpeed);
        _kickbackRig.weight = Mathf.SmoothDamp(_kickbackRig.weight, _desiredKickbackRigWeight, ref _kickbackRigWeightVelocity, 1/_kickbackSpeed);
        _aimRig.weight = Mathf.SmoothDamp(_aimRig.weight, _desiredAimRigWeight, ref _aimRigWeightVelocity, 0.2f);
        _mainCamera.fieldOfView = Mathf.SmoothDamp(_mainCamera.fieldOfView, _desiredFov, ref _fovVelocity, 0.4f);
    }

    public void Shoot()
    {
        //Debug.Log("SHOOOOTOTOTOTOTOTOTTOTO");
        var randPitch = Random.Range(0.98f, 1.02f);
        SoundManager.Instance.PlaySound(Sound.Shoot, transform, false, null, randPitch);
        StartCoroutine(Kickback());

        RaycastHit hit;
        if (Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out hit, range, AimMask))
        {
            var enemyHealth = hit.collider.gameObject.GetComponent<EnemyHealth>();
            var shootingEnemyAI = hit.collider.gameObject.GetComponent<ShootingEnemyAI>();
            var meleeEnemyAI = hit.collider.gameObject.GetComponent<MeleeEnemyAI>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);

            }
            if (shootingEnemyAI != null)
            {
                shootingEnemyAI.takeAction = true;

            }
            if (meleeEnemyAI != null)
            {
                meleeEnemyAI.takeAction = true;

            }

            // to be replaced
            //var obj = Instantiate(BulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
            //obj.transform.position += obj.transform.forward/1000f;
        }

        GunPlayEvents.GunRecoil(_recoilX, _recoilY, _recoilZ);
    }

    public void SheatheUnsheathe()
    {
        if (!_waiting)
        {
            SoundManager.Instance.PlaySound(Sound.Holster, transform, false);
            Animator.SetBool(_riflePulledOutHash, !_riflePulledOut);
            _riflePulledOut = !_riflePulledOut;
            if (_riflePulledOut)
            {
                WeaponPivot.SetActive(true);
                _putSpeed = 0.6f;
                _glueSpeed = 0.5f;
                Animator.SetLayerWeight(1, 1);
                _desiredHipsRigWeight = 1;
                _desiredHandsRigWeight = 1;
                _weaponPullRig.weight = 1;
                _desiredPullRigWeight = 0;
                TakeAim();
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

    }

    public void TakeAim()
    {
        _desiredAimRigWeight = 1f;
        _pc.MouseSensitivity = _startSensitivity * 0.5f;
        _desiredFov = aimFov;
    }

    public void StopAim()
    {
        _desiredAimRigWeight = 0f;
        _pc.MouseSensitivity = _startSensitivity;
        _desiredFov = _fov;
    }

    IEnumerator WaitToChangeWeight()
    {
        _waiting = true;
        yield return new WaitForSeconds(ArmLayerWeightDelay);
        _waiting = false;
        WeaponPivot.SetActive(false);
        Animator.SetLayerWeight(1, 0);
    }

    IEnumerator Kickback()
    {
        _desiredKickbackRigWeight = _kickbackStrength;
        yield return new WaitForSeconds(1 / _kickbackSpeed);
        _desiredKickbackRigWeight = 0f;
    }


}