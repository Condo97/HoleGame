using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject magnet;


    public void ConsumableMagnet()
    {
        //StartCoroutine(MagnetTimeCoroutine());
        EnableMagnet();
    }

    private void Awake()
    {
        DisableMagnet();
    }

    private void Start()
    {
        //MagnetConsumable.magnetUsed += ConsumableMagnet;
        GameManager.onStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        //MagnetConsumable.magnetUsed -= ConsumableMagnet;
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    private void EnableMagnet()
    {
        magnet.SetActive(true);
    }

    private void DisableMagnet()
    {
        magnet.SetActive(false);
    }

    //IEnumerator MagnetTimeCoroutine()
    //{
    //    EnableMagnet();

    //    yield return new WaitForSeconds();

    //    DisableMagnet();
    //}

    private void GameStateChangedCallback(GameState gameState)
    {
        // Disable magnet when game state is changed TODO: Check if permanent magnet if one gets in the game
        DisableMagnet();
    }

}
