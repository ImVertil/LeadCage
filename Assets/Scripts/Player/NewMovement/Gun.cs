using UnityEngine;
using UnityEngine.Animations.Rigging;
public interface Gun {


    //GetFireRate
    //GetKickBack

    float FireRate {get;}
    bool GunPulledOut {get; set;}
    RigBuilder RigBuilder{set;}
    Animator Animator{set;}

    void Shoot();
    void TakeAim();
    void StopAim();
    void SheatheUnsheathe();

}
