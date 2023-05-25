using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI timerText;


    public void SetTimerText(string text)
    {
        timerText.text = text;
    }

}
