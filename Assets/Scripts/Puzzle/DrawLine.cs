using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _lr;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _lr.SetPosition(0,_startPos.position);
        _lr.SetPosition(1,_endPos.position);
    }
}
