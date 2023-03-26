using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleController : MonoBehaviour
{
    private Animator _animator;

    private int _riflePulledOutHash;

    private bool _riflePulledOut = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _riflePulledOutHash = Animator.StringToHash("RiflePulledOut");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Controls.WEAPON))
        {
            _animator.SetBool(_riflePulledOutHash, !_riflePulledOut);
            _riflePulledOut = !_riflePulledOut;
            if(_riflePulledOut)
                _animator.SetLayerWeight(1,1);
            else
                _animator.SetLayerWeight(1, 0);
            
        }
    }
}
