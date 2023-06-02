using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperchargeConsumable : MonoBehaviour, Consumable
{

    [Header(" Elements ")]
    [SerializeField] private int superchargeScaleSteps;
    [SerializeField] private HoleParentSize holeParentSize;

    public event Action consumableUsed { add => _consumableUsed += value; remove => _consumableUsed -= value; }
    private Action _consumableUsed;


    public string GetConsumableName()
    {
        return DataManager.ConsumableName.Supercharge;
    }

    public void ConsumableAction()
    {
        // Do the action this way rather than having the target subscribe to this event.. Less coupling and it allows for the Action to not be static and also not have to have parameters and therefore specified in the Consumable interface!
        holeParentSize.ConsumableSupercharge(superchargeScaleSteps);

        _consumableUsed?.Invoke();
    }

}
