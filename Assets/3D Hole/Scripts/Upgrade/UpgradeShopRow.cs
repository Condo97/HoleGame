using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeShopRow : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] protected TextMeshProUGUI gemCostText;
    [SerializeField] protected TextMeshProUGUI ownedText;

    [Header(" Settings ")]
    [SerializeField] protected int gemCost;


    public virtual void DidPurchase()
    {

    }

    public void Start()
    {
        gemCostText.text = "-" + gemCost.ToString();
    }

}
