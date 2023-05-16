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

    [Header(" Timer ")]
    [SerializeField] private int timerTime;

    [Header(" Magnet ")]
    [SerializeField] private float magnetTime;

    [Header(" Events ")]
    public static Action<float, float> superchargeUsed;
    public static Action<int> timerUsed;
    public static Action<float> magnetUsed;

    private void Awake()
    {
        
    }

    public void Supercharge()
    {
        superchargeUsed?.Invoke(superchargeTime, superchargeMultiplier);
    }

    public void Timer()
    {
        timerUsed?.Invoke(timerTime);
    }

    public void Magnet()
    {
        magnetUsed?.Invoke(magnetTime);
    }

    private void UpdateButtonsInteractability()
    {

    }

}
