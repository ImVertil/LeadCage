using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTemplate : MonoBehaviour, IInteractable //remember to add the interface IInteractable as seen here
{
    public float MaxRange { get { return maxRange; } }

    private const float maxRange = 5f; //the furthest you can be for the below functions to fire in unity units, set this to public if you want to tweak it for every object that uses this script, set this to private const if you want it to be the same for all objects with this script

    public void OnStartLook()
    {
        //What happens when you mouse over the object
    }

    public void OnEndLook()
    {
        //What happens when you put the cursor off the object
    }

    public void OnInteract()
    {
        //What happens when you press the interaction key and in range
    }
}

