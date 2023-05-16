using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollectedManager : MonoBehaviour
{

    public static CollectedManager instance;

    [Header(" Elements ")]
    private float totalCollectedSize;
    private List<CollectedPrefabs> collectedPrefabs
        = new List<CollectedPrefabs>();

    [Header(" Events ")]
    public static Action<Collectible> collected;


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
        DestroyTrigger.collectibleCollected += CollectibleCollectedCallback;
    }

    private void OnDestroy()
    {
        DestroyTrigger.collectibleCollected -= CollectibleCollectedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetTotalCollectedSize()
    {
        return totalCollectedSize;
    }

    public void SetCollectedPrefabs(List<CollectedPrefabs> collectedPrefabs)
    {
        this.collectedPrefabs = collectedPrefabs;
    }

    public List<CollectedPrefabs> GetCollectedPrefabs()
    {
        return collectedPrefabs;
    }

    private void CollectibleCollectedCallback(Collectible collectible)
    {
        // Add to totalSize
        totalCollectedSize += collectible.GetValue();

        // Increment collectedPrefab for gameobject
        IncrementCollectedPrefab(collectible.gameObject);

        // Call collected
        collected?.Invoke(collectible);
    }

    private void IncrementCollectedPrefab(GameObject gameObject)
    {
        // Get game object prefab index, and increment the collectedPrefab at that index
        int gameObjectPrefabIndex = GetPrefabIndex(gameObject);
        collectedPrefabs[gameObjectPrefabIndex].Increment();
    }

    //private int GetGameObjectPrefabIndex(GameObject gameObject)
    //{
    //    return GetPrefabIndex(PrefabUtility.GetCorrespondingObjectFromSource(gameObject));
    //}

    private int GetPrefabIndex(GameObject gameObject)
    {
        Debug.Log("Prefab: " + gameObject);

        // Loop through collectedPrefabs
        for (int i = 0; i < collectedPrefabs.Count; i++)
        {
            // Check if materials are equal TODO: Is this good enough?
            if (collectedPrefabs[i].prefab.TryGetComponent(out MeshFilter collectedMeshFilter) && gameObject.TryGetComponent(out MeshFilter goMeshFilter)) {
                if (collectedMeshFilter.sharedMesh == goMeshFilter.sharedMesh)
                    return i;
            }

        }

        // If prefab not found, instantiate and append new collectedPrefab with that prefab to collectedPrefabs and return the index of the last object, which is this one
        GameObject prefab = Instantiate(gameObject);
        prefab.SetActive(false);

        CollectedPrefabs collectedPrefab = new CollectedPrefabs(prefab, 0);
        collectedPrefabs.Add(collectedPrefab);

        return collectedPrefabs.Count - 1;
    }

}
