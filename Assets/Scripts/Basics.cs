using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basics : MonoBehaviour
{
    // define variables
    public double numGold;
    public double goldPerSec;
    public Text goldText;
    public bool doUpdateCrewMenuText;

    // crew arrays
    const int numCrew = 5;
    public Pirate[] crew = {
        new Pirate("Shovel",1,1,10,1),
        new Pirate("Scouter",0,0,100,2),
        new Pirate("Hunter", 0,0,1000,4),
        new Pirate("Soldier",0,0,10000,8),
        new Pirate("Captain", 0,0,100000,16),
    };
    public Text[] headerText = new Text[numCrew];
    public Text[] upgradeText = new Text[numCrew];
    public double gg;
    
    // Start is called before the first frame update
    void Start()
    {
        creatText();
        doUpdateCrewMenuText = true;
        Load();
        UpdateCrewText();
        InvokeRepeating("Save",5f,5f);
    }

    // Update is called once per frame
    void Update()
    {
        gg = crew[1].m_clickPower;
        // header
        numGold += goldPerSec;
        goldText.text = "Gold: " + DisplayNumber(numGold);
        // crew menu text
        if(doUpdateCrewMenuText)
        {
            UpdateUpgradeText();
            getGoldSec();
            Save();
            doUpdateCrewMenuText = false;
        }
    }

    // click image funciton
    public void ShovelClick()
    {
        numGold += crew[0].m_clickPower;
    }

    // upgrade funcitons
    public void Upgrade(int i)
    {
        numGold -= crew[i].Upgrade(numGold);
        doUpdateCrewMenuText = crew[i].m_didUpdate;
    }

    // text functions
    private void getGoldSec()
    {
        goldPerSec = 0;
        for(int i = 1; i < numCrew; i++)
        {
            goldPerSec += crew[i].m_clickPower * Time.deltaTime;
        }
    }

    private void UpdateUpgradeText()
    {
        for(int i = 0; i < numCrew; i++)
        {
            headerText[i].text = crew[i].m_headerText;
            upgradeText[i].text = crew[i].m_upgradeText;
        }
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

    // load and save
    public void Load()
    {
        numGold = double.Parse(PlayerPrefs.GetString("numGold","0"));
        goldPerSec = double.Parse(PlayerPrefs.GetString("goldPerSec","0"));


        // shovel
        foreach(Pirate i in crew)
        {
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
            i.m_clickPower = double.Parse(PlayerPrefs.GetString(i.m_name + ".m_clickPower","0"));
        }

    }

    public void Save()
    {
        PlayerPrefs.SetString("numGold", numGold.ToString("f0"));
        PlayerPrefs.SetString("goldPerSec", goldPerSec.ToString());


        // shovel
        foreach(Pirate i in crew)
        {
            PlayerPrefs.SetInt(i.m_name + ".m_level", i.m_level);
            PlayerPrefs.SetString(i.m_name + ".m_clickPower", i.m_clickPower.ToString("f0"));
        } 

    }

    private void UpdateCrewText()
    {
        foreach(Pirate i in crew)
        {
            i.UpdateText();
        }
    }

    public void creatText()
    {
        for(int i = 0; i < numCrew; i++)
        {
            headerText[i].text = crew[i].m_headerText;
            upgradeText[i].text = crew[i].m_upgradeText;
        }
    }
}
