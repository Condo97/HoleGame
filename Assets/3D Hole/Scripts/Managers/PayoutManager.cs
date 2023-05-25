using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayoutManager : MonoBehaviour
{

    public static PayoutManager instance;

    [Header(" Elements ")]
    [System.NonSerialized] public float? levelCompletePayout;
    [System.NonSerialized] public float multiplier = 1;
    [System.NonSerialized] private bool payoutCompleted = false;
    [SerializeField] private GameObject payoutAnimationController;
    //[System.NonSerialized] public float? 

    [Header(" Settings ")]
    [SerializeField] private float levelBasePayout;
    [SerializeField] private float levelEachPayout;

    [Header(" Events ")]
    public static Action<float> levelCompletePayoutCalculated;


    public void SetMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
    }

    public void DoPayout(bool animated, Action completion)
    {
        // Try to do payout if not completed using GetCalculatedPayout
        if (!payoutCompleted)
            DataManager.instance.AddCoins(GetCalculatedPayout());
        else
            Debug.Log("Prevented double payout");

        payoutCompleted = true;

        // If animated do animation and pass on completion otherwise just invoke completion
        if (animated)
            payoutAnimationController.GetComponent<PayoutAnimationController>().DoAnimation(completion);
        else
            completion.Invoke();

        //completion?.Invoke();
    }

    public float GetCalculatedPayout()
    {
        return (float)levelCompletePayout * multiplier;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        GameManager.onStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.LEVELCOMPLETE || gameState == GameState.TRYAGAIN)
        {
            // Get values
            float totalCollectedValues = ScoreManager.instance.totalCollectedValues;
            float totalValuesToEat = LevelManager.instance.GetTotalValuesToEat();
            float percentageEaten = totalCollectedValues / totalValuesToEat;
            int levelIndex = LevelManager.instance.GetCurrentLevelIndex();

            // Calculate levcel payout
            float levelPayout = levelBasePayout + levelEachPayout * (levelIndex + 1);

            // Update 
            if (levelCompletePayout == null)
            {
                // Set win bonus if level is complete and additional win bonus if completed with 100% eaten
                float winBonus = 1;
                if (gameState == GameState.LEVELCOMPLETE)
                {
                    if (percentageEaten < 1)
                    {
                        winBonus = 2f;
                    }
                    else
                    {
                        winBonus = 2.5f;
                    }
                }

                levelCompletePayout = levelPayout + levelPayout * percentageEaten * winBonus;

                // Round payout to the nearest 10
                levelCompletePayout = Mathf.Round((float)levelCompletePayout / 10.0f) * 10.0f;

                // Call action
                levelCompletePayoutCalculated?.Invoke((float)levelCompletePayout);
            }

            Debug.Log("Coins Added: " + levelCompletePayout);

            // Send out payoutComplete action
                // Show coins animation... which should be done in some controller I guess, on payoutComplete, or just maybe on GameState change
        }
    }

}
