using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    [SerializeField] float health = 100f;
    // Start is called before the first frame update
    void Start()
    {
        healthText.text = health.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("playert hp " + health);


    }

    public void TakeDamage(float damage) {
        if (health > 0)
        {
            health -= damage;
            healthText.text = health.ToString();

        }
        
    }
}
