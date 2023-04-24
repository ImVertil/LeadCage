using System;

using UnityEngine;

namespace Player.Weapons
{
    public class GunPlayEvents : MonoBehaviour
    {
        public static GunPlayEvents Instance;
        
        //Akcje
        public Action<float, float, float> OnGunRecoil;
        public Action<Gun> OnGunEquip;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        public void GunRecoil(float recoilX, float recoilY, float recoilZ)
        {
            OnGunRecoil?.Invoke(recoilX,recoilY,recoilZ);
        }

        public void GunEquip(Gun gun)
        {
            OnGunEquip?.Invoke(gun);
        }

    }
}