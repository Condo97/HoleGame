using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickyLayeredImage : ClickyImage
{

    [Header(" Elements ")]
    [SerializeField] private Image topButtonImage;
    [SerializeField] private Sprite topSourceImage;
    [SerializeField] private Sprite topSourceImagePressed;


    private void Start()
    {
        if (isEnabled)
            topButtonImage.sprite = topSourceImage;
        else
            topButtonImage.sprite = topSourceImagePressed;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // Set image to topSourceImage
        topButtonImage.sprite = topSourceImage;

        base.OnPointerUp(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // Set image to topSourceImagePressed
        topButtonImage.sprite = topSourceImagePressed;

        base.OnPointerDown(eventData);
    }

    public override void Enable()
    {
        if (!isEnabled)
        {
            // Set to topSourceImage
            topButtonImage.sprite = topSourceImage;
        }

        base.Enable();
    }

    public override void Disable()
    {
        if(isEnabled)
        {
            // Set to topSourceImagePressed
            topButtonImage.sprite = topSourceImagePressed;
        }

        base.Disable();
    }

}
