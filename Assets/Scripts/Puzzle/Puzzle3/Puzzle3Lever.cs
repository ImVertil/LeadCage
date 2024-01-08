<<<<<<< Updated upstream
=======
using System;
>>>>>>> Stashed changes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3Lever : MonoBehaviour
{
    [SerializeField] private Puzzle3Switch[] _switches;
    [SerializeField] private string _code = "1100110011";
    [SerializeField] private MeshRenderer _greenLampMr;
    [SerializeField] private MeshRenderer _redLampMr;
    //
    [SerializeField] private Material _greenMatOn;
    [SerializeField] private Material _greenMatOff;
    [SerializeField] private Material _redMatOn;
    [SerializeField] private Material _redMatOff;

    private Animator _animator;
    private bool _isMoving = false;
    private bool _isCompleted = false;
    private char[] _codeArr;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _codeArr = _code.ToCharArray();
    }

    private void OnMouseDown()
    {
        if(!_isMoving && !_isCompleted)
        {
            StartCoroutine(PlayLeverAnim());
        }
    }

    private bool CheckPuzzle()
    {
        for(int i=0; i<_switches.Length; i++)
        {
            if (_switches[i].isOn != (_codeArr[i] == '1'))
            {
                return false;
            }
        }
        return true; 
    }

    private void DisableAllSwitches()
    {
        for (int i=0 ; i<_switches.Length; i++)
        {
            _switches[i].DisableSwitch();
        }
    }

    private IEnumerator PlayLeverAnim()
    {
        _isMoving = true;

        _animator.Play(Animator.StringToHash("Puzzle3_Lever"));
        yield return null;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        if (CheckPuzzle())
        {
            DisableAllSwitches();
            _animator.Play(Animator.StringToHash("Puzzle3_Voltage"), 1);
            yield return null;
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(1).length);
            _greenLampMr.material = _greenMatOn;
            _isCompleted = true;
        }
        else
        {
            _redLampMr.material = _redMatOn;
            yield return new WaitForSeconds(2);
            _redLampMr.material = _redMatOff;
            _animator.Play(Animator.StringToHash("Puzzle3_Lever_R"));
        }

        _isMoving = false;
        yield return null;
    }
}
