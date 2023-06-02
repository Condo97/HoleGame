using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableUpgradeShopRow : UpgradeShopRow
{

    [Header(" Elements ")]
    [SerializeField] public MonoBehaviour consumable;
    [SerializeField] private Clicky purchaseButton;

    [Header(" Settings ")]
    [SerializeField] private int consumableToAddAmount;

    public new void Start()
    {
        base.Start();

        DataManager.onCoinsUpdated += UpdatePurchaseButtonIsActive;
        DataManager.onGemsUpdated += UpdatePurchaseButtonIsActive;

        UpdateOwnedText();
        UpdatePurchaseButtonIsActive();
    }

    public override void DidPurchase()
    {
        base.DidPurchase();

        // Subtract Gems and add consumable
        Consumable c = consumable as Consumable;
        if (c != null)
            if (DataManager.instance.PurchaseWithGems(gemCost))
                DataManager.instance.AddToConsumableRemaining(c.GetConsumableName(), consumableToAddAmount);

        UpdateOwnedText();
        UpdatePurchaseButtonIsActive();
    }

    private void OnEnable()
    {
        UpdatePurchaseButtonIsActive();
    }

    private void UpdateOwnedText()
    {
        Consumable c = consumable as Consumable;
        if (c != null)
            ownedText.text = DataManager.instance.GetConsumableRemaining(c.GetConsumableName()).ToString();
    }

    private void UpdatePurchaseButtonIsActive()
    {
        // Enable the button if the amount of gems owned is more than the gem cost
        if (DataManager.instance?.GetGems() >= gemCost)
            purchaseButton.Enable();
        //purchaseButton.GetComponent<Clicky>().Enable();
        else
            purchaseButton.Disable();
        //purchaseButton.GetComponent<Clicky>().Disable();

    }

}
