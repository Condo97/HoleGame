using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public sealed class ConsumableName
    {
        public static string Supercharge = "Supercharge";
        public static string Timer = "Timer";
        public static string Magnet = "Magnet";
    }

    [Header(" Gems Data ")]
    private float gems;
    private const string gemsKey = "Gems";

    [Header(" Coins Data ")]
    private float coins;
    private const string coinsKey = "Coins";

    [Header(" Level Data ")]
    private int level;
    private const string levelKey = "Level";

    //[Header(" Ads Removed Data ")]
    //private bool adsRemoved;
    //private const string adsRemovedKey = "AdsRemoved";

    [Header(" Consumable ")]
    private const string consumableBaseKey = "Consumable";

    [Header(" Events ")]
    public static Action onGemsUpdated;
    public static Action onCoinsUpdated;
    public static Action onAdsRemovedUpdated;
    //public static Action onLevelUpdated;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
    }

    // Start is called before the first frame update
    void Start()
    {

        //AddCoins(500);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /***
     * Gems
     */

    public void AddGems(float amount)
    {
        gems += amount;
        SaveData();
        onGemsUpdated?.Invoke();
    }

    public bool PurchaseWithGems(float amount)
    {
        if (gems - amount < 0)
            return false;

        gems -= amount;
        SaveData();
        onGemsUpdated?.Invoke();

        return true;
    }

    public float GetGems()
    {
        return gems;
    }

    /***
     * Coins
     */

    public void AddCoins(float amount)
    {
        coins += amount;
        SaveData();
        onCoinsUpdated?.Invoke();
    }

    public bool PurchaseWithCoins(float price)
    {
        if (coins - price < 0)
            return false;

        coins -= price;
        SaveData();
        onCoinsUpdated?.Invoke();

        return true;
    }

    public float GetCoins()
    {
        return coins;
    }

    /***
     * Level
     */

    public void AddLevel()
    {
        level++;
        SaveData();
        //onLevelUpdated?.Invoke();
    }

    public void GoToLevel(int level)
    {
        this.level = level;
        SaveData();
        //onLevelUpdated?.Invoke();
    }

    public void ResetLevel()
    {
        level = 0;
        SaveData();
        //onLevelUpdated?.Invoke();
    }

    public int GetLevel()
    {
        return level;
    }

    /***
     * Ads Removed
     */

    //public void SetAdsRemoved(bool adsRemoved)
    //{
    //    // Ensure didPurchase is different than stored value before updating, saving, and calling event
    //    if (this.adsRemoved != adsRemoved)
    //    {
    //        this.adsRemoved = adsRemoved;
    //        SaveData();

    //        onAdsRemovedUpdated?.Invoke();
    //    }
    //}

    //public bool GetAdsRemoved()
    //{
    //    return adsRemoved;
    //}

    /***
     * Consumables
     */

    public int GetConsumableRemaining(string name)
    {
        return PlayerPrefs.GetInt(consumableBaseKey + name);
    }

    public void AddToConsumableRemaining(string name, int amount)
    {
        SetConsumableRemaining(name, GetConsumableRemaining(name) + amount);
    }

    public void DecrementConsumableRemaining(string name)
    {
        AddToConsumableRemaining(name, -1);
    }

    public void SetConsumableRemaining(string name, int value)
    {
        PlayerPrefs.SetInt(consumableBaseKey + name, value);
    }

    /***
     * Data Load and Save
     */

    private void LoadData()
    {
        gems = PlayerPrefs.GetFloat(gemsKey);
        coins = PlayerPrefs.GetFloat(coinsKey);
        level = PlayerPrefs.GetInt(levelKey);
        //adsRemoved = PlayerPrefs.GetInt(adsRemovedKey) == 1 ? true : false;

        onCoinsUpdated?.Invoke();
        //onLevelUpdated?.Invoke();
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(gemsKey, gems);
        PlayerPrefs.SetFloat(coinsKey, coins);
        PlayerPrefs.SetInt(levelKey, level);
        //PlayerPrefs.SetInt(adsRemovedKey, adsRemoved ? 1 : 0);
    }

}
