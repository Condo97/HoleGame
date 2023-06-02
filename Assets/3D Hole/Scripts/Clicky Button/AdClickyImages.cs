using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdClickyImages: AdClicky
{

    [System.Serializable]
    public struct AdClickyImage
    {
        [Header(" Elements ")]
        public Image buttonImage;
        public Sprite normalSourceImage;
        public Sprite normalSourceImagePressed;
        public Sprite adSourceImage;
        public Sprite adSourceImagePressed;
    }

    [Header(" Elements ")]
    [SerializeField] private List<AdClickyImage> adImages;


    /***
     * Overrided Methods
     */

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        // If isEnabled, change each image to normal or ad source image
        if (isEnabled)
            foreach (AdClickyImage image in adImages)
                if (!showAdDisplay)
                    image.buttonImage.sprite = image.normalSourceImage;
                else
                    image.buttonImage.sprite = image.adSourceImage;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        // If isEnabled, change each image to normal or ad source image pressed
        if (isEnabled)
            foreach (AdClickyImage image in adImages)
                if (!showAdDisplay)
                    image.buttonImage.sprite = image.normalSourceImagePressed;
                else
                    image.buttonImage.sprite = image.adSourceImagePressed;
    }

    public override void Enable()
    {
        // If not isEnabled, set all to normal or ad source image
        if (!isEnabled)
            foreach (AdClickyImage image in adImages)
                if (!showAdDisplay)
                    image.buttonImage.sprite = image.normalSourceImage;
                else
                    image.buttonImage.sprite = image.adSourceImage;

        base.Enable();
    }

    public override void Disable()
    {
        // If isEnabled, set all to normal or ad source image pressed
        if (isEnabled)
            foreach (AdClickyImage image in adImages)
                if (!showAdDisplay)
                    image.buttonImage.sprite = image.normalSourceImagePressed;
                else
                    image.buttonImage.sprite = image.adSourceImagePressed;

        base.Disable();
    }

    public override void UpdateDisplay()
    {
        base.UpdateDisplay();
        
        // If should show ad, set each image to ad image based on isEnabled state
        if (showAdDisplay)
            foreach (AdClickyImage image in adImages)
                if (isEnabled)
                    image.buttonImage.sprite = image.adSourceImage;
                else
                    image.buttonImage.sprite = image.adSourceImagePressed;
    }

    /***
     * Getters and Setters
     */

    public void SetAdImages(List<AdClickyImage> images)
    {
        this.adImages = images;
    }

    /***
     * Private Methods
     */

    private void Start()
    {
        UpdateDisplay();
    }

}
