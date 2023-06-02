using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Consumable
{

    event Action consumableUsed;

    public string GetConsumableName();
    public void ConsumableAction();

}
