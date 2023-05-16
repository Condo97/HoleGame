using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSwitchSize : MonoBehaviour
{

    [Header(" Events ")]
    public static Action<float> onDiameterIncrease;

    private void Start()
    {
        HoleParentSize.onIncrease += HoleSizeUpdatedCallback;
    }

    private void OnDestroy()
    {
        HoleParentSize.onIncrease -= HoleSizeUpdatedCallback;
    }

    private void HoleSizeUpdatedCallback()
    {
        // Get hole size and send as Action
        if (gameObject.TryGetComponent(out CapsuleCollider collider))
        {
            onDiameterIncrease?.Invoke(collider.bounds.size.x);
        }
    }

}
