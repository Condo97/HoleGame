using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{

    public static UpgradesManager instance;

    [Header(" Elements ")]
    [SerializeField] private Button timerButton;
    [SerializeField] private Button sizeButton;
    [SerializeField] private Button powerButton;

    [Header(" Settings ")]
    [SerializeField] private int timerUpgradeAddition;
    [SerializeField] private float powerUpgradeAddition;

    [Header(" Data ")]
    private int timerLevel;
    private int sizeLevel;
    private int powerLevel;
    private const string timerKey = "TimerLevel";
    private const string sizeKey = "SizeLevel";
    private const string powerKey = "PowerLevel";

    [Header(" Pricing ")]
    [SerializeField] private int basePrice;
    [SerializeField] private int priceStep;

    [Header(" Events ")]
    public static Action onTimerPurchased;
    public static Action onSizePurchased;
    public static Action onPowerPurchased;
    public static Action<int, int, int> onDataLoaded;

    // Called before the Start method
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DataManager.onCoinsUpdated += CoinsUpdatedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeButtons();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        DataManager.onCoinsUpdated -= CoinsUpdatedCallback;
    }

    public void ResetUpgradeLevels()
    {
        timerLevel = 0;
        sizeLevel = 0;
        powerLevel = 0;

        SaveData();
    }

    public int GetTimerUpgradeAddition()
    {
        return timerUpgradeAddition;
    }

    public float GetPowerUpgradeAddition()
    {
        return powerUpgradeAddition;
    }

    public void TimerPurchasedCallback()
    {
        onTimerPurchased?.Invoke();

        DataManager.instance.PurchaseWithCoins(GetUpgradePrice(timerLevel));
        timerLevel++;
        SaveAndUpdateVisuals();
    }

    public void SizePurchasedCallback()
    {
        onSizePurchased?.Invoke();

        DataManager.instance.PurchaseWithCoins(GetUpgradePrice(sizeLevel));
        sizeLevel++;
        SaveAndUpdateVisuals();
    }

    public void PowerPurchasedCallback()
    {
        onPowerPurchased?.Invoke();

        DataManager.instance.PurchaseWithCoins(GetUpgradePrice(powerLevel));
        powerLevel++;
        SaveAndUpdateVisuals();
    }

    private void SaveAndUpdateVisuals()
    {
        SaveData();

        UpdateButtonsVisuals();
    }

    private void CoinsUpdatedCallback()
    {
        UpdateButtonsInteractability();
    }

    private void InitializeButtons()
    {
        // A script attached to each upgrade button
        // Call this script and configure it
        // Upgrade level
        // Interactability

        UpdateButtonsVisuals();

    }

    private void UpdateButtonsInteractability()
    {
        timerButton.interactable = GetUpgradePrice(timerLevel) <= DataManager.instance.GetCoins();
        sizeButton.interactable = GetUpgradePrice(sizeLevel) <= DataManager.instance.GetCoins();
        powerButton.interactable = GetUpgradePrice(powerLevel) <= DataManager.instance.GetCoins();
    }

    private void UpdateButtonsVisuals()
    {
        timerButton.GetComponent<UpgradeButtonController>().Configure(GetTimerString(timerLevel), GetUpgradePrice(timerLevel));
        sizeButton.GetComponent<UpgradeButtonController>().Configure(GetSizeString(sizeLevel), GetUpgradePrice(sizeLevel));
        powerButton.GetComponent<UpgradeButtonController>().Configure(GetPowerString(powerLevel), GetUpgradePrice(powerLevel));

        UpdateButtonsInteractability();
    }

    private int GetUpgradePrice(int upgradeLevel)
    {
        return basePrice + upgradeLevel * priceStep;
    }

    private string GetTimerString(int timerLevel)
    {
        return "+" + timerUpgradeAddition * (timerLevel + 1) + "s";
    }

    private string GetSizeString(int sizeLevel)
    {
        return sizeLevel + 2 + "x";
    }

    private string GetPowerString(int powerLevel)
    {
        return "+" + string.Format($"{powerUpgradeAddition * (powerLevel + 1) * 100:n0}") + "%";
    }

    private void LoadData()
    {
        timerLevel = PlayerPrefs.GetInt(timerKey);
        sizeLevel = PlayerPrefs.GetInt(sizeKey);
        powerLevel = PlayerPrefs.GetInt(powerKey);

        // Send an event with the different upgrade levels
        onDataLoaded?.Invoke(timerLevel, sizeLevel, powerLevel);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(timerKey, timerLevel);
        PlayerPrefs.SetInt(sizeKey, sizeLevel);
        PlayerPrefs.SetInt(powerKey, powerLevel);
    }

}
