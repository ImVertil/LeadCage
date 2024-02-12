using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMetalShield : MonoBehaviour
{
    [SerializeField] private Puzzle3Switch[] _switches;
    [SerializeField] private string _code;
    [SerializeField] private GameObject _lever; // change to scripts 
    [SerializeField] private GameObject _greenLamp;
    [SerializeField] private GameObject _redLamp;
    //
    [SerializeField] private Material _greenMatOn;
    [SerializeField] private Material _greenMatOff;
    [SerializeField] private Material _redMatOn;
    [SerializeField] private Material _redMatOff;
    //

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check()
    {

    }
}
