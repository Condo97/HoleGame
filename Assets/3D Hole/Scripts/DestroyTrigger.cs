using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{

    //[Header(" Elements ")]
    //[SerializeField] private PlayerSize playerSize;

    [Header(" Events ")]
    public static Action<Collectible> collectibleCollected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        // Try get Collectable from other, and if successful, do something eaten for player with objectSize argument so that smaller and larger objects can be worth different amounts and destroy the object
        if (other.TryGetComponent(out Collectible collectible))
        {
            //playerSize.CollectableCollected(collectible.GetSize());
            collectibleCollected?.Invoke(collectible);
            Destroy(other.gameObject);
        }
    }

}
