using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spinner : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject pointer;
    [SerializeField] private TextMeshProUGUI amountText;
    private bool isSpinning;
    private bool spinningLeft;
    private int currentSpinDegrees;

    [Header(" Settings ")]
    [SerializeField] private float leftRightBoundsDegrees;
    [SerializeField] private int spinFrameSpeed;

    [Header(" Spinner Bonus Locations ")]
    [SerializeField] private float x1_5LocationPercentage;
    [SerializeField] private float x2LocationPercentage;
    [SerializeField] private float x2_5LocationPercentage;
    [SerializeField] private float x3LocationPercentage;


    private void Start()
    {
        // Set to initial position, on left
        pointer.transform.Rotate(0, 0, leftRightBoundsDegrees);

        // Set curentSpinDegrees to 0
        currentSpinDegrees = 0;

        // Set spiningLeft to false
        spinningLeft = false;
        
        // Begin spinning
        isSpinning = true;
    }

    private void Update()
    {
        if (isSpinning)
        {
            if (spinningLeft)
            {
                pointer.transform.Rotate(0, 0, spinFrameSpeed);
                currentSpinDegrees -= spinFrameSpeed;
            }
            else
            {
                pointer.transform.Rotate(0, 0, -spinFrameSpeed);
                currentSpinDegrees += spinFrameSpeed;
            }

            if (currentSpinDegrees >= leftRightBoundsDegrees * 2)
                spinningLeft = true;
            else if (currentSpinDegrees <= 0)
                spinningLeft = false;

            // Update amountText
            amountText.text = string.Format($"{PayoutManager.instance.levelCompletePayout * GetBonus():n0}");
        }
    }

    public void StartSpinning()
    {
        isSpinning = true;
    }

    public void StopSpinning()
    {
        isSpinning = false;
    }

    public float GetBonus()
    {
        float percentage = currentSpinDegrees / (leftRightBoundsDegrees * 2);

        // TODO: Make this more universal
        if (percentage <= x1_5LocationPercentage)
            return 1.5f;
        if (percentage <= x2LocationPercentage)
            return 2f;
        if (percentage <= x2_5LocationPercentage)
            return 2.5f;
        if (percentage <= x3LocationPercentage)
            return 3f;

        return 1f;
    }

}
