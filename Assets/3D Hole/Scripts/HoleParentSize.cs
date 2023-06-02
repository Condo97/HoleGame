using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleParentSize : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Image fillImage;
    private int currentScaleStep;
    private float totalAdjustedValuesEaten; // Sum of objectSize values eaten adjusted with the additionalPower from consumables

    [Header(" Scale Step Settings ")]
    [SerializeField] private float initialScale;
    [SerializeField] private float percentToIncreaseScalePerStep; // The percent to increase the scale per step, i.e. 1 starting scale, 0.5 percent to increase, means 1, 1.5, 2.25, etc

    [Header(" Scale Increase Threshold Settings ")]
    [SerializeField] private float startScaleIncreaseThreshold; // Threshold for amount scaleValue should be before actually scaling hole
    [SerializeField] private float percentToIncreaseScaleIncreaseThresholdPerStep; // The amount to increase the "cost" for the hole, i.e. 10 starting threshold, 0.5 percent to increase, means 10, 15, 22.5, etc
    //[SerializeField] private float scaleExponentialStep; // Defined interval for increase in hole size, x^?
    //private float scaleIncreaseThreshold;

    [Header(" Power ")]
    private float additionalPower;

    [Header(" Events ")]
    public static Action<float> onIncrease;
    public static Action<float> onIncreaseComplete;


    private void Awake()
    {
        //SuperchargeConsumable.superchargeUsed += ConsumableSupercharge;
        UpgradesManager.onDataLoaded += UpgradesDataLoadedCallback;
        CollectedManager.collected += CollectedCallback;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        fillImage.fillAmount = 0;

        UpgradesManager.onSizePurchased += SizePurchasedCallback;
        UpgradesManager.onPowerPurchased += PowerPurchasedCallback;
        GameManager.onStateChanged += GameStateChangedCallback;

        // Call UpdateScale after a frame to send an onIncrease event
        yield return null;
        UpdateScale(0f);
    }

    private void OnDestroy()
    {
        UpgradesManager.onSizePurchased -= SizePurchasedCallback;
        UpgradesManager.onPowerPurchased -= PowerPurchasedCallback;
        UpgradesManager.onDataLoaded -= UpgradesDataLoadedCallback;
        CollectedManager.collected -= CollectedCallback;
        //SuperchargeConsumable.superchargeUsed -= ConsumableSupercharge;
        GameManager.onStateChanged -= GameStateChangedCallback;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void CollectedCallback(Collectible collectible)
    {
        // Add objectSize times 1 + multiplier to scaleValue, which holds a sum of all the objectSize values eaten
        totalAdjustedValuesEaten += collectible.GetValue() * (1 + additionalPower);

        // Calculate scaleIncreaseThreshold
        float scaleIncreaseThreshold = startScaleIncreaseThreshold * Mathf.Pow(1 + percentToIncreaseScaleIncreaseThresholdPerStep, currentScaleStep);

        // If threshold is met, increase size and set scale value to scale value modulo scaleIncreaseThreshold
        if(totalAdjustedValuesEaten >= scaleIncreaseThreshold)
        {
            IncreaseScale();
            totalAdjustedValuesEaten = totalAdjustedValuesEaten % scaleIncreaseThreshold; // This is actually genius from Tabsil Games, imagine the player was at 8, threshold is 10, and just consumed 5 worth, the scaleValue should be set to 3, though this will make it so that the hole doesn't increase multiple times for an object say that is 20 or 40 or something
        }

        //UpdateFillDisplay();
    }

    private void IncreaseScale()
    {
        // Increase the size by a scale step
        currentScaleStep++;

        UpdateScale();
    }

    private void UpdateScale()
    {
        UpdateScale(0.2f);
    }

    private void UpdateScale(float animationDuration)
    {
        // Calculate targetScale and update scale
        float targetScale = (initialScale * Mathf.Pow(1 + percentToIncreaseScalePerStep, currentScaleStep));
        LeanTween.value(transform.localScale.x, targetScale, animationDuration * Time.deltaTime * 60)
            .setOnUpdate((value) =>
            {
                // Calculate targetScale and update player transform local scale
                Vector3 targetScaleVector = new Vector3(value, transform.localScale.y, value);
                transform.localScale = targetScaleVector;
            })
            .setOnComplete(() =>
            {
                IEnumerator SendOnIncreaseAfterDelay()
                {
                    yield return null;

                    onIncreaseComplete?.Invoke(targetScale);
                }

                StartCoroutine(SendOnIncreaseAfterDelay());
            });

        //LeanTween.scaleX(transform.gameObject, targetScale, 0.2f * Time.deltaTime * 60)
        //    .setEase(LeanTweenType.easeInOutBack);
        //LeanTween.scaleZ(transform.gameObject, targetScale, 0.2f * Time.deltaTime * 60)
        //    .setEase(LeanTweenType.easeInOutBack); // The ease can also be an AnimationCurve which can be set as a SerializeField and custom made in Unity

        onIncrease?.Invoke(targetScale);
    }

    //private void UpdateFillDisplay(float scaleIncreaseThreshold)
    //{
    //    // Set the fillImage fillAmount to an amount corresponding to the scaleValue to scaleThreshold between 0 and 1 using LeanTween
    //    float targetFillAmount = totalAdjustedValuesEaten / scaleIncreaseThreshold;

    //    LeanTween.value(fillImage.fillAmount, targetFillAmount, 0.2f * Time.deltaTime * 60)
    //        .setOnUpdate((value) => fillImage.fillAmount = value);
    //}

    public void ConsumableSupercharge(int scaleSteps)
    {
        // Doubles the hole size
        //float originalDiameter = transform.localScale.x;
        //float targetDiameter = originalDiameter * sizeMultiplier;

        currentScaleStep += scaleSteps;

        UpdateScale();
    }

    private void SizePurchasedCallback()
    {
        IncreaseScale();
    }

    private void PowerPurchasedCallback()
    {
        additionalPower += UpgradesManager.instance.GetPowerUpgradeAddition();
    }

    private void UpgradesDataLoadedCallback(int timerLevel, int sizeLevel, int powerLevel)
    {
        // Update size
        currentScaleStep = sizeLevel + 1;
        UpdateScale();

        // Update power
        additionalPower = powerLevel;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.BOSS:
                // Make hole bigger
                UpdateScale(4);
                break;
        }
    }

}
