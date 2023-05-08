using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEvents
{
    public static Action<Cable> DoorCableChanged;
    public static Action<Cable> DoorLockCableChanged;

    public static void OnDoorCableChanged(Cable c)
    {
        DoorCableChanged?.Invoke(c);
    }

    public static void OnDoorLockCableChanged(Cable c)
    {
        DoorLockCableChanged?.Invoke(c);
    }
}
