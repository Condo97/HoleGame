using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject boss;

    [Header(" Events ")]
    public static Action bossHPDepleted;

    private void Awake()
    {
        LauncherManager.finishedLaunch += FinishedLaunchCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        boss.GetComponent<BossController>().hpDepleted = HPDepletedCallback;
    }

    private void OnDestroy()
    {
        LauncherManager.finishedLaunch -= FinishedLaunchCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FinishedLaunchCallback(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Collectible collectible))
        {
            CollectibleImpacted(collectible);
        }
    }

    private void CollectibleImpacted(Collectible collectible)
    {
        boss.GetComponent<BossController>().Damage(collectible.GetValue());
    }

    private void HPDepletedCallback()
    {
        // Award coins here?
        DataManager.instance.AddCoins(500);

        bossHPDepleted?.Invoke();
    }

}
