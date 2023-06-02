using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdClicky : Clicky
{

    [Header(" Elements ")]
    [SerializeField] private List<GameObject> normalOnlyElements; // These are shown only when the ad is not to be shown
    [SerializeField] private List<GameObject> adOnlyElements; // These are shown only when the ad is to be shown
    protected bool showAdDisplay;


    private void Start()
    {
        UpdateDisplay();
    }

    public void SetShowAdDisplay(bool showAdDisplay)
    {
        this.showAdDisplay = showAdDisplay;

        UpdateDisplay();
    }

    public virtual void UpdateDisplay()
    {
        // If should show ad, set all normalOnlyElements to not enabled and adOnlyElements to enable, otherwise set all normalonlyElements to enable adOnlyElements to not enabled
        if (showAdDisplay)
        {
            foreach (GameObject element in normalOnlyElements)
                element.SetActive(false);
            foreach (GameObject element in adOnlyElements)
                element.SetActive(true);
        }
        else
        {
            foreach (GameObject element in normalOnlyElements)
                element.SetActive(true);
            foreach (GameObject element in adOnlyElements)
                element.SetActive(false);
        }
    }

}
