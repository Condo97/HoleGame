using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    [Header(" Elements ")]
    [SerializeField] private GameObject[] levels;
    private float totalValuesToEat;

    [Header(" Settings ")]
    [SerializeField] private float completionPercentage;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Current Level Index: " + GetCurrentLevelIndex());
        // TODO: Do something more than just restarting if all the levels are beaten.. maybe make it harder somehow?
        // If current level index is outside of level count, reset to start and return       
        if (GetCurrentLevelIndex() >= levels.Length)
        {
            ResetToStart();
            return;
        }

        // Only show the current level
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject level = levels[i]; 
            if (i == GetCurrentLevelIndex() && !level.activeSelf)
            {
                // If i is the current level index and the level is not active, activate it
                level.SetActive(true);
            }
            else if (i != GetCurrentLevelIndex() && level.activeSelf)
            {
                // If i is not the current level index and the level is active, make in inactive
                level.SetActive(false);
            }
        }

        // Set total values to eat
        totalValuesToEat = GetTotalValuesToEatRecursive(GetCurrentLevel());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNext()
    {
        // Check if completion met and load next or restart level
        float totalEatenValues = ScoreManager.instance.totalCollectedValues;
        float percentageComplete = totalEatenValues / totalValuesToEat;

        if (percentageComplete >= completionPercentage)
            LoadNextLevel();
        else
            RestartLevel();
    }

    private void ResetToStart()
    {
        // Reset back to first level
        DataManager.instance.ResetLevel();

        // Reload scene, which should trigger the loading of the first level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadNextLevel()
    {
        // Add level to DataManager
        DataManager.instance.AddLevel();

        // Reset upgrades
        UpgradesManager.instance.ResetUpgradeLevels();

        // Reload scene, which should trigger loading of new level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public GameObject GetCurrentLevel()
    {
        return levels[GetCurrentLevelIndex()];
    }

    public int GetCurrentLevelIndex()
    {
        return DataManager.instance.GetLevel();
    }

    public float GetTotalValuesToEat()
    {
        return totalValuesToEat;
    }

    public float CountTotalToEat()
    {
        return levels[GetCurrentLevelIndex()].transform.childCount;
    }

    public float GetCompletionPercentage()
    {
        return completionPercentage;
    }

    private float GetTotalValuesToEatRecursive(GameObject gameObject)
    {
        float layerSum = 0;
        foreach (Transform transform in gameObject.transform)
        {
            if (transform.gameObject.GetComponent<Collectible>())
            {
                //TODO: Better implementation for this
                Collectible collectible = transform.gameObject.GetComponent<Collectible>();

                layerSum += collectible.GetValue();
            }
            else
            {
                layerSum += GetTotalValuesToEatRecursive(transform.gameObject);
            }
        }

        return layerSum;
    }

}
