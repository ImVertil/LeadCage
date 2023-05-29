using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach(var rigid in rigidbodies)
        {
            rigid.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigid in rigidbodies)
        {
            rigid.isKinematic = false;
        }
        animator.enabled = false;
    }
}
