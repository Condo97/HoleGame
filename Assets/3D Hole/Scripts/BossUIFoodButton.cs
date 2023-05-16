using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BossUIFoodButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    [Header(" Elements ")]
    [System.NonSerialized] public GameObject foodPrefab;

    [Header(" Events ")]
    public Action touchDown, touchUp;


    public BossUIFoodButton(GameObject foodPrefab)
    {
        this.foodPrefab = foodPrefab;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        touchUp?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchDown?.Invoke();
    }

}
