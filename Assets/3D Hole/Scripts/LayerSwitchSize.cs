using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSwitchSize : MonoBehaviour
{

    [Header(" Events ")]
    public static Action<float> onDiameterIncrease;

    private void Awake()
    {
        HoleParentSize.onIncreaseComplete += HoleSizeUpdatedCallback;
    }

    private void OnDestroy()
    {
        HoleParentSize.onIncreaseComplete -= HoleSizeUpdatedCallback;
    }

    private void HoleSizeUpdatedCallback(float holeScale)
    {
        // Get hole size and send as Action
        if (gameObject.TryGetComponent(out CapsuleCollider collider))
        {
            onDiameterIncrease?.Invoke(collider.bounds.size.x);
        }
        //if (gameObject.TryGetComponent(out Renderer renderer))
        //{
        //    onDiameterIncrease?.Invoke(renderer.bounds.size.x);
        //}
    }

}
