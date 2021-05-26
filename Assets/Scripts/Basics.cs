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
    public Text goldPerSecText;
    public Text multPerSecText;

    public double multPerSec;

    private bool doUpdateCrewText;
    private bool doUpdateShipText;

    // achievments
    public int totalCrewUpgrades;

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

    // ship arrays
    const int numShip = 1;
    public Multiplier[] ship = {
        new Multiplier("Sail",2,0,100),
    };
    public Text[] shipHeaderText = new Text[numShip];
    public Text[] shipUpgradeText = new Text[numShip];
    
    
    // Start is called before the first frame update
    void Start()
    {
        // frames
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        totalCrewUpgrades = 0;

        doUpdateCrewText = true;
        doUpdateShipText = true;
        multPerSec = 1;
        Load();
        
        InitShipText();
        InitCrewText();

        InvokeRepeating("SaveGold",5f,5f);
    }

    // Update is called once per frame
    void Update()
    {


        // header
        numGold += goldPerSec * Time.deltaTime;
        goldText.text = "Gold: " + DisplayNumber(numGold);
        
        // ship text
        if(doUpdateShipText)
        {
            UpdateShipText();
            getMultSec();
            getGoldSec();
            SaveShip(); 
            doUpdateShipText = false;
            UpdateHeaderText();
            totalCrewUpgrades++;
        }
        // crew menu text
        if(doUpdateCrewText)
        {
            UpdateUpgradeText();
            getGoldSec();
            SaveCrew();
            doUpdateCrewText = false;
            UpdateHeaderText();
        }
    }

    private void UpdateHeaderText()
    {
        goldPerSecText.text = "Gold/Sec: " + DisplayNumber(goldPerSec);
        multPerSecText.text = "Multiplier: " + multPerSec + "x";
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
        doUpdateCrewText = crew[i].m_didUpdate;
    }

    // text functions
    private void getGoldSec()
    {
        goldPerSec = 0;
        for(int i = 1; i < numCrew; i++)
        {
            goldPerSec += crew[i].m_clickPower;
        }
        goldPerSec *= multPerSec;
    }

    private void UpdateUpgradeText()
    {
        for(int i = 0; i < numCrew; i++)
        {
            headerText[i].text = crew[i].m_headerText;
            upgradeText[i].text = crew[i].m_upgradeText;
        }
    }

    private void InitCrewText()
    {
        foreach(Pirate i in crew)
            i.InitText();
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


        // crew
        foreach(Pirate i in crew)
        {
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
            i.m_clickPower = double.Parse(PlayerPrefs.GetString(i.m_name + ".m_clickPower","0"));
        }

        // ship
        foreach(Multiplier i in ship)
        {
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
        }

        // achievements
        totalCrewUpgrades = PlayerPrefs.GetInt("totalCrewUpgrades",0);
    }

    private void SaveGold()
    {
        PlayerPrefs.SetString("numGold", numGold.ToString("f0"));
    }

    public void SaveCrew()
    {
        // PlayerPrefs.SetString("numGold", numGold.ToString("f0"));
        // PlayerPrefs.SetString("goldPerSec", goldPerSec.ToString());

        // crew
        foreach(Pirate i in crew)
        {
            PlayerPrefs.SetInt(i.m_name + ".m_level", i.m_level);
            PlayerPrefs.SetString(i.m_name + ".m_clickPower", i.m_clickPower.ToString("f0"));
        } 

        // achievments
        PlayerPrefs.SetInt("totalCrewUpgrades", totalCrewUpgrades);
    }

    private void UpdateCrewText()
    {
        foreach(Pirate i in crew)
            i.UpdateText();
    }

    // ship
    private void UpdateShipText()
    {
        for(int i = 0; i < numShip; i++)
        {
            shipHeaderText[i].text = ship[i].m_headerText;
            shipUpgradeText[i].text = ship[i].m_upgradeText;
        }
    }

    public void UpgradeShip(int i)
    {
        numGold -= ship[i].Upgrade(numGold);
        doUpdateShipText = ship[i].m_didUpdate;
    }

    public void getMultSec()
    {
        multPerSec = 1;
        for(int i = 0; i < numShip; i++)
        {
            multPerSec *= ship[i].m_currentMultiple;
        }
    }

    private void SaveShip()
    {
        foreach(Multiplier i in ship)
        {
            PlayerPrefs.SetInt(i.m_name + ".m_level", i.m_level);
        } 
    }

    private void InitShipText()
    {
        foreach(Multiplier i in ship)
        {
            i.InitText();
        }
    }
}
