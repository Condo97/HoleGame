using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public static TimerManager instance;

    [Header(" Elements ")]
    [SerializeField] private GameObject timer;
    [SerializeField] private int baseTimerDuration;
    private int currentTimerDuration;
    private bool timerIsOn;

    [Header(" Events ")]
    public static Action onTimerOver;

    /***
     * Timer Manager Methods
     */

    public void Initialize()
    {
        currentTimerDuration = baseTimerDuration;
        timer.GetComponent<PlayerTimer>().SetTimerText(FormatSeconds(currentTimerDuration));
    }

    public void StartTimer()
    {
        if (timerIsOn)
        {
            Debug.LogWarning("A timer is already on.");
            return;
        }

        Initialize();

        timerIsOn = true;
        StartCoroutine(TimerCoroutine());
    }

    public void StopTimer()
    {
        onTimerOver?.Invoke();
    }

    public void ConsumableTimer(int time)
    {
        // Update the timer
        currentTimerDuration += time;
        timer.GetComponent<PlayerTimer>().SetTimerText(FormatSeconds(currentTimerDuration));
    }

    /***
     * Private Methods
     */

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Subscribe UpgradesDataLoadedCallback to UpgradesManager onDataLoaded
        UpgradesManager.onDataLoaded += UpgradesDataLoadedCallback;

        // More subscriptions
        ConsumableManager.timerUsed += ConsumableTimer;
    }

    private void Start()
    {
        // Initialize timer
        Initialize();

        // Subscribe GameStateChangedCallback to GameManager onStateChanged
        GameManager.onStateChanged += GameStateChangedCallback;

        // Subscribe TimerPurchaseCallback to UpgradesManager onTimerPurchased
        UpgradesManager.onTimerPurchased += TimerPurchaseCallback;

    }

    private void OnDestroy()
    {
        // Unsubscribe GameStateChangedCallback from GameManager onStateChanged
        GameManager.onStateChanged -= GameStateChangedCallback;

        // Unsubscribe TimerPurchaseCallback from UpgradesManager onTimerPurchased
        UpgradesManager.onTimerPurchased -= TimerPurchaseCallback;

        // Unsubscribe UpgradesDataLoadedCallback to UpgradesManager onDataLoaded
        UpgradesManager.onDataLoaded -= UpgradesDataLoadedCallback;

        // More unsubscriptions
        ConsumableManager.timerUsed -= ConsumableTimer;
    }

    private IEnumerator TimerCoroutine()
    {
        while (timerIsOn)
        {
            yield return new WaitForSeconds(1);

            currentTimerDuration--;
            timer.GetComponent<PlayerTimer>().SetTimerText(FormatSeconds(currentTimerDuration));

            if (currentTimerDuration == 0)
            {
                timerIsOn = false;
                StopTimer();
            }
        }
    }

    private string FormatSeconds(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    /***
     * Callbacks
     */

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.COLLECTION:
                StartTimer();
                break;
        }
    }

    private void UpgradesDataLoadedCallback(int timerLevel, int sizeLevel, int powerLevel)
    {
        baseTimerDuration += UpgradesManager.instance.GetTimerUpgradeAddition() * timerLevel;
        Initialize();
    }

    private void TimerPurchaseCallback()
    {
        baseTimerDuration += UpgradesManager.instance.GetTimerUpgradeAddition();
        Initialize();
    }

}
