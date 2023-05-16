using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSize : MonoBehaviour
{

    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    public void ConsumableSupercharge(float time, float sizeMultiplier)
    {
        StartCoroutine(ConsumableSuperchargeCoroutine(time, sizeMultiplier));
    }

    public IEnumerator ConsumableSuperchargeCoroutine(float time, float sizeMultiplier)
    {
        // Supercharges the hole for a few seconds, currently called directly from consumable button
        float originalScale = transform.localScale.x;
        float targetScale = originalScale * sizeMultiplier;

        UpdateScale(targetScale);

        yield return new WaitForSeconds(time);

        UpdateScale(targetScale - originalScale);
    }

    private void UpdateScale(float targetScale)
    {
        LeanTween.scale(transform.gameObject, targetScale * Vector3.one, 0.2f * Time.deltaTime * 60)
            .setEase(LeanTweenType.easeInOutBack); // The ease can also be an AnimationCurve which can be set as a SerializeField and custom made in Unity
    }

}
