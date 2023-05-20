using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clicky : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    [Header(" Elements ")]
    [SerializeField] private List<GameObject> gameObjectsToMoveAndDarkenWhenPressed;
    [System.NonSerialized] public bool isEnabled = true;
    [System.NonSerialized] public bool pointerDown = true;

    [Header(" Settings ")]
    [SerializeField] private float amountToMoveDownWhenPressed;


    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (isEnabled)
        {
            MoveEverythingUp();
        }

        pointerDown = false;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (isEnabled)
        {
            MoveEverythingDown();

            pointerDown = true;
        }
    }

    private void OnDisable()
    {
        Debug.Log("asdfasdfasdfasdfasdf");
    }

    public virtual void Enable()
    {
        if (!isEnabled)
        {
            // Move everything up and set isEnabled to true
            MoveEverythingUp();

            isEnabled = true;
        }
    }

    public virtual void Disable()
    {
        if (isEnabled && !pointerDown)
        {
            // Move everything down and set isEnabled to false
            MoveEverythingDown();

            isEnabled = false;
        }
        else if (isEnabled && pointerDown)
        {
            // Just set isEnabled to false
            isEnabled = false;
        }
    }

    private void MoveEverythingUp()
    {
        // Move gameObjectsToMoveWhenPressed up by amountTomMoveDownWhenPressed;
        foreach (GameObject go in gameObjectsToMoveAndDarkenWhenPressed)
        {
            // Move up
            Vector3 targetPosition = new Vector3(go.transform.position.x, go.transform.position.y + amountToMoveDownWhenPressed, go.transform.position.z);
            go.transform.position = targetPosition;

            // If any of the objects are text, set to white
            if (go.TryGetComponent(out TextMeshProUGUI tmpUGUI))
                tmpUGUI.color = Color.white;

            // If any of the objects' children are text, set to white
            if (go.GetComponentInChildren<TextMeshProUGUI>())
                go.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

            // If any of the objects are images or rawImages, set to white
            if (go.TryGetComponent(out Image image))
                image.color = Color.white;
            if (go.TryGetComponent(out Image rawImage))
                rawImage.color = Color.white;

            // If any of the objects' children are images or rawImages, set to white
            if (go.GetComponentInChildren<Image>())
                foreach (Image i in go.GetComponentsInChildren<Image>())
                    i.color = Color.white;
            if (go.GetComponentInChildren<RawImage>())
                foreach (RawImage i in go.GetComponentsInChildren<RawImage>())
                    i.color = Color.white;
        }
    }

    private void MoveEverythingDown()
    {
        // Move gameObjectsToMoveWhenPressed down by amountToMoveDownWhenPressed
        foreach (GameObject go in gameObjectsToMoveAndDarkenWhenPressed)
        {
            // Move down
            Vector3 targetPosition = new Vector3(go.transform.position.x, go.transform.position.y - amountToMoveDownWhenPressed, go.transform.position.z);
            go.transform.position = targetPosition;

            // If any of the objects are text, darken them
            if (go.TryGetComponent(out TextMeshProUGUI tmpUGUI))
                tmpUGUI.color = Constants.Colors.lightGray;

            // If any of the objects' children are text, darken them
            if (go.GetComponentInChildren<TextMeshProUGUI>())
                go.GetComponentInChildren<TextMeshProUGUI>().color = Constants.Colors.lightGray;

            // If any of the objects are images or rawImages, darken them
            //if (go.TryGetComponent(out Image image))
            //    image.color = Constants.Colors.lightGray;
            //if (go.TryGetComponent(out RawImage rawImage))
            //    rawImage.color = Constants.Colors.lightGray;

            //// If any of the objects' children are images, darken them
            //if (go.GetComponentInChildren<Image>())
            //    foreach (Image i in go.GetComponentsInChildren<Image>())
            //        i.color = Constants.Colors.lightGray;
            //if (go.GetComponentInChildren<RawImage>())
            //    foreach (RawImage i in go.GetComponentsInChildren<RawImage>())
            //        i.color = Constants.Colors.lightGray;

        }
    }

}
