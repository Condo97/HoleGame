using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    [Header(" Elements ")]
    private bool isAlive;
    private float totalDamage;
    private float totalHP;

    [Header(" Events ")]
    public Action hpDepleted;

    public float hp
    {
        get
        {
            return totalHP - totalDamage;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GameManager.onStateChanged += GameStateChangedCallback;

        UpdateIsAlive();
    }

    private void OnDestroy()
    {
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Heal(float amount)
    {
        totalDamage -= amount;

        UpdateIsAlive();
    }

    public void Damage(float amount)
    {
        totalDamage += amount;

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

    private void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.BOSS)
        {
            totalHP = LevelManager.instance.GetTotalValuesToEat() * LevelManager.instance.GetCompletionPercentage();
        }
    }

}
