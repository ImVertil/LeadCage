using UnityEngine;

public class ClickObject : MonoBehaviour
{
    private bool _isDefault;
    private Animator _animator;
    private int _cameraDefault = Animator.StringToHash("CameraDefault");
    private int _cameraInstruction = Animator.StringToHash("CameraInstruction");

    private void Start()
    {
        _isDefault = true;
        _animator = GetComponentInParent<Animator>();
    }

    private void OnMouseDown()
    {
        if(_isDefault)
        {
            _animator.SetTrigger(_cameraInstruction);
            _isDefault = false;
        }
        else
        {
            _animator.SetTrigger(_cameraDefault);
            _isDefault = true;
        }
    }
}
