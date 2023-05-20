using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherManager : MonoBehaviour
{

    private class ButtonRenderedCollectedPrefab
    {
        public RenderedCollectedPrefab renderedCollectedPrefab;
        public Button button;

        public ButtonRenderedCollectedPrefab(RenderedCollectedPrefab renderedCollectedPrefab, Button button)
        {
            this.renderedCollectedPrefab = renderedCollectedPrefab;
            this.button = button;
        }
    }

    [Header(" Elemenets ")]
    [SerializeField] private GameObject launcher;
    [SerializeField] private GameObject bossUIFoodGridLayoutController;
    private List<ButtonRenderedCollectedPrefab> launchingPrefabs = new List<ButtonRenderedCollectedPrefab>();
    private int launchingFrames = 0;

    [Header(" Settings ")]
    [SerializeField] private int launchDelayFrames;
    [SerializeField] private int secondsToWaitBeforeInvokingDepletedFood = 1;

    [Header(" Events ")]
    public static Action<GameObject> finishedLaunch;
    public static Action depletedFood;


    // Start is called before the first frame update
    void Start()
    {
        // Setup bossUIFoodGridLayoutController
        bossUIFoodGridLayoutController.GetComponent<BossUIFoodGridLayoutController>().didPress = DidPressFoodButtonCallback;
        bossUIFoodGridLayoutController.GetComponent<BossUIFoodGridLayoutController>().didRelease = DidReleaseFoodButtonCallback;

        GameManager.onStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (launchingPrefabs.Count > 0)
        {
            if (launchingFrames % launchDelayFrames == 0)
            {
                // Only launch one prefab at a time, so choose one within the range of 0 to launchingPrefabs.Count to launch
                int randomIndex = UnityEngine.Random.Range(0, launchingPrefabs.Count - 1);
                ButtonRenderedCollectedPrefab launchingPrefab = launchingPrefabs[randomIndex];

                // Launch if there is something to launch
                if (launchingPrefab.renderedCollectedPrefab.count > 0)
                {
                    // Launch the prefab and decrement the count!
                    Launch(launchingPrefab.renderedCollectedPrefab.prefab);
                    launchingPrefab.renderedCollectedPrefab.count--;

                    // Update the remaining text for the button
                    launchingPrefab.button.GetComponent<BossUIFoodButton>().SetText(launchingPrefab.renderedCollectedPrefab.count.ToString());
                }

                // Immediately check for 0 to remove the prefab
                if (launchingPrefab.renderedCollectedPrefab.count <= 0)
                {
                    // Remove launchingPrefab, which is the one at the randomIndex
                    launchingPrefabs.RemoveAt(randomIndex);

                    // Update the button state here, could be outside of the if conditional but it's only really going to be needed to disable the buttons when 0 in this specific function
                    UpdateButtonState(launchingPrefab);

                    // Check if the "bag" of collected objects is empty and call the depletedFood action if so
                    CheckIfAllEmpty();
                }
            }

            launchingFrames++;
        }
        else
        {
            launchingFrames = 0;
        }
    }

    private void Launch(GameObject prefab)
    {
        // Don't launch the prefab! Pick a matching object from CollectedManager
        var collectedObject = CollectedManager.instance.Pick(prefab);

        if (collectedObject != null)
        {
            StartCoroutine(launcher.GetComponent<LauncherController>().AnimateLaunch(collectedObject, () =>
            {
                finishedLaunch?.Invoke(prefab);
            }));
        }
    }

    private void UpdateButtonState(ButtonRenderedCollectedPrefab buttonRenderedCollectionPrefab)
    {
        if (buttonRenderedCollectionPrefab.renderedCollectedPrefab.count <= 0)
        {
            // Disable button
            buttonRenderedCollectionPrefab.button.interactable = false;

            // TODO: Better implementation for this
            buttonRenderedCollectionPrefab.button.GetComponent<Clicky>().Disable();
        }
        else
        {
            // Enable button
            buttonRenderedCollectionPrefab.button.interactable = true;

            // TODO: Better implementation
            buttonRenderedCollectionPrefab.button.GetComponent<Clicky>().Enable();
        }
    }

    private void CheckIfAllEmpty()
    {
        // Check if CollectedManager is empty and if so invoke depletedFood action
        if (CollectedManager.instance.IsEmpty())
            StartCoroutine(InvokeDepletedFoodAfterSeconds(secondsToWaitBeforeInvokingDepletedFood));
    }

    private IEnumerator InvokeDepletedFoodAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        depletedFood?.Invoke();
    }

    private void DidPressFoodButtonCallback(Button button, RenderedCollectedPrefab prefab)
    {
        // If the prefab is not in the launchingPrefabs list, add it
        foreach (ButtonRenderedCollectedPrefab brcp in launchingPrefabs)
        {
            if (brcp.renderedCollectedPrefab == prefab)
                return;
        }

        launchingPrefabs.Add(new ButtonRenderedCollectedPrefab(prefab, button));
    }

    private void DidReleaseFoodButtonCallback(Button button, RenderedCollectedPrefab prefab)
    {
        // Remove the prefab from launchingPrefabs
        foreach (ButtonRenderedCollectedPrefab brcp in launchingPrefabs)
        {
            if (brcp.renderedCollectedPrefab == prefab)
            {
                launchingPrefabs.Remove(brcp);
                return;
            }
        }
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.BOSS)
        {
            // Build/set BossUIFoodGridLayoutController buttons
            bossUIFoodGridLayoutController.GetComponent<BossUIFoodGridLayoutController>().BuildButtons(BossUIFoodRenderingEngine.RenderPrefabs(CollectedManager.instance.GetCollectedPrefabs()));
        }
    }

}
