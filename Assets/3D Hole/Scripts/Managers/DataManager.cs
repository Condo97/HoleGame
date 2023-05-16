using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Coins Data ")]
    private int coins;
    private const string coinsKey = "Coins";

    [Header(" Level Data ")]
    private int level;
    private const string levelKey = "Level";

    [Header(" Events ")]
    public static Action onCoinsUpdated;
    public static Action onLevelUpdated;


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
     * Coins
     */

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveData();
        onCoinsUpdated?.Invoke();
    }

    public void Purchase(int price)
    {
        coins -= price;
        SaveData();
        onCoinsUpdated?.Invoke();
    }

    public int GetCoins()
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
        onLevelUpdated?.Invoke();
    }

    public void GoToLevel(int level)
    {
        this.level = level;
        SaveData();
        onLevelUpdated?.Invoke();
    }

    public int GetLevel()
    {
        return level;
    }

    /***
     * Data Load and Save
     */

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt(coinsKey);
        level = PlayerPrefs.GetInt(levelKey);

        onCoinsUpdated?.Invoke();
        onLevelUpdated?.Invoke();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(coinsKey, coins);
        PlayerPrefs.SetInt(levelKey, level);
    }

}
