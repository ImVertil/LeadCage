public abstract class Rifle : Gun {


    void Shoot()
    {
        SoundManager.Instance.PlaySound(Sound.Shoot, transform, false);
        StartCoroutine(Kickback());

        RaycastHit hit;
        if (Physics.Raycast(bulletOrigin.transform.position, bulletOrigin.transform.forward, out hit, _range, AimMask))
        {
            var obj = Instantiate(BulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
            obj.transform.position += obj.transform.forward/1000f;
        }

        GunPlayEvents.Instance.GunRecoil(_recoilX, _recoilY, _recoilZ);
    }


}