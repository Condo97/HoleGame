using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderedCollectedPrefab : CollectedPrefabs
{

    public GameObject spawnedGameObject;
    public RenderTexture renderTexture;

    public RenderedCollectedPrefab(GameObject spawnedGameObject, RenderTexture renderTexture, CollectedPrefabs collectedPrefabs) : this(spawnedGameObject, renderTexture, collectedPrefabs.prefab, collectedPrefabs.count) { }

    public RenderedCollectedPrefab(GameObject spawnedGameObject, RenderTexture renderTexture, GameObject prefab, int count) : base(prefab, count)
    {
        this.spawnedGameObject = spawnedGameObject;
        this.renderTexture = renderTexture;
    }

}
