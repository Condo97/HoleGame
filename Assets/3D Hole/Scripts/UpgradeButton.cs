using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI priceText;


    public void Configure(int level, int price)
    {
        levelText.text = "Lvl " + (level + 1).ToString();
        priceText.text = price.ToString(); // Can format with k and M and so on
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
