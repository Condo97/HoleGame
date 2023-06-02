using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayoutManager : MonoBehaviour
{

    public static PayoutManager instance;

    [Header(" Elements ")]
    [SerializeField] private GameObject payoutAnimationController;
    [System.NonSerialized] public float? levelCompletePayout;
    [System.NonSerialized] public float? levelCompletionPercentagePayout;
    //[System.NonSerialized] public float winBonus = 1;
    [System.NonSerialized] public float additionalBonus = 1;
    [System.NonSerialized] private bool payoutCompleted = false;
    //[System.NonSerialized] public float? 

    [Header(" Settings ")]
    [SerializeField] private float levelBasePayout;
    [SerializeField] private float levelEachPayout;

    [Header(" Events ")]
    public static Action<float> levelCompletePayoutCalculated;


    public void SetAdditionalBonus(float additionalBonus)
    {
        this.additionalBonus = additionalBonus;
    }

    public void DoPayout(bool animated, Action completion)
    {
        // Try to do payout if not completed using GetCalculatedPayout
        if (!payoutCompleted)
            DataManager.instance.AddCoins((float)GetCalculatedPayout());
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

    public float? GetLevelCompletePayout()
    {
        return levelCompletePayout;
    }

    public float? GetLevelCompletionPercentagePayout()
    {
        return levelCompletionPercentagePayout;
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

    private float? GetCalculatedPayout()
    {
        return levelCompletePayout + levelCompletionPercentagePayout * additionalBonus;
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

            // Set win bonus multiplier if level is complete and additional win bonus if completed with 100% eaten
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

            // Calculate level complete payout, however much the player gets as a base amount per level
            levelCompletePayout = levelBasePayout + levelEachPayout * (levelIndex + 1) * winBonus;

            // Calculate level completion percentage payout, the additional amount the player gets depending on completion percentage
            levelCompletionPercentagePayout = Mathf.Round(((float)levelCompletePayout + (float)levelCompletePayout * percentageEaten)/10f) * 10f;

            //levelCompletePayout = levelPayout + levelPayout * percentageEaten * winBonus;

            //// Round payout to the nearest 10
            //levelCompletePayout = Mathf.Round((float)levelCompletePayout / 10.0f) * 10.0f;

            //// Call action
            //levelCompletePayoutCalculated?.Invoke((float)levelCompletePayout);

            Debug.Log("Coins Added: " + levelCompletePayout);

            // Send out payoutComplete action
                // Show coins animation... which should be done in some controller I guess, on payoutComplete, or just maybe on GameState change
        }
    }

}
