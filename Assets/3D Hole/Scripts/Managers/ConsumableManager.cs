using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableManager: MonoBehaviour
{

    // Manages the amount of each consumable?


    //[Header(" Buttons ")]
    //[SerializeField] private Button superchargeButton;
    //[SerializeField] private Button timerButton;
    //[SerializeField] private Button magnetButton;

    //[Header(" Supercharge ")]
    //[SerializeField] private float superchargeTime;
    //[SerializeField] private float superchargeMultiplier;
    //private bool superchargeEnabled;

    //[Header(" Timer ")]
    //[SerializeField] private int timerTime;
    //private bool timerEnabled;

    //[Header(" Magnet ")]
    //[SerializeField] private float magnetTime;
    //private bool magnetEnabled;

    [Header(" Elements ")]
    [SerializeField] private List<ConsumableController> consumables;

    //[Header(" Events ")]
    //public static Action<float> superchargeUsed;
    //public static Action<int> timerUsed;
    //public static Action<float> magnetUsed;



    public void UpdateConsumable(ConsumableController consumableController)
    {
        int remaining = 0;
        Consumable c = consumableController.consumable as Consumable;
        if (c != null)
            remaining = DataManager.instance.GetConsumableRemaining(c.GetConsumableName());

        // If there are some remaining, set display ad to false in AdClickyImage and update the remaining as button's text
        if (remaining > 0)
        {
            consumableController.button.GetComponent<AdClickyImages>().SetShowAdDisplay(false);
            consumableController.button.GetComponent<ConsumableController>().SetRemainingText(remaining);
        }
        else
        {
            consumableController.button.GetComponent<AdClickyImages>().SetShowAdDisplay(true);
        }
    }

    private void Start()
    {
        // Update all consumables
        foreach (ConsumableController consumable in consumables)
            UpdateConsumable(consumable);
    }

    //public void UpdateSupercharge()
    //{
    //    UpdateConsumable(superchargeButton, DataManager.ConsumableName.Supercharge);
    //}

    //public void UpdateTimer()
    //{
    //    UpdateConsumable(timerButton, DataManager.ConsumableName.Timer);
    //}

    //public void UpdateMagnet()
    //{
    //    UpdateConsumable(magnetButton, DataManager.ConsumableName.Magnet);
    //}


    //public void Supercharge()
    //{
    //    // Check if there are any remaining, otherwise purchase with ad
    //    //if (DataManager.instance.GetAdsRemoved())
    //    //    SuperchargeAction();
    //    //else
    //    //{
    //    //    Time.timeScale = 0;
    //    //    RewardedAdManager.instance.ShowAd((success) =>
    //    //    {
    //    //        Time.timeScale = 1;
    //    //        SuperchargeAction();
    //    //    });
    //    //}

    //}

    //public void Timer()
    //{
    //    //if (DataManager.instance.GetAdsRemoved())
    //    //    TimerAction();
    //}

    //private void SuperchargeAction()
    //{
    //    if (superchargeEnabled)
    //    {
    //        superchargeUsed?.Invoke(superchargeMultiplier);

    //        superchargeEnabled = false;
    //    }

    //    UpdateButtonsInteractability();
    //}

    //public void TimerAction()
    //{
    //    if (timerEnabled)
    //    {
    //        timerUsed?.Invoke(timerTime);

    //        timerEnabled = false;
    //    }

    //    UpdateButtonsInteractability();
    //}

    //public void Magnet()
    //{
    //    if (magnetEnabled)
    //    {
    //        magnetUsed?.Invoke(magnetTime);

    //        magnetEnabled = false;
    //    }

    //    UpdateButtonsInteractability();
    //}

    //private void UpdateButtonsInteractability()
    //{
    //    if (superchargeEnabled)
    //        superchargeButton.GetComponent<Clicky>().Enable();
    //    else
    //        superchargeButton.GetComponent<Clicky>().Disable();

    //    if (timerEnabled)
    //        timerButton.GetComponent<Clicky>().Enable();
    //    else
    //        timerButton.GetComponent<Clicky>().Disable();

    //    if (magnetEnabled)
    //        magnetButton.GetComponent<Clicky>().Enable();
    //    else
    //        magnetButton.GetComponent<Clicky>().Disable();
    //}

}
