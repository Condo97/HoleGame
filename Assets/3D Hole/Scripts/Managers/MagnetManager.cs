using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject magnet;


    private void Awake()
    {
        DisableMagnet();
    }

    private void Start()
    {
        ConsumableManager.magnetUsed += ConsumableMagnet;
        GameManager.onStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        ConsumableManager.magnetUsed -= ConsumableMagnet;
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

    private void ConsumableMagnet(float time)
    {
        StartCoroutine(MagnetTimeCoroutine(time));
    }

    IEnumerator MagnetTimeCoroutine(float time)
    {
        EnableMagnet();

        yield return new WaitForSeconds(time);

        DisableMagnet();
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        // Disable magnet when game state is changed TODO: Check if permanent magnet if one gets in the game
        DisableMagnet();
    }

}
