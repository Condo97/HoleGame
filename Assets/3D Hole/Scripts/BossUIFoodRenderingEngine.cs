using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossUIFoodRenderingEngine : MonoBehaviour
{

    /******* Public Static Methods *******/
    public static List<RenderedCollectedPrefab> RenderPrefabs(List<CollectedPrefabs> prefabs)
    {
        List<RenderedCollectedPrefab> renderedCollectedPrefabs = new List<RenderedCollectedPrefab>();

        // Spawn foods from collected prefabs and notify generatedTexture
        foreach (CollectedPrefabs collectedPrefab in prefabs)
        {
            // Render collected prefab and append to renderedCollectedPrefabs
            renderedCollectedPrefabs.Add(RenderCollectedPrefab(collectedPrefab, renderedCollectedPrefabs.Count));
        }

        return renderedCollectedPrefabs;
    }


    /******* Private Methods *******/

    private static BossUIFoodRenderingEngine instance;

    [Header(" Elements ")]
    [SerializeField] private GameObject foodRendererPrefab;
    //private List<RenderedCollectedPrefab> renderedCollectedPrefabs = new List<RenderedCollectedPrefab>();

    // Start is called before the first frame update
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private static RenderedCollectedPrefab RenderCollectedPrefab(CollectedPrefabs collectedPrefab, int horizontalPositionIndex)
    {
        // Set horizontal offset for spawning the renderer prefabs and renderTexture size
        float horizontalOffset = 40;
        Vector3 renderTextureSize = new Vector3(256, 256, 1);

        // Spawn the food using the mesh from collectedPrefab
        GameObject g = Instantiate(BossUIFoodRenderingEngine.instance.foodRendererPrefab, new Vector3(-100 + horizontalOffset * horizontalPositionIndex, -100, -100), Quaternion.identity);
        g.GetComponentInChildren<MeshFilter>().mesh = collectedPrefab.prefab.GetComponent<MeshFilter>().mesh;

        // Get bounds and calculate scaleFactor and verticalOffset
        Vector3 targetSize = new Vector3(1, 1, 1);
        float scaleFactor = targetSize.magnitude / g.GetComponentInChildren<MeshRenderer>().bounds.size.magnitude;
        float verticalOffset = (g.GetComponentInChildren<MeshRenderer>().bounds.size.y / 2) * scaleFactor;

        // Multiply the localScale of the gameObject of the MeshFilter (the food, since it is the child gameObject that has the MeshFilter component) by the scale factor
        g.GetComponentInChildren<MeshFilter>().gameObject.transform.localScale *= scaleFactor;

        // Set position
        Vector3 p = g.GetComponentInChildren<MeshFilter>().gameObject.transform.position;
        p.y -= verticalOffset;
        g.GetComponentInChildren<MeshFilter>().gameObject.transform.position = p;

        // Get center location
        Vector3 centerLocationPreRotation = g.GetComponentInChildren<MeshRenderer>().bounds.center;

        // Set rotation
        Vector3 rotation = new Vector3(-45, 45, 0);
        //Quaternion q = new Quaternion(-45, 45, 0, 0);
        g.GetComponentInChildren<MeshFilter>().gameObject.transform.Rotate(rotation);

        // Get new center location, calculate difference with pre rotation center location, and move to new center
        Vector3 centerLocationPostRotation = g.GetComponentInChildren<MeshRenderer>().bounds.center;
        Vector3 centerLocationDifference = centerLocationPreRotation - centerLocationPostRotation;
        g.GetComponentInChildren<MeshFilter>().gameObject.transform.position += centerLocationDifference;

        // Create renderTexture and set it as the outputTexture of camera
        RenderTexture renderTexture = new RenderTexture((int)renderTextureSize.x, (int)renderTextureSize.y, (int)renderTextureSize.z);
        g.GetComponentInChildren<Camera>().targetTexture = renderTexture;

        return new RenderedCollectedPrefab(g, renderTexture, collectedPrefab);
    }

}
