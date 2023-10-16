using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    private Transform _defaultPos;
    private Transform _viewManualPos;
    private bool _isDefault;

    void Start()
    {
        _defaultPos = Camera.main.transform;
        _isDefault = true;
    }

    private void OnMouseDown()
    {
        if(_isDefault)
        {
            
        }
    }
}
