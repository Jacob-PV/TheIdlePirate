using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShipManager : MonoBehaviour
{
    public double numGold;
    public Text goldText;
    public double goldPerSec;

    public Pirate shovel = new Pirate("Shovel",1,1,10,1);
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
    }

    public void Load()
    {
        numGold = double.Parse(PlayerPrefs.GetString("numGold","0"));
        goldPerSec = double.Parse(PlayerPrefs.GetString("goldPerSec","0"));
        
        shovel.m_level = PlayerPrefs.GetInt("Shovel.m_level",0);
        shovel.m_clickPower = double.Parse(PlayerPrefs.GetString("Shovel.m_clickPower","0"));
    }

    public void Save()
    {
        PlayerPrefs.SetString("numGold", numGold.ToString("f0"));
        PlayerPrefs.SetString("goldPerSec", goldPerSec.ToString());

        PlayerPrefs.SetInt("Shovel.m_level", shovel.m_level);
        PlayerPrefs.SetString("Shovel.m_clickPower", shovel.m_clickPower.ToString("f0"));
    }

    // click image funciton
    public void ShovelClick()
    {
        numGold += shovel.m_clickPower;
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
