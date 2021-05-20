using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShipManager : MonoBehaviour
{
    public double numGold;
    public Text goldText;
    public double goldPerSec;
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        numGold += goldPerSec;
        goldText.text = "Gold: " + DisplayNumber(numGold);
        Debug.Log(goldPerSec);
    }

    public void Load()
    {
        numGold = double.Parse(PlayerPrefs.GetString("numGold","0"));
        goldPerSec = double.Parse(PlayerPrefs.GetString("goldPerSec","0"));
        Debug.Log("yolo");
    }

        // display numbers
    private string DisplayNumber(double number, string decimals = "f3")
    {
        if(number >= 1000000000)
            return (number / 1000000000).ToString(decimals) + "B";
        else if(number >= 1000000)
            return (number / 1000000).ToString(decimals) + "M";
        else
            return number.ToString("f0");
    }
}
