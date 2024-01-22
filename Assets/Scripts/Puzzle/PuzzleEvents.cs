using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEvents
{
    // PUZZLE 1
    public static Action<Cable> DoorCableChanged;
    public static Action<Cable> DoorLockCableChanged;

    // PUZZLE 3
    public static Action<Puzzle3Lever> GeneratorShieldStatusChanged;
    public static Action<Puzzle3Valve> GeneratorPipeStatusChanged;
    public static Action GeneratorActivated;

    public static void OnDoorCableChanged(Cable c)
    {
        DoorCableChanged?.Invoke(c);
    }

    public static void OnDoorLockCableChanged(Cable c)
    {
        DoorLockCableChanged?.Invoke(c);
    }

    public static void OnGeneratorShieldStatusChanged(Puzzle3Lever completed)
    {
        GeneratorShieldStatusChanged?.Invoke(completed);
    }

    public static void OnGeneratorPipeStatusChanged(Puzzle3Valve completed)
    {
        GeneratorPipeStatusChanged?.Invoke(completed);
    }

    public static void OnGeneratorActivated()
    {
        GeneratorActivated?.Invoke();
    }
}
