using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableManager: MonoBehaviour
{

    [Header(" Buttons ")]
    [SerializeField] private Button superchargeButton;
    [SerializeField] private Button timerButton;
    [SerializeField] private Button magnetButton;

    [Header(" Supercharge ")]
    [SerializeField] private float superchargeTime;
    [SerializeField] private float superchargeMultiplier;
    private bool superchargeEnabled;

    [Header(" Timer ")]
    [SerializeField] private int timerTime;
    private bool timerEnabled;

    [Header(" Magnet ")]
    [SerializeField] private float magnetTime;
    private bool magnetEnabled;

    [Header(" Events ")]
    public static Action<float, float> superchargeUsed;
    public static Action<int> timerUsed;
    public static Action<float> magnetUsed;

    private void Start()
    {
        // TODO: Check if consumables should be enabled
        superchargeEnabled = true;
        timerEnabled = true;
        magnetEnabled = true;
    }

    public void Supercharge()
    {
        if (superchargeEnabled)
        {
            superchargeUsed?.Invoke(superchargeTime, superchargeMultiplier);

            superchargeEnabled = false;
        }

        UpdateButtonsInteractability();
    }

    public void Timer()
    {
        if (timerEnabled)
        {
            timerUsed?.Invoke(timerTime);

            timerEnabled = false;
        }

        UpdateButtonsInteractability();
    }

    public void Magnet()
    {
        if (magnetEnabled)
        {
            magnetUsed?.Invoke(magnetTime);

            magnetEnabled = false;
        }

        UpdateButtonsInteractability();
    }

    private void UpdateButtonsInteractability()
    {
        if (superchargeEnabled)
            superchargeButton.GetComponent<Clicky>().Enable();
        else
            superchargeButton.GetComponent<Clicky>().Disable();

        if (timerEnabled)
            timerButton.GetComponent<Clicky>().Enable();
        else
            timerButton.GetComponent<Clicky>().Disable();

        if (magnetEnabled)
            magnetButton.GetComponent<Clicky>().Enable();
        else
            magnetButton.GetComponent<Clicky>().Disable();
    }

}
