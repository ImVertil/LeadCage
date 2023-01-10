using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Health _PlayerHp;
    
    public TextMeshProUGUI _text;

    private void Update()
    {
        _text.text = _PlayerHp.health.ToString();

        if (Input.GetKeyDown(KeyCode.L))
        {
            _PlayerHp.TakeDamage(1);
        }
    }
}
