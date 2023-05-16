using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIFoodGridLayoutController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Button foodButtonPrefab;

    [Header(" Events ")]
    public static Action<Button, GameObject> didPress;
    public static Action<Button, GameObject> didRelease;


    // Start is called before the first frame update
    void Start()
    {
        BossUIFoodRenderingEngine.didRenderCollectedPrefabs += DidRenderCollectedPrefabsCallback;
    }

    private void OnDestroy()
    {
        BossUIFoodRenderingEngine.didRenderCollectedPrefabs -= DidRenderCollectedPrefabsCallback;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DidRenderCollectedPrefabsCallback(List<RenderedCollectedPrefab> renderedCollectedPrefabs)
    {
        // Add components in Food Buttons in grid
        foreach (RenderedCollectedPrefab renderedCollectedPrefab in renderedCollectedPrefabs)
        {
            // Instantiate foodButton and set parent transform to gameObject's and set RawImage texture to renderedCollectedPrefab renderTexture
            Button foodButton = Instantiate(foodButtonPrefab);
            foodButton.transform.SetParent(gameObject.transform, false);
            foodButton.GetComponentInChildren<RawImage>().texture = renderedCollectedPrefab.renderTexture;
            foodButton.GetComponent<BossUIFoodButton>().touchDown = () => didPress.Invoke(foodButton, renderedCollectedPrefab.prefab);
            foodButton.GetComponent<BossUIFoodButton>().touchUp = () => didRelease.Invoke(foodButton, renderedCollectedPrefab.prefab);
        }

    }

}
