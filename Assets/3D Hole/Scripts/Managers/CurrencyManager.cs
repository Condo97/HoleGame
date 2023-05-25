using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private TextMeshProUGUI coinsText;
    private float gemsTextAmount = -1; // Instantiate to -1 to ensure it updates when checking if it is the same as the PlayerPrefs amount
    private float coinsTextAmount = -1; // Instantiate to -1 to ensure it updates when checking if it is the same as the PlayerPrefs amount

    [Header(" Settings ")]
    [SerializeField] private float changingAnimationSpeed;


    private void Awake()
    {
        DataManager.onGemsUpdated += UpdateCurrencies;
        DataManager.onCoinsUpdated += UpdateCurrencies;
    }

    private void OnDestroy()
    {
        DataManager.onGemsUpdated -= UpdateCurrencies;
        DataManager.onCoinsUpdated -= UpdateCurrencies;
    }

    private void UpdateCurrencies()
    {
        // If the previous gems amount is different from the new amount, do the update animation
        if (gemsTextAmount != DataManager.instance.GetGems())
            LeanTween.value(gemsTextAmount, DataManager.instance.GetGems(), changingAnimationSpeed * Time.deltaTime * 60)
                .setOnUpdate((value) => {
                    gemsText.text = FormatCurrencyText(value);
                    gemsTextAmount = value;
                });

        if (coinsTextAmount != DataManager.instance.GetCoins())
            LeanTween.value(coinsTextAmount, DataManager.instance.GetCoins(), changingAnimationSpeed * Time.deltaTime * 60)
                .setOnUpdate((value) =>
                {
                    coinsText.text = FormatCurrencyText(value);
                    coinsTextAmount = value;
                });

        //gemsText.text = FormatCurrencyText(DataManager.instance.GetGems());
        //coinsText.text = FormatCurrencyText(DataManager.instance.GetCoins());
    }

    private string FormatCurrencyText(float amount)
    {
        if (amount < 0)
            return "0";
        if (amount < 999)               // 0-999, 999
            return amount.ToString("0");
        else if (amount < 9999)         // 1,000-9,999, 9.99k
            return (amount / 1000f).ToString("0.00") + "k";
        else if (amount < 99999)        // 10,000-99,999, 99.9k
            return (amount / 99999f).ToString("0.0") + "k";
        else if (amount < 999999)       // 100,000-999,999, 999k
            return (amount / 999999f).ToString("0") + "k";
        else if (amount < 9999999)      // 1,000,000-9,999,999, 9.99M
            return (amount / 9999999f).ToString("0.00") + "M";
        else if (amount < 99999999)     // 10,000,000-99,999,999, 99.9M
            return (amount / 99999999f).ToString("0.0") + "M";
        else if (amount < 999999999)    // 100,000,000-999,999,999, 999M
            return (amount / 999999999f).ToString("0") + "M";
        else if (amount < 9999999999)   // 1,000,000,000-9,999,999,999, 9.99B
            return (amount / 9999999999f).ToString("0.00") + "B";
        else if (amount < 99999999999)  // 10,000,000,000-99,999,999,999, 99.9B
            return (amount / 99999999999f).ToString("0.0") + "B";
        else if (amount < 999999999999) // 100,000,000,000-999,999,999,999, 999B
            return (amount / 999999999999f).ToString("0") + "B";

        return amount.ToString();
    }

}
