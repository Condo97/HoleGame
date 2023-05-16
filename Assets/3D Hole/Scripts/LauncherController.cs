using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LauncherController : MonoBehaviour
{

    [Header(" Settings ")]
    [SerializeField] private float horizontalRandomVariation;
    [SerializeField] private float verticalRandomVariation;
    [SerializeField] private AnimationCurve trailCurve;
    [SerializeField] private float trailTime;
    [SerializeField] private float tarilMinVertexDistance;
    [SerializeField] private Gradient trailGradient;
    [SerializeField] private Material trailMaterial;
    [SerializeField] private float destroyTimeSeconds;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AnimateLaunch(GameObject prefab, Action completion)
    {
        // Create and launch gameObject from prefab
        GameObject toLaunch = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        toLaunch.SetActive(true);
        toLaunch.GetComponent<Rigidbody>().mass = 1;
        toLaunch.GetComponent<Rigidbody>().AddForce(new Vector3(0 + (Random.Range(0, horizontalRandomVariation) - horizontalRandomVariation / 2), 7000 + (Random.Range(0, verticalRandomVariation) - verticalRandomVariation / 2), 1700));
        Debug.Log(toLaunch.GetComponent<Rigidbody>());

        // Setup trailRenderer
        TrailRenderer trailRenderer = toLaunch.AddComponent<TrailRenderer>();
        trailRenderer.widthCurve = trailCurve;
        trailRenderer.time = trailTime;
        trailRenderer.minVertexDistance = tarilMinVertexDistance;
        trailRenderer.colorGradient = trailGradient;
        trailRenderer.material = trailMaterial;

        // Wait for 4 seconds then destroy toLaunch and call completion block
        yield return new WaitForSeconds(destroyTimeSeconds);

        Destroy(toLaunch);

        completion.Invoke();
    }

}
