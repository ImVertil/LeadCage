using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEvents
{
    // PUZZLE 1
    public static Action<Cable> CableConnected;
    public static Action<Cable> CableDisconnected;
    public static Action<bool> Puzzle1Done;

    // PUZZLE 3
    public static Action<Puzzle3Lever> GeneratorShieldStatusChanged;
    public static Action<Puzzle3Valve> GeneratorPipeStatusChanged;
    public static Action GeneratorActivated;

    public static void OnCableConnected(Cable c)
    {
        CableConnected?.Invoke(c);
    }

    public static void OnCableDisconnected(Cable c)
    {
        CableDisconnected?.Invoke(c);
    }

    public static void OnPuzzle1Done(bool status)
    {
        Puzzle1Done?.Invoke(status);
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
