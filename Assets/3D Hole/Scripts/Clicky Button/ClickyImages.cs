using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickyImages : Clicky
{

    [System.Serializable]
    public class ClickyImage
    {
        [Header(" Elements ")]
        public Image buttonImage;
        public Sprite sourceImage;
        public Sprite sourceImagePressed;
    }

    [Header(" Elements ")]
    [SerializeField] private List<ClickyImage> images;


    protected void Start()
    {
        // Initially change each image depending on isEnabled state
        foreach (ClickyImage image in images)
            if (isEnabled)
                image.buttonImage.sprite = image.sourceImage;
            else
                image.buttonImage.sprite = image.sourceImagePressed;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        // Change each image to sourceImage if isEnabled
        if (isEnabled)
            foreach (ClickyImage image in images)
                image.buttonImage.sprite = image.sourceImage;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        // Change image to sourceImagePressed if isEnabled
        if (isEnabled)
            foreach (ClickyImage image in images)
                image.buttonImage.sprite = image.sourceImagePressed;
    }

    public override void Enable()
    {
        if (!isEnabled)
        {
            // Set all to sourceImage
            foreach (ClickyImage image in images)
                image.buttonImage.sprite = image.sourceImage;
        }

        base.Enable();
    }

    public override void Disable()
    {
        if (isEnabled)
        {
            // Set all to sourceImagePressed
            foreach (ClickyImage image in images)
                image.buttonImage.sprite = image.sourceImagePressed;
        }

        base.Disable();
    }

    public void SetImages(List<ClickyImage> images)
    {
        this.images = images;
    }

}
