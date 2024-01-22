using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float _timer;
    [SerializeField] private float _regenerationRate = 2;
    [SerializeField] private float _maxHealth = 100;

    private float _health;
    private MeleeEnemyAI meleeEnemy;
    private ShootingEnemyAI shootingEnemy;


    void Start()
    {
        meleeEnemy = GetComponent<MeleeEnemyAI>();
        shootingEnemy = GetComponent<ShootingEnemyAI>();
        _health = _maxHealth;
    }



    void Update()
    {
        if(meleeEnemy != null)
        {
            if (_health < _maxHealth && meleeEnemy.takeAction == false) {
                _timer += Time.deltaTime;
                if (_timer >= 2f / _regenerationRate)
                {
                    _health += 10;
                    _timer = 0f;
                }
            }

            
        } else if (shootingEnemy != null)
        {
            if (_health < _maxHealth && shootingEnemy.takeAction == false)
            {
                _timer += Time.deltaTime;
                if (_timer >= 2f / _regenerationRate)
                {
                    _health += 10;
                    _timer = 0f;
                }
            }
        }

        //Debug.Log(_health);
        if(_health <= 0)
        {
            shootingEnemy.DisableEnemy();
        }
    }

    public void TakeDamage(float damage)
    {
        if (_health > 0)
        {
            _health -= damage;
        }

    }
}
