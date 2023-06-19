using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    [SerializeField] float healthSet;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        health = healthSet;
        healthText.text = "HP: " + health.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "HP: " + health.ToString();


    }

    public void TakeDamage(float damage) {
        if (health > 0)
        {
            health -= damage;
            healthText.text = health.ToString();

        }
        
    }
}
