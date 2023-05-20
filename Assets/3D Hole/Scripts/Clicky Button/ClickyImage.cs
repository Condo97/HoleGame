using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickyImage : Clicky
{

    [Header(" Elements ")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite sourceImage;
    [SerializeField] private Sprite sourceImagePressed;

    private void Start()
    {
        if (isEnabled)
            buttonImage.sprite = sourceImage;
        else
            buttonImage.sprite = sourceImagePressed;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // Change image to sourceImage if isEnabled
        if (isEnabled)
            buttonImage.sprite = sourceImage;

        base.OnPointerUp(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // Change image to sourceImagePressed if isEnabled
        if (isEnabled)
            buttonImage.sprite = sourceImagePressed;

        base.OnPointerDown(eventData);
    }

    public override void Enable()
    {
        if (!isEnabled)
        {
            // Set to sourceImage
            buttonImage.sprite = sourceImage;
        }

        base.Enable();
    }

    public override void Disable()
    {
        if (isEnabled)
        {
            // Set to sourceImagePressed
            buttonImage.sprite = sourceImagePressed;
        }

        base.Disable();
    }

}
