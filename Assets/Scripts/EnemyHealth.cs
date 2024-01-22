using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float _timer;
    private float _regenerationRate;
    [SerializeField] private float _health;
    private MeleeEnemyAI meleeEnemy;
    private ShootingEnemyAI shootingEnemy;


    void Start()
    {
        meleeEnemy = GetComponent<MeleeEnemyAI>();
        shootingEnemy = GetComponent<ShootingEnemyAI>();
        _regenerationRate = 2;
        _health = 200;
    }



    void Update()
    {
        if(meleeEnemy != null)
        {
            if (_health < 200 && meleeEnemy.takeAction == false) {
                _timer += Time.deltaTime;
                if (_timer >= 2f / _regenerationRate)
                {
                    _health += 10;
                    _timer = 0f;
                }
            }

            
        } else if (shootingEnemy != null)
        {
            if (_health < 200 && shootingEnemy.takeAction == false)
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
            gameObject.SetActive(false);
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
