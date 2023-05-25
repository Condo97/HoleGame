using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUIFoodGridLayoutController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Button foodButtonPrefab;

    [Header(" Events ")]
    public Action<Button, RenderedCollectedPrefab> didPress;
    public Action<Button, RenderedCollectedPrefab> didRelease;


    public void BuildButton(RenderedCollectedPrefab renderedCollectedPrefab)
    {
        BuildButtons(new List<RenderedCollectedPrefab>() { renderedCollectedPrefab });
    }

    public void BuildButtons(List<RenderedCollectedPrefab> renderedCollectedPrefabs)
    {
        // Add components in Food Buttons in grid
        foreach (RenderedCollectedPrefab renderedCollectedPrefab in renderedCollectedPrefabs)
        {
            // Instantiate foodButton and set parent transform to gameObject's and set RawImage texture to renderedCollectedPrefab renderTexture
            Button foodButton = Instantiate(foodButtonPrefab);
            foodButton.transform.SetParent(gameObject.transform, false);
            foodButton.GetComponentInChildren<BossUIFoodButton>().SetTexture(renderedCollectedPrefab.renderTexture);
            foodButton.GetComponentInChildren<BossUIFoodButton>().SetText(renderedCollectedPrefab.count.ToString());
            foodButton.GetComponent<BossUIFoodButton>().touchDown = () => didPress.Invoke(foodButton, renderedCollectedPrefab);
            foodButton.GetComponent<BossUIFoodButton>().touchUp = () => didRelease.Invoke(foodButton, renderedCollectedPrefab);
        }
    }

}
