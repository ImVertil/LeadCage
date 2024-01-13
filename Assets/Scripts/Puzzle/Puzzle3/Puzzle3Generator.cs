using System.Collections.Generic;
using UnityEngine;

public class Puzzle3Generator : MonoBehaviour
{
    [SerializeField] private int _amountOfShields = 2;
    [SerializeField] private int _amountOfPipes = 4;

    private HashSet<Puzzle3Lever> _part1done = new();
    private HashSet<Puzzle3Valve> _part2done = new();
    // Start is called before the first frame update
    void Awake()
    {
        PuzzleEvents.GeneratorShieldStatusChanged += CheckPart1;
        PuzzleEvents.GeneratorPipeStatusChanged += CheckPart2;
    }

    public void CheckPart1(Puzzle3Lever p)
    {
        if(_part1done.Contains(p))
        {
            _part1done.Remove(p);
        }
        else
        {
            _part1done.Add(p);
        }
    }

    public void CheckPart2(Puzzle3Valve p)
    {
        if (_part2done.Contains(p))
        {
            _part2done.Remove(p);
        }
        else
        {
            _part2done.Add(p);
        }
    }

    public void ActivateGenerator()
    {
        if(_part1done.Count == _amountOfShields && _part2done.Count == _amountOfPipes)
        {
            Debug.Log("ON");
        }
        else
        {
            Debug.Log("OFF");
        }
    }
}
