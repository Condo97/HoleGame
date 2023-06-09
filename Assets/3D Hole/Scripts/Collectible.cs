using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    [Header(" Settings ")]
    [SerializeField] private float value;
    private bool softDisable;
    private float initialXSize, initialZSize;

    private void Awake()
    {
        // Set initialX and initialZ as renderer x and z at spawn.. renderer is used instead of collider because the objects may have mesh colliders and the bounds seems to be not totally accurate whereas renderer bounds uses a rect around the object
        if (gameObject.TryGetComponent(out Renderer renderer))
        {
            initialXSize = renderer.bounds.size.x;
            initialZSize = renderer.bounds.size.z;
        }

        // Subscribe to actions
        LayerSwitchSize.onDiameterIncrease += LayerSwitchSizeUpdatedCallback;
        GameManager.onStateChanged += GameStateChangedCallback;
    }

    private void Start()
    {
        // Set sleepThreshold to 0 to ensure the objects don't go to sleep and don't fall in the hole
        // Now objects are woken up in LayerSwitch and automatically woken up by magnet
        //GetComponent<Rigidbody>().sleepThreshold = 0;

        // Get hole size and check if object diameter fits
        //EnableIfObjectDiameterFits()
    }

    private void OnDestroy()
    {
        LayerSwitchSize.onDiameterIncrease -= LayerSwitchSizeUpdatedCallback;
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    public float GetValue()
    {
        return value;
    }

    public void SoftEnable()
    {
        if (softDisable)
        {
            // Set grey color for each material if to be disabled
            if (gameObject.TryGetComponent(out Renderer renderer))
            {
                foreach (Material material in renderer.materials)
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor("_EmissionColor", Color.black);
                }
            }

            // Set softDisable to false
            softDisable = false;
        }

    }

    public void SoftDisable()
    {
        //if (!softDisable)
        {
            // Set white color for each material if to be enabled
            if (gameObject.TryGetComponent(out Renderer renderer))
            {
                foreach (Material material in renderer.materials)
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor("_EmissionColor", Color.gray);
                }
                
            }

            // Set softDisable to true
            softDisable = true;
        }
    }

    public bool GetIsSoftDisabled()
    {
        return softDisable;
    }

    private void LayerSwitchSizeUpdatedCallback(float layerSwitchDiameter)
    {
        EnableIfObjectDiameterFits(layerSwitchDiameter);
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.BOSS:
                gameObject.SetActive(false);
                break;
            default:
                gameObject.SetActive(true);
                break;
        }
    }

    private void EnableIfObjectDiameterFits(float fitDiameter)
    {
        if (CheckIfObjectFits(fitDiameter))
        {
            SoftEnable();
        }
        else
        {
            SoftDisable();
        }
    }

    private bool CheckIfObjectFits(float holeDiameter)
    {
        if (gameObject.TryGetComponent(out Collider collider))
        {
            return initialXSize < holeDiameter;// && initialZSize < holeDiameter;
        }

        //TODO: This may be better to default to true lol idk
        return false;
    }

}
