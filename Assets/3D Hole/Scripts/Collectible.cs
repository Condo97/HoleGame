using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    [Header(" Settings ")]
    [SerializeField] private float value;
    private bool softDisable;
    private float initialXSize, initialZSize;

    private void Start()
    {
        // Subscribe to actions
        LayerSwitchSize.onDiameterIncrease += LayerSwitchSizeUpdatedCallback;
        GameManager.onStateChanged += GameStateChangedCallback;

        // Set sleepThreshold to 0 to ensure the objects don't go to sleep and don't fall in the hole
        // Now objects are woken up in LayerSwitch and automatically woken up by magnet
        //GetComponent<Rigidbody>().sleepThreshold = 0;

        // Set initialX and initialZ as renderer x and z at spawn.. renderer is used instead of collider because the objects may have mesh colliders and the bounds seems to be not totally accurate whereas renderer bounds uses a rect around the object
        if (gameObject.TryGetComponent(out Renderer renderer))
        {
            initialXSize = renderer.bounds.size.x;
            initialZSize = renderer.bounds.size.z;
        }
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
            // Set grey color if to be disabled
            if (gameObject.TryGetComponent(out Renderer renderer))
            {
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.black);
            }

            // Set softDisable to false
            softDisable = false;
        }

    }

    public void SoftDisable()
    {
        //if (!softDisable)
        {
            // Set white color if to be enabled
            if (gameObject.TryGetComponent(out Renderer renderer))
            {
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.gray);
                
            }

            // Set softDisable to true
            softDisable = true;
        }
    }

    public bool GetIsSoftDisabled()
    {
        return softDisable;
    }

    public void TEST_PrintRendererBounds()
    {
        Debug.Log("Collectible: " + gameObject.GetComponent<Renderer>().bounds.size);
    }

    private void LayerSwitchSizeUpdatedCallback(float holeDiameter)
    {
        if (CheckIfObjectFits(holeDiameter))
        {
            SoftEnable();
        }
        else
        {
            SoftDisable();
        }
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

    private bool CheckIfObjectFits(float holeDiameter)
    {
        if (gameObject.TryGetComponent(out Collider collider))
        {
            return initialXSize < holeDiameter && initialZSize < holeDiameter;
        }

        //TODO: This may be better to default to true lol idk
        return false;
    }

}
