using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionHoleController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject holeTop;
    [SerializeField] private GameObject holeBottom;
    [SerializeField] private float standardHoleMovementAnimationSpeed = 0.2f;
    [SerializeField] private float holeOffset;
    [SerializeField] private float holeBottomInset;
    [SerializeField] private float holeTopInset;
    private float initialBarBottomPosition;
    private float initialBarTopPosition;
    private bool levelCompleteAnimationPlayed = false;

    [Header(" Win Animation ")]
    [SerializeField] private float wiggleMaxRotationDegrees = 70f;
    [SerializeField] private int wiggleFrameSpeed = 2;
    [SerializeField] private float wiggleLeftRightAmount = 40f;
    [SerializeField] private int wiggleLeftRightFrameSpeed = 2;
    [SerializeField] private float wiggleDownHoleMovementAnimationSpeed = 4.0f;
    private bool wiggleDown = false;
    private bool isWigglingForward = true;
    private float wiggleRotatedAmount;
    private float? wiggleLeftRightMin;
    private float? wiggleLeftRightMax;
    private bool isWigglingLeft = true;


    void Start()
    {
        CollectedManager.collected += CollectedCallback;
        GameManager.onStateChanged += GameStateChangedCallback;

        // Initial hole localPosition update
        initialBarBottomPosition = gameObject.transform.localPosition.y - gameObject.GetComponent<RectTransform>().rect.size.y / 2;
        initialBarTopPosition = gameObject.transform.localPosition.y + gameObject.GetComponent<RectTransform>().rect.size.y / 2;

        UpdateHolePosition(false);
    }

    private void OnDestroy()
    {
        CollectedManager.collected -= CollectedCallback;
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    private void Update()
    {
        if (wiggleDown)
        {
            WiggleFrame();
            WiggleLeftRightFrame();
        }
    }

    private void UpdateHolePosition(bool animated)
    {
        float totalEaten = CollectedManager.instance.GetTotalCollectedSize();
        float totalToEat = LevelManager.instance.GetTotalValuesToEat();

        float targetMoveAmount = totalEaten / totalToEat;

        MoveHole(targetMoveAmount, animated);
    }

    private void MoveHole(float percentage, bool animated)
    {
        MoveHole(percentage, animated, standardHoleMovementAnimationSpeed, true, null);
    }

    private void MoveHole(float percentage, bool animated, float animationSpeed, bool ensureUp, Action tweenComplete)
    {
        Debug.Log("Percentage: " + percentage);
        // If percentage is greater than 1, set it to 1 to animate to max
        if (percentage > 1)
            percentage = 1;

        // If the percentage is greater than or equal to CompletionPercentage, stop the hole from animating and make it big and show "Complete" animation or some stars or something
        if (percentage >= 0.8)
        {
            DoLevelCompleteAnimations();
            return;
        }

        // Get target value and animation time
        float targetValue = (initialBarTopPosition - initialBarBottomPosition) * percentage + initialBarBottomPosition;
        float animationTime = animated ? animationSpeed * Time.deltaTime * 60 : 0;

        // Since the targetValue is calculated without the holeBottomOffset, ensure said value is reached before moving the hole up
        if (targetValue < initialBarBottomPosition + holeBottomInset)
        {
            targetValue = initialBarBottomPosition + holeBottomInset;
        }

        // Add hole offset to target value
        targetValue += holeOffset;

        // Only animate if targetValue and holeBottom localPosition y are not the same
        if (targetValue != holeBottom.transform.localPosition.y)
        {
            LeanTween.value(holeBottom.transform.localPosition.y, targetValue, animationTime)
                .setOnUpdate((value) =>
                {
                    if (value > holeBottom.transform.localPosition.y || !ensureUp)
                    {
                        Vector3 targetVector = new Vector3(holeBottom.transform.localPosition.x, value, holeBottom.transform.localPosition.z);

                        holeBottom.transform.localPosition = targetVector;
                        holeTop.transform.localPosition = targetVector;
                    }
                })
                .setOnComplete(() => tweenComplete?.Invoke());
        }
    }

    private void DoLevelCompleteAnimations()
    {
        if (!levelCompleteAnimationPlayed)
        {
            // Make hole bigger
            float targetScale = 1.2f;
            float animationTime = 0.2f * Time.deltaTime * 60;
            LeanTween.value(holeBottom.transform.localScale.y, targetScale, animationTime)
                .setOnUpdate((value) =>
                {
                    holeBottom.transform.localScale = value * Vector3.one;
                    holeTop.transform.localScale = value * Vector3.one;
                });

            // Wiggle down
            WiggleDown();

            // Set levelCompleteAnimationPlayed to true
            levelCompleteAnimationPlayed = true;
        }
    }

    private void WiggleFrame()
    {
        // Do the rotation
        if (isWigglingForward)
        {
            Vector3 targetRotation = new Vector3(0, 0, -1);
            holeBottom.transform.Rotate(targetRotation);
            holeTop.transform.Rotate(targetRotation);

            wiggleRotatedAmount += wiggleFrameSpeed;
        }
        else
        {
            Vector3 targetRotation = new Vector3(0, 0, 1);
            holeBottom.transform.Rotate(targetRotation);
            holeTop.transform.Rotate(targetRotation);

            wiggleRotatedAmount -= wiggleFrameSpeed;
        }

        // Check if should switch direction
        if (wiggleRotatedAmount <= 0)
            isWigglingForward = true;
        else if (wiggleRotatedAmount >= wiggleMaxRotationDegrees)
            isWigglingForward = false;
    }

    private void WiggleLeftRightFrame()
    {
        // If wiggleLeftRightMin or wiggleLeftRightMax are null, set them
        if (wiggleLeftRightMin == null || wiggleLeftRightMax == null)
        {
            wiggleLeftRightMin = holeBottom.transform.localPosition.x - wiggleLeftRightAmount / 2;
            wiggleLeftRightMax = holeBottom.transform.localPosition.x + wiggleLeftRightAmount / 2;
        }

        // Do wiggle
        if (isWigglingLeft)
        {
            Vector3 targetPosition = new Vector3(holeBottom.transform.localPosition.x - wiggleLeftRightFrameSpeed, holeBottom.transform.localPosition.y, holeBottom.transform.localPosition.z);

            holeBottom.transform.localPosition = targetPosition;
            holeTop.transform.localPosition = targetPosition;
        }
        else
        {
            Vector3 targetPosition = new Vector3(holeBottom.transform.localPosition.x + wiggleLeftRightFrameSpeed, holeBottom.transform.localPosition.y, holeBottom.transform.localPosition.z);

            holeBottom.transform.localPosition = targetPosition;
            holeTop.transform.localPosition = targetPosition;
        }

        // Check if should switch direction
        if (holeBottom.transform.localPosition.x <= wiggleLeftRightMin)
            isWigglingLeft = false;
        else if (holeBottom.transform.localPosition.x >= wiggleLeftRightMax)
            isWigglingLeft = true;
        
    }

    private void WiggleDown()
    {
        // Set wiggleDown to true to start wiggle animations in update
        wiggleDown = true;

        // Move the hole down to 0% with a slower animation
        MoveHole(0, true, wiggleDownHoleMovementAnimationSpeed, false, () =>
        {
            wiggleDown = false;
        });


    }

    private void CollectedCallback(Collectible collectible)
    {
        UpdateHolePosition(true);
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        //if (gameState == GameState.COLLECTION)
        //{
        //    // Initial hole localPosition update
        //    initialBarBottomPosition = gameObject.transform.localPosition.y - gameObject.GetComponent<RectTransform>().rect.size.y / 2;
        //    initialBarTopPosition = gameObject.transform.localPosition.y + gameObject.GetComponent<RectTransform>().rect.size.y / 2;

        //    UpdateHolePosition(false);
        //}
    }

}
