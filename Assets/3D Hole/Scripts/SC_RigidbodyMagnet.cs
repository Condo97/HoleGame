using System.Collections.Generic;
using UnityEngine;

public class SC_RigidbodyMagnet : MonoBehaviour
{
    public float magnetForce = 100;

    List<Rigidbody> caughtRigidbodies = new List<Rigidbody>();
    List<Rigidbody> enteredMagnetOut = new List<Rigidbody>();

    private void Awake()
    {
        // Subscribe to SC_RigidbodyMagnetOutSwitch enterTrigger and exitTrigger
        SC_RigidbodyMagnetOutSwitch.enteredTrigger += EnteredLayerSwitchCallback;
        SC_RigidbodyMagnetOutSwitch.exitedTrigger += ExitedLayerSwitchCallback;

    }

    private void OnDestroy()
    {
        // Unsubscribe from SC_RigidbodyMagnetOutSwitch enterTrigger and exitTrigger
        SC_RigidbodyMagnetOutSwitch.enteredTrigger -= EnteredLayerSwitchCallback;
        SC_RigidbodyMagnetOutSwitch.exitedTrigger -= ExitedLayerSwitchCallback;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < caughtRigidbodies.Count; i++)
        {
            if (caughtRigidbodies[i] == null)
                caughtRigidbodies.RemoveAt(i);
            else if (!enteredMagnetOut.Contains(caughtRigidbodies[i]))
                caughtRigidbodies[i].velocity = (transform.position - (caughtRigidbodies[i].transform.position + caughtRigidbodies[i].centerOfMass)) * magnetForce * Time.deltaTime;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Collectible c) && other.TryGetComponent(out Rigidbody r))
        {
            // Only add if collectible is not softDisabled
            if (!c.GetIsSoftDisabled())
            {
                if (!caughtRigidbodies.Contains(r))
                {
                    //Add Rigidbody
                    caughtRigidbodies.Add(r);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody r))
        {
            if (caughtRigidbodies.Contains(r))
            {
                //Remove Rigidbody
                caughtRigidbodies.Remove(r);
            }
        }
    }

    private void EnteredLayerSwitchCallback(Collider other)
    {
        // Get rigidbody from other
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();

        // Remove the rigidbody from caughtRigidbodies if it is in there
        caughtRigidbodies.Remove(rigidbody);

        // Add rigidbody to enteredMagnetOut list if it is a Collectible and not softDisabled
        if (other.gameObject.TryGetComponent(out Collectible c))
            if (!c.GetIsSoftDisabled())
                enteredMagnetOut.Add(rigidbody);
    }

    private void ExitedLayerSwitchCallback(Collider other)
    {
        // Remove rigidbody from enteredMagnetOut list
        enteredMagnetOut.Remove(other.GetComponent<Rigidbody>());
    }

}