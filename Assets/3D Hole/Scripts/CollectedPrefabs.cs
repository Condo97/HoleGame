using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedPrefabs
{

    public GameObject prefab;
    public int count;

    public CollectedPrefabs(GameObject prefab, int count)
    {
        this.prefab = prefab;
        this.count = count;
    }

    public void Increment()
    {
        count++;
    }

}
