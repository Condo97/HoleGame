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
        // Only show the current level
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject level = levels[i]; 
            if (i == GetCurrentLevelIndex() && !level.activeSelf)
            {
                // If i is the current level index and the level is not active, activate it
                level.SetActive(true);
            } else if (i != GetCurrentLevelIndex() && level.activeSelf)
            {
                // If i is not the current level index and the level is active, make in inactive
                level.SetActive(false);
            }
        }

        // Get total values to eat
        totalValuesToEat = GetTotalValuesToEatRecursive(GetCurrentLevel());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNextLevel()
    {
        // Add level to DataManager
        DataManager.instance.AddLevel();

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

    private float GetTotalValuesToEatRecursive(GameObject gameObject)
    {
        float layerSum = 0;
        foreach (Transform transform in gameObject.transform)
        {
            Debug.Log(transform.gameObject.transform.childCount );
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

        Debug.Log(layerSum);
        return layerSum;
    }

}
