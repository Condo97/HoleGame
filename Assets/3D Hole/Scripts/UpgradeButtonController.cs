using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{

    [Header(" Elements ")]
    //[SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI priceText;


    public void Configure(int level, int price)
    {
        //levelText.text = "Lvl " + (level + 1).ToString();
        priceText.text = price.ToString(); // Can format with k and M and so on
    }

}
