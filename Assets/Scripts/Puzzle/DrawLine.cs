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

    void Update()
    {
        _lr.SetPosition(0,_startPos.localPosition);
        _lr.SetPosition(1,_endPos.localPosition);
    }
}
