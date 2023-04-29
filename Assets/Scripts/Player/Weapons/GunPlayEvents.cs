using System;
using UnityEngine;

namespace Player.Weapons
{
    public class GunPlayEvents
    {
        public static Action<float, float, float> OnGunRecoil;
        public static Action<Gun> OnGunEquip;

        public static void GunRecoil(float recoilX, float recoilY, float recoilZ) => OnGunRecoil?.Invoke(recoilX, recoilY, recoilZ);
        public static void GunEquip(Gun gun) => OnGunEquip?.Invoke(gun);
    }
}