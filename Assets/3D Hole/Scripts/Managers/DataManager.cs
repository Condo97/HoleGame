using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Gems Data ")]
    private float gems;
    private const string gemsKey = "Gems";

    [Header(" Coins Data ")]
    private float coins;
    private const string coinsKey = "Coins";

    [Header(" Level Data ")]
    private int level;
    private const string levelKey = "Level";

    [Header(" Premium Data ")]
    private bool isPremium;
    private const string premiumKey = "Premium";

    [Header(" Events ")]
    public static Action onGemsUpdated;
    public static Action onCoinsUpdated;
    //public static Action onLevelUpdated;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadData();

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

    public void PurchaseWithGems(float amount)
    {
        gems -= amount;
        SaveData();
        onGemsUpdated?.Invoke();
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

    public void PurchaseWithCoins(float price)
    {
        coins -= price;
        SaveData();
        onCoinsUpdated?.Invoke();
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
     * Premium
     */

    public bool GetIsPremium()
    {
        return isPremium;
    }

    /***
     * Data Load and Save
     */

    private void LoadData()
    {
        gems = PlayerPrefs.GetFloat(gemsKey);
        coins = PlayerPrefs.GetFloat(coinsKey);
        level = PlayerPrefs.GetInt(levelKey);
        isPremium = PlayerPrefs.GetInt(premiumKey) == 1 ? true : false;

        onCoinsUpdated?.Invoke();
        //onLevelUpdated?.Invoke();
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(gemsKey, gems);
        PlayerPrefs.SetFloat(coinsKey, coins);
        PlayerPrefs.SetInt(levelKey, level);
        PlayerPrefs.SetInt(premiumKey, isPremium ? 1 : 0);
    }

}
