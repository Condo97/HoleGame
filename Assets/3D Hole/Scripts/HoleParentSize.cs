using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleParentSize : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Image fillImage;

    [Header(" Settings ")]
    [SerializeField] private float scaleIncreaseThreshold; // Threshold for amount scaleValue should be before actually scaling hole
    [SerializeField] private float scaleStep; // Defined interval for hole to increase
    private float scaleValue; // Sum of objectSize values eaten

    [Header(" Power ")]
    private float powerMultiplier;

    [Header(" Events ")]
    public static Action onIncrease;


    private void Awake()
    {
        ConsumableManager.superchargeUsed += ConsumableSupercharge;
        UpgradesManager.onDataLoaded += UpgradesDataLoadedCallback;
        CollectedManager.collected += CollectedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        fillImage.fillAmount = 0;

        UpgradesManager.onSizePurchased += SizePurchasedCallback;
        UpgradesManager.onPowerPurchased += PowerPurchasedCallback;
        GameManager.onStateChanged += GameStateChangedCallback;

        // Call UpdateScale to send an onIncrease event
        UpdateScale(transform.localScale.x);
    }

    private void OnDestroy()
    {
        UpgradesManager.onSizePurchased -= SizePurchasedCallback;
        UpgradesManager.onPowerPurchased -= PowerPurchasedCallback;
        UpgradesManager.onDataLoaded -= UpgradesDataLoadedCallback;
        CollectedManager.collected -= CollectedCallback;
        ConsumableManager.superchargeUsed -= ConsumableSupercharge;
        GameManager.onStateChanged -= GameStateChangedCallback;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void CollectedCallback(Collectible collectible)
    {
        // Add objectSize times 1 + multiplier to scaleValue, which holds a sum of all the objectSize values eaten
        scaleValue += collectible.GetValue() * (1 + powerMultiplier);

        // If threshold is met, increase size and set scale value to scale value modulo scaleIncreaseThreshold
        if(scaleValue >= scaleIncreaseThreshold)
        {
            IncreaseScale();
            scaleValue = scaleValue % scaleIncreaseThreshold; // This is actually genius from Tabsil Games, imagine the player was at 8, threshold is 10, and just consumed 5 worth, the scaleValue should be set to 3, though this will make it so that the hole doesn't increase multiple times for an object say that is 20 or 40 or something
        }

        UpdateFillDisplay();
    }

    private void IncreaseScale()
    {
        // Increase the size by a scale step
        float targetDiameter = transform.localScale.x + scaleStep;

        UpdateScale(targetDiameter);
    }

    private void UpdateScale(float holeScale)
    {
        LeanTween.scale(transform.gameObject, holeScale * Vector3.one, 0.2f * Time.deltaTime * 60)
            .setEase(LeanTweenType.easeInOutBack); // The ease can also be an AnimationCurve which can be set as a SerializeField and custom made in Unity

        onIncrease?.Invoke();
    }

    private void UpdateFillDisplay()
    {
        // Set the fillImage fillAmount to an amount corresponding to the scaleValue to scaleThreshold between 0 and 1 using LeanTween
        float targetFillAmount = scaleValue / scaleIncreaseThreshold;

        LeanTween.value(fillImage.fillAmount, targetFillAmount, 0.2f * Time.deltaTime * 60)
            .setOnUpdate((value) => fillImage.fillAmount = value);
    }

    public void ConsumableSupercharge(float time, float sizeMultiplier)
    {
        StartCoroutine(ConsumableSuperchargeCoroutine(time, sizeMultiplier));
    }

    public IEnumerator ConsumableSuperchargeCoroutine(float time, float sizeMultiplier)
    {
        // Supercharges the hole for a few seconds, currently called directly from consumable button
        float originalDiameter = transform.localScale.x;
        float targetDiameter = originalDiameter * sizeMultiplier;

        UpdateScale(targetDiameter);

        yield return new WaitForSeconds(time);

        UpdateScale(transform.localScale.x - (targetDiameter - originalDiameter));
    }

    private void SizePurchasedCallback()
    {
        IncreaseScale();
    }

    private void PowerPurchasedCallback()
    {
        powerMultiplier++;
    }

    private void UpgradesDataLoadedCallback(int timerLevel, int sizeLevel, int powerLevel)
    {
        // Update size
        float targetScale = transform.localScale.x + scaleStep * sizeLevel;
        UpdateScale(targetScale);

        // Update power
        powerMultiplier = powerLevel;
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
