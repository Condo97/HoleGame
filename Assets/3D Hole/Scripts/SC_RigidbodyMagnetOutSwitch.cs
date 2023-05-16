using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_RigidbodyMagnetOutSwitch : MonoBehaviour
{

    [Header(" Events ")]
    public static Action<Collider> enteredTrigger;
    public static Action<Collider> exitedTrigger;


    private void OnTriggerEnter(Collider other)
    {
        enteredTrigger?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        exitedTrigger?.Invoke(other);
    }

}
