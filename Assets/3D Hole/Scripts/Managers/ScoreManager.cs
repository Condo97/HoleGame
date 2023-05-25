using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    [Header(" All Phases ")]
    [System.NonSerialized] public float globalMultiplier = 1;

    [Header(" Collection Phase ")]
    [System.NonSerialized] public int totalCollectedCount = 0;
    [System.NonSerialized] public float totalCollectedValues = 0;

    [Header(" Boss Phase ")]
    [System.NonSerialized] public int totalBossImpactCount = 0;
    [System.NonSerialized] public float bossScore = 0;
    [System.NonSerialized] public bool bossDefeated = false;

    [Header(" Settings ")]
    [SerializeField] public float bossDefeatedBonusPerLevel = 500;


    public float calculatedBossDefeatedBonus
    {
        get
        {
            return LevelManager.instance.GetCurrentLevelIndex() * bossDefeatedBonusPerLevel;
        }
    }


    //public float GetScore()
    //{
    //    return (collectionScore + bossScore + (bossDefeated ? calculatedBossDefeatedBonus : 0)) * globalMultiplier;
    //}

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CollectedManager.collected += CollectedCollectibleCallback;
        LauncherManager.finishedLaunch += BossImpactCallback;
        BossManager.bossHPDepleted += BossHPDepletedCallback;
    }

    private void OnDestroy()
    {
        CollectedManager.collected -= CollectedCollectibleCallback;
        LauncherManager.finishedLaunch -= BossImpactCallback;
        BossManager.bossHPDepleted -= BossHPDepletedCallback;
    }

    private void CollectedCollectibleCallback(Collectible collectible)
    {
        totalCollectedCount++;
        totalCollectedValues += collectible.GetValue();
    }

    private void BossImpactCallback(GameObject gameObject)
    {
        // The different name is for fun :)

        totalBossImpactCount++;
        if (gameObject.TryGetComponent<Collectible>(out Collectible c))
            bossScore += c.GetValue();
    }

    private void BossHPDepletedCallback()
    {
        bossDefeated = true;
    }

}
