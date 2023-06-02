using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerConsumable : MonoBehaviour, Consumable
{

    [Header(" Elements ")]
    [SerializeField] private int timerTime;
    [SerializeField] private TimerManager timerManager;

    [Header(" Static Events ")]
    public static Action<int> timerUsed;

    public event Action consumableUsed { add => _consumableUsed += value; remove => _consumableUsed -= value; }
    private Action _consumableUsed;


    public string GetConsumableName()
    {
        return DataManager.ConsumableName.Timer;
    }

    public void ConsumableAction()
    {
        timerManager.ConsumableTimer(timerTime);

        _consumableUsed?.Invoke();
    }

}
