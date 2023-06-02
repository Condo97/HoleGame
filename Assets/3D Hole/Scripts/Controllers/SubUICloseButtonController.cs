using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubUICloseButtonController : MonoBehaviour, IPointerUpHandler
{

    [Header(" Elements ")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject uiElementToDefaultTo;


    public void OnPointerUp(PointerEventData eventData)
    {
        Close();
    }

    private void Close()
    {
        uiManager.CloseSubUI(uiElementToDefaultTo);
    }

}
