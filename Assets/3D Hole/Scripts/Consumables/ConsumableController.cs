using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] public MonoBehaviour consumable;
    [SerializeField] public AdClicky button;
    [SerializeField] public TextMeshProUGUI remainingText;

    [Header(" Settings ")]
    [SerializeField] private string remainingTextPrefix;
    protected bool consumableUsed;


    /***
     * Public Methods
     */

    public void UseConsumable()
    {
        if (!consumableUsed)
        {
            Consumable c = consumable as Consumable;
            if (c != null)
            {
                int count = DataManager.instance.GetConsumableRemaining(c.GetConsumableName());
                if (count > 0)
                {
                    c.ConsumableAction();
                    DataManager.instance.DecrementConsumableRemaining(c.GetConsumableName());

                    consumableUsed = true;
                }
                else
                {
                    RewardedAdManager.instance.ShowAd((success) =>
                    {
                        c.ConsumableAction();
                        consumableUsed = true;
                    });
                }
            }
        }
    }

    public void SetRemainingText(int remaining)
    {
        remainingText.text = remainingTextPrefix + remaining.ToString();
    }


    /***
     * Private Methods
     */

    private void Start()
    {
        Consumable c = consumable as Consumable;
        if (c != null)
        {
            c.consumableUsed += UpdateButtonInteractability;

            SetRemainingText(DataManager.instance.GetConsumableRemaining(c.GetConsumableName()));
        }

        consumableUsed = false;

        UpdateButtonNormalOrAd();
    }

    private void UpdateButtonNormalOrAd()
    {
        Consumable c = consumable as Consumable;
        if (c != null)
        {
            int count = DataManager.instance.GetConsumableRemaining(c.GetConsumableName());
            if (count > 0)
            {
                // Normal state
                button.SetShowAdDisplay(false);
            }
            else
            {
                // Ad state
                button.SetShowAdDisplay(true);
            }
        }
    }

    private void UpdateButtonInteractability()
    {
        if (consumableUsed)
            button.Enable();
        else
            button.Disable();
    }

}
