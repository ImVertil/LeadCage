using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    public bool _dead { get; set; }
    public float health { get; set; }
    public TextMeshProUGUI _text;




    private void Awake()
    {
        //health = _maxHealth;
        health = 3;
    }



    public void TakeDamage(float _damage)
    {
        health = Mathf.Clamp(health - _damage, 0, _maxHealth);
    }
}
