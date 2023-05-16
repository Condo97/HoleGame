using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    [Header(" Elements ")]
    private bool isAlive;

    [Header(" Settings ")]
    [SerializeField] private float hp;

    [Header(" Events ")]
    public Action hpDepleted;

    // Start is called before the first frame update
    void Start()
    {
        UpdateIsAlive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Heal(float amount)
    {
        hp += amount;

        UpdateIsAlive();
    }

    public void Damage(float amount)
    {
        hp -= amount;

        UpdateIsAlive();
    }

    private void UpdateIsAlive()
    {
        if (hp <= 0)
        {
            isAlive = false;

            hpDepleted?.Invoke();
        } else
        {
            isAlive = true;
        }
    }

}
