using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    [SerializeField] float healthSet;
    private float health;
    void Start()
    {
        health = healthSet;
        healthText.text = "HP: " + health.ToString();
    }

    public void TakeDamage(float damage) {
        if (health > 0)
        {
            health -= damage;
            healthText.text = "HP: " + health.ToString();
        }
        
    }
}
