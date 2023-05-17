using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [Header(" All Phases ")]
    public float globalMultiplier = 1;

    [Header(" Collection Phase ")]
    public int totalCollectedCount = 0;
    public float collectionScore = 0;
    public float collectionMultiplier = 1;

    [Header(" Boss Phase ")]
    public int totalBossImpactCount = 0;
    public float bossScore = 0;
    public float bossMultiplier = 1;
    public float bossDefeatedBonusPerLevel = 500;
    public bool bossDefeated = false;

    public float calculatedBossDefeatedBonus
    {
        get
        {
            return LevelManager.instance.GetCurrentLevelIndex() * bossDefeatedBonusPerLevel;
        }
    }


    public float GetScore()
    {
        return (collectionScore * collectionMultiplier + bossScore * bossMultiplier + (bossDefeated ? calculatedBossDefeatedBonus : 0)) * globalMultiplier;
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
        collectionScore += collectible.GetValue();
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
