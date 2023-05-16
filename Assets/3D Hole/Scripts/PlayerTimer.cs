using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int timerDuration;
    private int timer;
    private bool timerIsOn;

    [Header(" Settings ")]
    [SerializeField] private int additionalTimePerLevel;

    [Header(" Events ")]
    public static Action onTimerOver;

    private void Awake()
    {
        // Subscribe UpgradesDataLoadedCallback to UpgradesManager onDataLoaded
        UpgradesManager.onDataLoaded += UpgradesDataLoadedCallback;

        // More subscriptions
        ConsumableManager.timerUsed += ConsumableTimer;
    }

    // Start is called before the first frame update
    void Start()
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
        ConsumableManager.timerUsed += ConsumableTimer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        timer = timerDuration;
        timerText.text = FormatSeconds(timer);
    }

    public void StartTimer()
    {
        if(timerIsOn)
        {
            Debug.LogWarning("A timer is already on.");
            return;
        }

        Initialize();

        timerIsOn = true;
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        while(timerIsOn)
        {
            yield return new WaitForSeconds(1);

            timer--;
            timerText.text = FormatSeconds(timer);

            if(timer == 0)
            {
                timerIsOn = false;
                StopTimer();
            }
        }
    }

    public void StopTimer()
    {
        onTimerOver?.Invoke();
    }

    public void ConsumableTimer(int time)
    {
        // Update the timer
        timer += time;
        timerText.text = FormatSeconds(timer);
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.COLLECTION:
                StartTimer();
                break;
        }
    }

    private void UpgradesDataLoadedCallback(int timerLevel, int sizeLevel, int powerLevel)
    {
        timerDuration += additionalTimePerLevel * timerLevel;
        Initialize();
    }

    private void TimerPurchaseCallback()
    {
        timerDuration += additionalTimePerLevel;
        Initialize();
    }

    private string FormatSeconds(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }
}
