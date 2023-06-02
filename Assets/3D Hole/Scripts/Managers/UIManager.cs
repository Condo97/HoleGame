using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    /***
     * Shows correct UI mobjects on game state change
     */

    [Header(" Main UI Elements ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject collectionPanel;
    [SerializeField] private GameObject collectionBossTransitionPanel;
    [SerializeField] private GameObject bossPanel;
    [SerializeField] private GameObject tryAgainPanel;
    [SerializeField] private GameObject levelCompletePanel;

    [Header(" Popups and Subviews ")]
    [SerializeField] private GameObject storePanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject upgradePanel;
    private GameObject cacheUIElement;

    private List<GameObject> uiElements // All UI panels need to be added to this list
    {
        get
        {
            return new List<GameObject>()
            {
                menuPanel,
                collectionPanel,
                collectionBossTransitionPanel,
                bossPanel,
                tryAgainPanel,
                levelCompletePanel,
                storePanel,
                shopPanel,
                upgradePanel
            };
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Subscribe GameStateChangedCallback to GameManager onStateChanged
        GameManager.onStateChanged += GameStateChangedCallback;

        // Call SetMenu to setup initial UI stuf
        SetMenu(true);
    }

    private void OnDestroy()
    {
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /***
     * Game State Changed Callback
     */

    private void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.MENU:
                SetMenu(false);
                break;
            case GameState.COLLECTION:
                SetCollection();
                break;
            case GameState.BOSS:
                SetBoss();
                break;
            case GameState.TRYAGAIN:
                SetLevelComplete();
                break;
            case GameState.LEVELCOMPLETE:
                SetLevelComplete();
                break;
        }
    }

    /***
     * Main UIs
     */

    private void SetMenu(bool animated)
    {
        if (animated)
            menuPanel.GetComponent<MoveInFromSidesAnimationController>().MoveObjectsOut();

        SetUIElementActive(menuPanel);

        if (animated)
            menuPanel.GetComponent<MoveInFromSidesAnimationController>().MoveObjectsIn(animated);
    }

    private void SetCollection()
    {
        SetUIElementActive(collectionPanel);
    }

    private void SetBoss()
    {
        SetUIElementActive(bossPanel);

        DoCollectionBossTransition();
    }

    //private void SetTryAgain()
    //{
    //    menuPanel.SetActive(false);
    //    collectionPanel.SetActive(false);
    //    bossPanel.SetActive(false);
    //    tryAgainPanel.SetActive(true);
    //    levelCompletePanel.SetActive(false);
    //}

    private void SetLevelComplete()
    {
        SetUIElementActive(levelCompletePanel);
    }

    /***
     * Side-UIs, which can be "closed" 
     */

    public void CloseSubUI()
    {
        CloseSubUI(null);
    }

    public void CloseSubUI(GameObject fallbackUIElement)
    {
        if (cacheUIElement != null)
        {
            SetUIElementActive(cacheUIElement);

            cacheUIElement = null;
        }
        else if (fallbackUIElement != null)
        {
            SetUIElementActive(fallbackUIElement);
        }
    }

    public void OpenStore()
    {
        cacheUIElement = GetCurrentActiveUIElement();

        SetUIElementActive(storePanel);
    }

    public void OpenShop()
    {
        cacheUIElement = GetCurrentActiveUIElement();

        SetUIElementActive(shopPanel);
    }

    public void OpenUpgrade()
    {
        cacheUIElement = GetCurrentActiveUIElement();

        SetUIElementActive(upgradePanel);
    }

    /***
     * Transitions
     */

    private void DoCollectionBossTransition()
    {
        var transitionAnimationController = collectionBossTransitionPanel.GetComponent<TransitionAnimationController>();
        collectionBossTransitionPanel.SetActive(true);
        StartCoroutine(transitionAnimationController.AnimateTransition(() => collectionBossTransitionPanel.SetActive(false)));
    }

    /***
     * Logic
     */

    private bool SetUIElementActive(GameObject uiElement)
    {
        bool uiElementDidSet = false;

        foreach (GameObject localUIElement in uiElements)
        {
            if (localUIElement == uiElement)
            {
                localUIElement.SetActive(true);

                uiElementDidSet = true;
            }
            else
            {
                localUIElement.SetActive(false);
            }
        }

        return uiElementDidSet;
    }

    private GameObject GetCurrentActiveUIElement()
    {
        foreach (GameObject localUIElement in uiElements)
        {
            if (localUIElement.activeSelf)
                return localUIElement;
        }

        return null;
    }

}
