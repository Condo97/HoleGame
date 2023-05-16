using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MENU, COLLECTION, BOSS, TRYAGAIN, LEVELCOMPLETE, GAMEOVER }

public class GameManager : MonoBehaviour
{

    [Header(" Settings ")]
    private GameState gameState;

    [Header(" Events ")]
    public static Action<GameState> onStateChanged;
    public static Action<int> level;


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update, using IEnumerator so that yield return null can be used and it can wait until all the other Start methods have been finished TODO: Could the yield return null and IEnumerator be used in SetMenuState instead? Should it?
    IEnumerator Start()
    {
        yield return null;

        PlayerTimer.onTimerOver += SetBossState;
        LauncherManager.depletedFood += SetTryAgainState;
        BossManager.bossHPDepleted += SetLevelCompleteState;

        SetMenuState();
    }

    private void OnDestroy()
    {
        PlayerTimer.onTimerOver -= SetBossState;
        LauncherManager.depletedFood -= SetTryAgainState;
        BossManager.bossHPDepleted -= SetLevelCompleteState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMenuState()
    {
        if (gameState != GameState.MENU)
        {
            gameState = GameState.MENU;

            onStateChanged?.Invoke(gameState);
        }
    }

    public void SetCollectionState()
    {
        if (gameState != GameState.COLLECTION)
        {
            gameState = GameState.COLLECTION;

            onStateChanged?.Invoke(gameState);
        }
    }

    public void SetBossState()
    {
        if (gameState != GameState.BOSS)
        {
            gameState = GameState.BOSS;

            onStateChanged?.Invoke(gameState);
        }
    }

    public void SetTryAgainState()
    {
        if (gameState != GameState.TRYAGAIN)
        {
            gameState = GameState.TRYAGAIN;

            onStateChanged?.Invoke(gameState);
        }
    }

    public void SetLevelCompleteState()
    {
        if (gameState != GameState.LEVELCOMPLETE)
        {
            gameState = GameState.LEVELCOMPLETE;

            onStateChanged?.Invoke(gameState);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void NextLevel()
    //{

    //}

}
