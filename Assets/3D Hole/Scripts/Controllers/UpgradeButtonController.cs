using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private AdClicky button;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI priceText;


    public void Configure(string desc, int price)
    {
        descText.text = desc;
        priceText.text = price.ToString(); // Can format with k and M and so on
    }

    public void SetButtonShowAdDisplay(bool showAdDisplay)
    {
        button.SetShowAdDisplay(showAdDisplay);
    }

}
