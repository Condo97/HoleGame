using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PayoutAnimationController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject coinPayoutAnimationPrefab;

    [Header(" Spawn Settings ")]
    [SerializeField] private int minSpawnAmount;
    [SerializeField] private int maxSpawnAmount;

    [Header(" Movement Settings ")]
    [SerializeField] private float spawnToMoveTime;
    [SerializeField] private float moveTime;
    [SerializeField] private float moveToDissapearTime;


    public void DoAnimation(Action completion)
    {
        bool didInvokeCompletion = false;

        // Spawn between minAmount and maxAmount of coins using the current bounds as the bounds to spawn
        float amountToSpawn = Random.Range(minSpawnAmount, maxSpawnAmount);
        for (int i = 0; i < amountToSpawn; i++)
        {
            // Generate random spawn position in bounds
            float minXPos = gameObject.transform.position.x - gameObject.GetComponent<RectTransform>().rect.size.x / 2;
            float maxXPos = gameObject.transform.position.x + gameObject.GetComponent<RectTransform>().rect.size.x / 2;
            float minYPos = gameObject.transform.position.y - gameObject.GetComponent<RectTransform>().rect.size.y / 2;
            float maxYPos = gameObject.transform.position.y + gameObject.GetComponent<RectTransform>().rect.size.y / 2;

            float generatedXPos = Random.Range(minXPos, maxXPos);
            float generatedYPos = Random.Range(minYPos, maxYPos);

            Vector3 generatedPos = new Vector3(generatedXPos, generatedYPos, 0);

            // Create moveToPos as top left corner
            Vector3 moveToPos = new Vector3(0, Screen.height, 0);

            // Instantiate object
            GameObject coinPrefab = Instantiate(coinPayoutAnimationPrefab, generatedPos, Quaternion.identity, transform);

            // Move each object to the top left corner and destroy
            LeanTween.delayedCall(spawnToMoveTime, () =>
            {
                LeanTween.move(coinPrefab, moveToPos, moveTime)
                    .setOnComplete(() =>
                    {
                        LeanTween.delayedCall(moveToDissapearTime, () =>
                        {
                            Destroy(coinPrefab);

                            if (!didInvokeCompletion)
                                completion?.Invoke();

                            didInvokeCompletion = true;
                        });
                    });
            });
        }

    }

}
