using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    private float current = 3.0f;

    public float Current
    {
        get
        {
            return current;
        }
    }

    void Start()
    {
        current = GetComponent<Balance>().characterBalanceData.Health;
    }

    public void ApplyDamage(float damage)
    {
        current -= damage;
        if (current < 0)
            current = 0;
    }

}
