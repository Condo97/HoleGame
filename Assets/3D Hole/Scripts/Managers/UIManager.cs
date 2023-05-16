using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    /***
     * Shows correct UI mobjects on game state change
     */

    [Header(" Elements ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject collectionPanel;
    [SerializeField] private GameObject bossPanel;
    [SerializeField] private GameObject tryAgainPanel;
    [SerializeField] private GameObject levelCompletePanel;

    [Header(" Coins ")]
    [SerializeField] private TextMeshProUGUI menuCoinsText;


    private void Awake()
    {
        DataManager.onCoinsUpdated += UpdateCoins;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe GameStateChangedCallback to GameManager onStateChanged
        GameManager.onStateChanged += GameStateChangedCallback;

        // Call SetMenu to setup initial UI stuf
        SetMenu();
    }

    private void OnDestroy()
    {
        GameManager.onStateChanged -= GameStateChangedCallback;
        DataManager.onCoinsUpdated -= UpdateCoins;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.MENU:
                SetMenu();
                break;
            case GameState.COLLECTION:
                SetCollection();
                break;
            case GameState.BOSS:
                SetBoss();
                break;
            case GameState.TRYAGAIN:
                SetTryAgain();
                break;
            case GameState.LEVELCOMPLETE:
                SetLevelComplete();
                break;
        }
    }

    private void SetMenu()
    {
        menuPanel.SetActive(true);
        collectionPanel.SetActive(false);
        bossPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
    }

    private void SetCollection()
    {
        menuPanel.SetActive(false);
        collectionPanel.SetActive(true);
        bossPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
        levelCompletePanel.SetActive(false);

    }

    private void SetBoss()
    {
        menuPanel.SetActive(false);
        collectionPanel.SetActive(false);
        bossPanel.SetActive(true);
        tryAgainPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
    }

    private void SetTryAgain()
    {
        menuPanel.SetActive(false);
        collectionPanel.SetActive(false);
        bossPanel.SetActive(false);
        tryAgainPanel.SetActive(true);
        levelCompletePanel.SetActive(false);
    }

    private void SetLevelComplete()
    {
        menuPanel.SetActive(false);
        collectionPanel.SetActive(false);
        bossPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
        levelCompletePanel.SetActive(true);
    }

    private void UpdateCoins()
    {
        menuCoinsText.text = DataManager.instance.GetCoins().ToString();
    }

}
