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
    private bool didPrestige;
    public Text currentKeysText;

    // hold
    bool isDown = false;
    bool isDownShip = false;
    int holdInt = 0;
    float holdStart;
    float holdCurrent;
    float holdElapsed;

    // idle
    DateTime currentTime;
    DateTime oldTime;
    TimeSpan timeAway;
    int secTimeAway;
    public Text idelIncome;
    private double idleGold;
    public GameObject idleMenu;

    // breakdown
    public Text breakdownText;
    public GameObject breakdownMenu;

    // sound
    public AudioSource clickSound;

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
        didPrestige = false;

        doUpdateCrewText = true;
        doUpdateShipText = true;
        doUpdatePermText = true;
        multPerSec = 1;
        

        //idle
        currentTime = DateTime.Now;
        // Load();
        oldTime = currentTime;
        currentTime = DateTime.Now;
        timeAway = currentTime - oldTime;
        secTimeAway = timeAway.Days * 86400 + timeAway.Hours * 3600 + timeAway.Minutes * 60 + timeAway.Seconds;
        idleGold = goldPerSec * secTimeAway;
        if(idleGold > 0)
        {
            numGold += idleGold;
            totalGold += idleGold;
            Debug.Log(timeAway);
            Debug.Log(goldPerSec);

            idelIncome.text = "You earned " + DisplayNumber(idleGold) + " Gold while you were away!";
            idleMenu.gameObject.SetActive(true);
        }
        else
            idleMenu.gameObject.SetActive(false); // test

        InitShipText();
        InitCrewText();
        InitPermText();

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
        // perm text
        if(doUpdatePermText)
        {
            UpdatePermText();
            getpermMultSec();
            getGoldSec();
            SavePerm();
            UpdateHeaderText();
            doUpdatePermText = false;
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

        // color
        colorButton();
        permColorButton();
        shipButtonColor();

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
            if(claimableKeys>0)
                prestigeButton.interactable = true;
            else
                prestigeButton.interactable = false;
            currentKeysText.text = "Current Skeleton Keys: " + keys;
        }

        // breakdown
        if(breakdownMenu.gameObject.activeSelf || isFirstRun)
        {
            breakdownText.text = "Gold/Click: " + crew[0].m_clickPower
                + "\nShip Multipliers: " + multPerSec + "x"
                + "\nPermanent Multipliers: " + permMultPerSec + "x"
                + "\nSkelaton Keys: " + (keys * 0.5 + 1) + "x";
        }

        // hold upgrade
        if(isDown)
        {
            holdCurrent = Time.time;
            holdElapsed = holdCurrent - holdStart;
            if(holdElapsed > .2f)
            {
                Upgrade(holdInt);
                holdStart = Time.time;
            }
        }

        if(isDownShip)
        {
            holdCurrent = Time.time;
            holdElapsed = holdCurrent - holdStart;
            if(holdElapsed > .2f)
            {
                UpgradeShip(holdInt);
                holdStart = Time.time;
            }
        }
    }

    // text functions
    private void getGoldSec()
    {
        getpermMultSec();
        getMultSec();
        goldPerSec = 0;
        for(int i = 1; i < numCrew; i++)
        {
            goldPerSec += crew[i].m_clickPower;
        }
        goldPerSec *= multPerSec;
        goldPerSec *= permMultPerSec;
        goldPerSec *= keys*0.5 + 1;
    }

    // display numbers
    private string DisplayNumber(double number, string decimals = "f3")
    {
        if(number >= 1000000000000)
            return (number / 1000000000000).ToString(decimals) + "T";
        else if(number >= 1000000000)
            return (number / 1000000000).ToString(decimals) + "B";
        else if(number >= 1000000)
            return (number / 1000000).ToString(decimals) + "M";
        else
            return number.ToString("f0");

        // string[] suffix = new string[] {"M", "B", "T"};
        // for(int i = 6; i <= suffix.Length+6; i++)
        // {

        // }
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

        // perm
        foreach(Multiplier i in perm)
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
        permMultPerSec = double.Parse(PlayerPrefs.GetString("permMultPerSec", "0"));
    }

    private void SaveGold()
    {
        PlayerPrefs.SetString("numGold", numGold.ToString("f0"));
        PlayerPrefs.SetString("currentTime", currentTime.ToString());
        PlayerPrefs.SetString("goldPerSec", goldPerSec.ToString());
        PlayerPrefs.SetString("totalGold", totalGold.ToString());

        if(didPrestige)
        {
            PlayerPrefs.SetString("numGold", "0");
            PlayerPrefs.SetString("currentTime", currentTime.ToString());
            PlayerPrefs.SetString("goldPerSec", "0");
            PlayerPrefs.SetString("totalGold", totalGold.ToString());
        }
    }

    private void SaveAll()
    {
        SaveGold();
        SaveCrew();
        SaveShip();
        SaveAch();
    }

    public void ButtonDown(int i)
    {   
        isDown = true;
        holdInt = i;
        holdStart = Time.time;
        Upgrade(holdInt);
    }

    public void ButtonUp()
    {
        isDown = false;
    }

    public void ShipButtonDown(int i)
    {   
        isDownShip = true;
        holdInt = i;
        holdStart = Time.time;
        UpgradeShip(holdInt);
    }

    public void ShipButtonUp()
    {
        isDownShip = false;
    }

}
