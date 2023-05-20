using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BossUIFoodButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    [Header(" Elements ")]
    [System.NonSerialized] public GameObject foodPrefab;
    //[SerializeField] private float rawImageMoveOnTapAmount;
    //private float? originalRawImageYPosition;

    [Header(" Events ")]
    public Action touchDown, touchUp;


    public BossUIFoodButton(GameObject foodPrefab)
    {
        this.foodPrefab = foodPrefab;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if (gameObject.GetComponent<Button>().enabled)
        //{
        //    // Move the rawImage back up a little bit
        //    Vector3 targetPosition = new Vector3(gameObject.transform.position.x, (float)originalRawImageYPosition, gameObject.transform.position.z);
        //    gameObject.GetComponentInChildren<RawImage>().GetComponent<RectTransform>().position = targetPosition;

        //    // Darken the rawImage a little bit
        //    gameObject.GetComponentInChildren<RawImage>().color = Color.white;
        //}

        touchUp?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (originalRawImageYPosition == null)
        //{
        //    originalRawImageYPosition = gameObject.GetComponentInChildren<RawImage>().GetComponent<RectTransform>().position.y;
        //}

        //// Move the rawImage down a little bit
        //Vector3 targetPosition = new Vector3(gameObject.transform.position.x, (float)originalRawImageYPosition - rawImageMoveOnTapAmount, gameObject.transform.position.z);
        //gameObject.GetComponentInChildren<RawImage>().GetComponent<RectTransform>().position = targetPosition;

        //// Darken the rawImage a little bit
        //gameObject.GetComponentInChildren<RawImage>().color = Constants.Colors.lightGray;

        touchDown?.Invoke();
    }

    public void SetTexture(Texture texture)
    {
        gameObject.GetComponentInChildren<RawImage>().texture = texture;
    }

    public void SetText(string text)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

}
