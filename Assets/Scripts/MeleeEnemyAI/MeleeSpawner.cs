using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpawner : MonoBehaviour
{
    void Awake()
    {
        PuzzleEvents.GeneratorActivated += SpawnMeleeEnemies; 
    }

    public void SpawnMeleeEnemies()
    {
        foreach(var enemy in gameObject.GetComponentsInChildren<MeleeEnemyAI>(true))
        {
            if(!enemy.gameObject.activeSelf)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }
}
