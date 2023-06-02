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
    [SerializeField] private List<int> priceList;
    [SerializeField] private int priceAdditionAfterListComplete;

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
        LoadData();
        InitializeButtons();
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

    public void SizePurchasedCallback()
    {
        PurchaseWithCoinsOrAd(GetUpgradePrice(sizeLevel), (success) =>
        {
            onSizePurchased?.Invoke();

            sizeLevel++;
            SaveAndUpdateVisuals();
        });
    }

    public void TimerPurchasedCallback()
    {
        PurchaseWithCoinsOrAd(GetUpgradePrice(timerLevel), (success) =>
        {
            onTimerPurchased?.Invoke();

            timerLevel++;
            SaveAndUpdateVisuals();
        });
    }

    public void PowerPurchasedCallback()
    {
        PurchaseWithCoinsOrAd(GetUpgradePrice(powerLevel), (success) =>
        {
            onPowerPurchased?.Invoke();

            powerLevel++;
            SaveAndUpdateVisuals();
        });
    }

    private void SaveAndUpdateVisuals()
    {
        SaveData();

        UpdateButtonsVisuals();
    }

    private void CoinsUpdatedCallback()
    {
        UpdateButtonNormalOrAd();
    }

    private void InitializeButtons()
    {
        // A script attached to each upgrade button
        // Call this script and configure it
        // Upgrade level
        // Interactability

        UpdateButtonsVisuals();

    }

    //private void UpdateButtonsInteractability()
    //{
    //    timerButton.interactable = GetUpgradePrice(timerLevel) <= DataManager.instance.GetCoins();
    //    sizeButton.interactable = GetUpgradePrice(sizeLevel) <= DataManager.instance.GetCoins();
    //    powerButton.interactable = GetUpgradePrice(powerLevel) <= DataManager.instance.GetCoins();
    //}

    private void PurchaseWithCoinsOrAd(int price, Action<bool> success)
    {
        if (CanPurchaseWithCoins(price))
        {
            DataManager.instance.PurchaseWithCoins(price);

            success?.Invoke(true);
        }
        else
        {
            RewardedAdManager.instance.ShowAd((showAdSuccess) =>
            {
                success?.Invoke(true);
            });
        }
    }

    private void UpdateButtonNormalOrAd()
    {
        bool sizeShouldShowAd = !CanPurchaseUpgradeLevel(sizeLevel);
        bool timerShouldShowAd = !CanPurchaseUpgradeLevel(timerLevel);
        bool powerShouldShowAd = !CanPurchaseUpgradeLevel(powerLevel);

        sizeButton.GetComponent<UpgradeButtonController>().SetButtonShowAdDisplay(sizeShouldShowAd);
        timerButton.GetComponent<UpgradeButtonController>().SetButtonShowAdDisplay(timerShouldShowAd);
        powerButton.GetComponent<UpgradeButtonController>().SetButtonShowAdDisplay(powerShouldShowAd);
    }

    private void UpdateButtonsVisuals()
    {
        sizeButton.GetComponent<UpgradeButtonController>().Configure(GetSizeString(sizeLevel), GetUpgradePrice(sizeLevel));
        timerButton.GetComponent<UpgradeButtonController>().Configure(GetTimerString(timerLevel), GetUpgradePrice(timerLevel));
        powerButton.GetComponent<UpgradeButtonController>().Configure(GetPowerString(powerLevel), GetUpgradePrice(powerLevel));

        UpdateButtonNormalOrAd();
    }

    private int GetUpgradePrice(int upgradeLevel)
    {
        if (priceList.Count > upgradeLevel)
        {
            // Price is within priceList, so just return the price using the upgradeLevel as the index
            return priceList[upgradeLevel];
        }

        // Price is outside of priceList, so add the priceAdditionAfterListComplete multiplied by upgrade level minus the priceList count to the last price in priceList
        return priceList[priceList.Count - 1] + priceAdditionAfterListComplete * (upgradeLevel - (priceList.Count - 1));
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

    private bool CanPurchaseUpgradeLevel(int upgradeLevel)
    {
        return CanPurchaseWithCoins(GetUpgradePrice(upgradeLevel));
    }

    private bool CanPurchaseWithCoins(int coins)
    {
        return DataManager.instance.GetCoins() >= coins;
    }

    private void LoadData()
    {
        sizeLevel = PlayerPrefs.GetInt(sizeKey);
        timerLevel = PlayerPrefs.GetInt(timerKey);
        powerLevel = PlayerPrefs.GetInt(powerKey);

        // Send an event with the different upgrade levels
        onDataLoaded?.Invoke(timerLevel, sizeLevel, powerLevel);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(sizeKey, sizeLevel);
        PlayerPrefs.SetInt(timerKey, timerLevel);
        PlayerPrefs.SetInt(powerKey, powerLevel);
    }

}
