using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanelController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI coinsAmountText;
    [SerializeField] private TextMeshProUGUI fedPercentageText;
    [SerializeField] private GameObject fedBackground;
    [SerializeField] private GameObject spinner;
    [SerializeField] private Button noThanksButton;
    private float coinsAmount;
    private bool shouldGoToNextLevel = false;

    [Header(" Settings ")]
    [SerializeField] private float noThanksButtonShowDelaySeconds;
    [SerializeField] private float spinShowAdDelaySeconds;
    [SerializeField] private float coinsUpdateAnimationDuration;


    /***
     * Button Callbacks
     */

    public void NoThanksButtonCallback()
    {
        // Disable spinner button and do payout
        DisableButtons();

        DoPayout();
    }

    public void SpinnerButtonCallback()
    {
        // Disable buttons and stop spinning
        DisableButtons();
        spinner.GetComponent<Spinner>().StopSpinning();

        // Start coroutine to show ad after a little delay
        StartCoroutine(ShowAdAfterDelay());
    }

    /***
     * Inherited Methods
     */

    private void Awake()
    {
        // Subscribe to events
        PayoutManager.levelCompletePayoutCalculated += UpdateCoinsAmountAnimated;
        GameManager.onStateChanged += GameStateChangedCallback;
    }

    private void Start()
    {
        // Set noThanksButton to clear
        noThanksButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
    }

    private void OnEnable()
    {
        // Update no thanks button opacity after delay
        UpdateNoThanksButtonOpacityAfterDelay();
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        PayoutManager.levelCompletePayoutCalculated -= UpdateCoinsAmountAnimated;
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    /***
     * Coins Amount Animation and Text Handling
     */

    private void UpdateCoinsAmountAnimated(float payout)
    {
        UpdateCoinsAmount(payout, true);
    }

    private void UpdateCoinsAmount(float payout, bool animated)
    {
        float animationDuration = animated ? coinsUpdateAnimationDuration : 0;
        LeanTween.value(coinsAmount, payout, animationDuration)
            .setOnUpdate((value) => UpdateCoinsAmountText(value));
    }

    private void UpdateCoinsAmountText(float coinsAmount)
    {
        this.coinsAmount = coinsAmount;
        coinsAmountText.text = string.Format($"{coinsAmount:n0}");
    }

    /***
     * Fed Percentage Handling
     */

    private float GetFedPercentage()
    {
        float totalToEat = LevelManager.instance.GetTotalValuesToEat();
        float totalEaten = ScoreManager.instance.totalCollectedValues;

        return totalEaten / totalToEat;
    }

    private void UpdateFedPercentage()
    {
        fedPercentageText.text = string.Format($"{GetFedPercentage() * 100:n0}%");
    }

    private void UpdateFedBackground()
    {
        Color? color = GetFedBackgroundColor(GetFedPercentage());
        if (color != null)
            fedBackground.GetComponent<Image>().color = (Color)color;
    }

    private Color? GetFedBackgroundColor(float percentage)
    {
        if (percentage <= 0.4)
            return Color.red;
        if (percentage <= 0.6)
            return Color.yellow;
        if (percentage <= 0.8)
            return Color.green;
        if (percentage <= 1)
            return Color.magenta;

        return null;
    }

    /***
     * No Thanks Button Handling
     */

    private IEnumerator UpdateNoThanksButtonOpacityAfterDelay(float delaySeconds, float fadeInAnimationTime)
    {
        yield return new WaitForSeconds(delaySeconds);

        LeanTween.value(noThanksButton.gameObject, Color.clear, Color.white, fadeInAnimationTime)
            .setOnUpdate((Color value) => noThanksButton.GetComponentInChildren<TextMeshProUGUI>().color = value);
    }

    private void UpdateNoThanksButtonOpacityAfterDelay()
    {
        StartCoroutine(UpdateNoThanksButtonOpacityAfterDelay(noThanksButtonShowDelaySeconds, 0.2f));
    }

    /***
     * Payout Handling
     */

    private void DoPayout()
    {
        PayoutManager.instance.DoPayout(true, () =>
        {
            // Load next level or retry level
            LevelManager.instance.LoadNext();
        });
    }

    private IEnumerator ShowAdAfterDelay()
    {
        // Get bonus
        float bonus = spinner.GetComponent<Spinner>().GetBonus();

        yield return new WaitForSeconds(spinShowAdDelaySeconds);

        // Update text
        RewardedAdManager.instance.ShowAd((success) =>
        {
            //TODO: Maybe don't apply the reward if not success?
            Debug.Log("Successfully showed ad after delay for reward? " + success);

            // Apply bonus on ad watch
            PayoutManager.instance.SetMultiplier(bonus);

            // Animate cions amount change
            UpdateCoinsAmountAnimated(PayoutManager.instance.GetCalculatedPayout());

            // Do payout which should triger animation automatically
            DoPayout();

            // Do animation and then load next level TODO
            //LevelManager.instance.LoadNextLevel();
        });
    }

    /***
     * Button State Handling
     */

    private void DisableButtons()
    {
        // Spinner button
        spinner.GetComponentInChildren<Button>().interactable = false;

        // No thanks button
        noThanksButton.interactable = false;
    }

    private void EnableButtons()
    {
        // Spinner button
        spinner.GetComponentInChildren<Button>().interactable = true;

        // No thanks button
        noThanksButton.interactable = true;
    }

    /***
     * Event Callbacks
     */

    private void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.LEVELCOMPLETE || gameState == GameState.TRYAGAIN)
        {
            // Set if should go to next level
            shouldGoToNextLevel = gameState == GameState.LEVELCOMPLETE;

            // Update no thanks button color
            //UpdateNoThanksButtonOpacity();

            // Update fed percentage
            UpdateFedPercentage();

            // Update fed background
            UpdateFedBackground();
        }
    }

}
