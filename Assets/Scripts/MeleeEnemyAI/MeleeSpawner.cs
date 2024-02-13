using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpawner : MonoBehaviour
{
    void Start()
    {
        PuzzleEvents.GeneratorActivated += SpawnMeleeEnemies; 
    }

    public void SpawnMeleeEnemies()
    {
        foreach(var obj in gameObject.GetComponentsInChildren<GameObject>())
        {
            if(!obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }
    }
}
