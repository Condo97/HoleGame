using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetConsumable : MonoBehaviour, Consumable
{

    [Header(" Elements ")]
    [SerializeField] private MagnetManager magnetManager;

    public event Action consumableUsed { add => _consumableUsed += value; remove => _consumableUsed -= value; }
    private Action _consumableUsed;


    public string GetConsumableName()
    {
        return DataManager.ConsumableName.Magnet;
    }

    public void ConsumableAction()
    {
        magnetManager.ConsumableMagnet();

        _consumableUsed?.Invoke();
    }

}
