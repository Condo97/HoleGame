using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherManager : MonoBehaviour
{

    [Header(" Elemenets ")]
    [SerializeField] private GameObject launcher;
    private List<GameObject> launchingPrefabs = new List<GameObject>();
    private int launchingFrames = 0;

    [Header(" Settings ")]
    [SerializeField] private int launchDelayFrames;

    [Header(" Events ")]
    public static Action<GameObject> finishedLaunch;


    // Start is called before the first frame update
    void Start()
    {
        BossUIFoodGridLayoutController.didPress += DidPressFoodButtonCallback;
        BossUIFoodGridLayoutController.didRelease += DidReleaseFoodButtonCallback;
    }

    private void OnDestroy()
    {
        BossUIFoodGridLayoutController.didPress -= DidPressFoodButtonCallback;
        BossUIFoodGridLayoutController.didRelease -= DidReleaseFoodButtonCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (launchingPrefabs.Count > 0)
        {
            if (launchingFrames % launchDelayFrames == 0)
            {
                foreach (GameObject launchingPrefab in launchingPrefabs)
                {
                    Launch(launchingPrefab);
                }
            }

            launchingFrames++;
        }
        else
        {
            launchingFrames = 0;
        }
    }

    private void Launch(GameObject prefab)
    {
        StartCoroutine(launcher.GetComponent<LauncherController>().AnimateLaunch(prefab, () =>
        {
            finishedLaunch?.Invoke(prefab);
        }));
    }

    private void DidPressFoodButtonCallback(Button button, GameObject prefab)
    {
        if (!launchingPrefabs.Contains(prefab))
        {
            launchingPrefabs.Add(prefab);
        }
    }

    private void DidReleaseFoodButtonCallback(Button button, GameObject prefab)
    {
        launchingPrefabs.Remove(prefab);
    }

}
