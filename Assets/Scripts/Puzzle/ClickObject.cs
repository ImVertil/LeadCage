using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    private bool _isDefault;
    private Animator _animator;
    private int _cameraDefault = Animator.StringToHash("CameraDefault");
    private int _cameraInstruction = Animator.StringToHash("CameraInstruction");

    void Start()
    {
        _isDefault = true;
        _animator = GetComponentInParent<Animator>();
    }

    private void OnMouseDown()
    {
        Debug.Log("MOUSE DOWN");
        if(_isDefault)
        {
            Debug.Log("TRUE -> INSTR");
            _animator.SetTrigger(_cameraInstruction);
            _isDefault = false;
        }
        else
        {
            Debug.Log("FALSE -> DEFAULT");
            _animator.SetTrigger(_cameraDefault);
            _isDefault = true;
        }
    }
}
