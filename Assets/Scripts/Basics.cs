using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class Basics : MonoBehaviour
{
    // define variables
    public double numGold; 
    public double goldPerSec;
    public Text goldText;
    public Text goldPerSecText;
    public Text multPerSecText;
    public int numRubies;
    public Text rubiesText;
    private bool isFirstRun;
    public double totalGold;

    public double multPerSec;

    private bool doUpdateCrewText;
    private bool doUpdateShipText;

    // achievments
    public GameObject achMenu;
    public int totalCrewUpgrades;

    // prestige
    public GameObject prestigeMenu;
    private double claimableKeys;
    public Text prestigeClaimText;
    public Button prestigeButton;
    private double keys;

    // idle
    DateTime currentTime;
    DateTime oldTime;
    TimeSpan timeAway;
    int secTimeAway;

    // Start is called before the first frame update
    void Start()
    {
        // frames
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        isFirstRun = true;
        totalCrewUpgrades = 0;
        numRubies = 0;
        claimableKeys = 0;

        doUpdateCrewText = true;
        doUpdateShipText = true;
        multPerSec = 1;
        

        //idle
        currentTime = DateTime.Now;
        // Load();
        oldTime = currentTime;
        currentTime = DateTime.Now;
        if(oldTime != currentTime)
        {
            timeAway = currentTime - oldTime;
            secTimeAway = timeAway.Days * 86400 + timeAway.Hours * 3600 + timeAway.Minutes * 60 + timeAway.Seconds;
            numGold += goldPerSec * secTimeAway;
            Debug.Log(timeAway);
            Debug.Log(goldPerSec);
        }

        InitShipText();
        InitCrewText();

        InvokeRepeating("SaveGold",5f,5f);

        //tmp
        rubiesText.text = "Rubies: 0";
    }

    // Update is called once per frame
    void Update()
    {
        // save
        if(isFirstRun)
            SaveAll();

        // header
        numGold += goldPerSec * Time.deltaTime;
        totalGold += goldPerSec * Time.deltaTime;
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
        }
        // crew menu text
        if(doUpdateCrewText)
        {
            UpdateUpgradeText();
            getGoldSec();
            SaveCrew();
            doUpdateCrewText = false;
            UpdateHeaderText();
            if(!isFirstRun)
                totalCrewUpgrades++;
        }
        // ach menu text
        if(achMenu.gameObject.activeSelf || isFirstRun)
        {
            UpdateAchievementsText();
            //move once get perm buy menu
            rubiesText.text = "Rubies: " + numRubies;
            isFirstRun = false;
        }
        
        // prestige
        if(prestigeMenu.gameObject.activeSelf || isFirstRun)
        {
            claimableKeys = Math.Floor(totalGold / 1000000);
            prestigeClaimText.text = claimableKeys + " Skelaton Keys\nClaim";
            if(claimableKeys>=0)
                prestigeButton.interactable = true;
            else
                prestigeButton.interactable = false;
        }
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
        goldPerSec *= keys*0.5 + 1;
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
        currentTime = DateTime.Parse(PlayerPrefs.GetString("currentTime", DateTime.Now.ToString()));
        totalGold = double.Parse(PlayerPrefs.GetString("totalGold"));

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
        numRubies = PlayerPrefs.GetInt("numRubies",0);
        totalCrewUpgrades = PlayerPrefs.GetInt("totalCrewUpgrades",0); //saved in crew
        foreach(Achievement i in achievements)
        {
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
        }
    }

    private void SaveGold()
    {
        PlayerPrefs.SetString("numGold", numGold.ToString("f0"));
        PlayerPrefs.SetString("currentTime", currentTime.ToString());
        PlayerPrefs.SetString("goldPerSec", goldPerSec.ToString());
        PlayerPrefs.SetString("totalGold", totalGold.ToString());
    }

    private void SaveAll()
    {
        SaveGold();
        SaveCrew();
        SaveShip();
        SaveAch();
    }

    
    public void Prestige()
    {
        keys += claimableKeys;
        claimableKeys = 0;
        numGold = 0;
        
    }

    // private IdleIncome()
    // {

    // }

    // crashes apparently
    // float elapsed = 0f;
    // public void HoldUpgrade(int i)
    // {
    //     while(Input.GetMouseButtonDown(0))
    //     {
    //         elapsed += Time.deltaTime;
    //         if(elapsed >= 0.2f)
    //         {
    //             elapsed = 0f;
    //             Upgrade(i);
    //         }
    //         Debug.Log(Input.GetMouseButtonDown(0));
    //     }
    //     Debug.Log(Input.GetMouseButtonDown(0));
    //     Upgrade(i);
    // }
    public void HoldUpgrade(int i)
    {
        float elpased = 0f;
        bool exit = false;
        while(!exit)
        {
            Debug.Log(Input.GetMouseButton(0));
            elpased += Time.deltaTime;
            Debug.Log(elpased);
            if(!Input.GetMouseButton(0))
                exit = true;
            if(elpased > 300f)
                exit = true;
        }
        Debug.Log("out");
    }
}
