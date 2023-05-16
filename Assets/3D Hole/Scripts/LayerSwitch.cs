using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSwitch : MonoBehaviour
{

    [Header(" Settings ")]
    [SerializeField] private string enterLayer;
    [SerializeField] private string exitLayer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Collectible collectible))
        {
            if (!collectible.GetIsSoftDisabled())
            {
                // Only switch layers to collect if object is not soft disabled meaning it can be collected by the hole
                other.gameObject.layer = LayerMask.NameToLayer(enterLayer);

                //other.gameObject.GetComponent<Rigidbody>().WakeUp();
                other.gameObject.GetComponent<Rigidbody>().sleepThreshold = 0;
            }

            collectible.TEST_PrintRendererBounds();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //TODO: Should there be a check to see if the gameObject layer is enterLayer before switching to exitLayer?
        other.gameObject.layer = LayerMask.NameToLayer(exitLayer);
    }

}
