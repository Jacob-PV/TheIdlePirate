using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Advertisements;

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
    private double shovelPower;

    public double multPerSec;

    private bool doUpdateCrewText;
    private bool doUpdateShipText;

    // achievments
    public GameObject achMenu;
    public int totalCrewUpgrades;
    private int totalShovelClicks;

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
    bool isDownPerm = false;
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
    int doubleIdleUses;
    public GameObject adErrorMenu;

    // breakdown
    public Text breakdownText;
    public GameObject breakdownMenu;

    // sound
    public AudioSource clickSound;
    const int numSongs = 3;
    public AudioSource[] music = new AudioSource[numSongs];
    public GameObject musicOnObject;
    private bool musicOn;
    public bool sfxOn;
    public GameObject sfxButtonOn;
    int songIndex;

    // alert
    public GameObject crewAlert;
    public GameObject shipAlert;
    public GameObject permAlert;
    public GameObject achAlert;
    public GameObject prestigeAlert;
    public GameObject prestigeAlert2;

    //
    private int indMultInt;

    // Start is called before the first frame update
    void Start()
    {
        // ads
        Advertisement.Initialize("4176969");
        Advertisement.AddListener(this);

        // frames
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        isFirstRun = true;
        totalCrewUpgrades = 0;
        totalShovelClicks = 0;
        doubleIdleUses = 0;
        numRubies = 0;
        claimableKeys = 0;
        didPrestige = false;
        indMultInt = 0;
        // sound
        musicOn = true;
        sfxOn = true;

        doUpdateCrewText = true;
        doUpdateShipText = true;
        doUpdatePermText = true;
        multPerSec = 1;
        

        //idle
        currentTime = DateTime.Now;
        Load();
        InitShipText();
        InitCrewText();
        InitPermText();
        InitAchText();

        InitSound();
        numGold = 9999999999999;
        totalGold = 99999999999999;
        oldTime = currentTime;
        currentTime = DateTime.Now;
        timeAway = currentTime - oldTime;
        secTimeAway = timeAway.Days * 86400 + timeAway.Hours * 3600 + timeAway.Minutes * 60 + timeAway.Seconds;
        idleGold = goldPerSec * secTimeAway;
        if(idleGold >= 1)
        {
            // numGold += idleGold;
            // totalGold += idleGold;
            // Debug.Log(timeAway);
            // Debug.Log(goldPerSec);
            secTimeAway = 99999;
            GreedPunish();
            numGold += idleGold;

            idelIncome.text = "You earned " + DisplayNumber(idleGold) + " Gold while you were away!";
            idleMenu.gameObject.SetActive(true);
        }
        else
            idleMenu.gameObject.SetActive(false); // test

        InitShipText();
        InitCrewText();
        InitPermText();
        InitAchText();

        InitSound();

        InvokeRepeating("SaveGold",0f,5f);

        //tmp
        rubiesText.text = "Rubies: 0";

        // Debug.Log(DisplayNumber(2 * 743008370688 * Mathf.Pow(1.15f,410)));
    }

    // Update is called once per frame
    void Update()
    {
        // save
        if(isFirstRun)
            SaveAll();

        CheckAlerts();

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
            SaveGold(); 
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
            SaveGold(); 
            UpdateHeaderText();
            rubiesText.text = "Rubies: " + numRubies;
            doUpdatePermText = false;
        }
        // crew menu text
        if(doUpdateCrewText)
        {
            UpdateUpgradeText();
            getGoldSec();
            SaveCrew();
            SaveGold(); 
            doUpdateCrewText = false;
            UpdateHeaderText();
            if(!isFirstRun)
                totalCrewUpgrades++;
        }

        // greed
        if(greedMenu.gameObject.activeSelf)
        {
            CheckGreedSlider();
        }

        // color
        colorButton();
        permColorButton();
        shipButtonColor();
        if(indMultMenu.gameObject.activeSelf)
            ColorMults();

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
            // ^15 ref
            claimableKeys = Math.Floor(150 * Math.Pow((totalGold/Math.Pow(10,13)),.5) - keys);
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
            GetShovelPower();
            breakdownText.text = "Gold/Click: " + DisplayNumber(shovelPower, true)
                + "\nShip Boost: " + DisplayNumber((multPerSec-1)*100, true) + "%"
                + "\nPermanent Boost: " + DisplayNumber((permMultPerSec-1)*100, true) + "%"
                + "\nGreed Boost: " + DisplayNumber(greedPercent) + "%"
                + "\nSkelaton Keys: " + DisplayNumber((keys * 0.05) * 100, true) + "%";
        }

        // music
        if(musicOn)
            MusicUpdater();

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

        if(isDownPerm)
        {
            holdCurrent = Time.time;
            holdElapsed = holdCurrent - holdStart;
            if(holdElapsed > .2f)
            {
                UpgradePerm(holdInt);
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
        goldPerSec *= keys*0.05 + 1;
        goldPerSec *= (greedPercent + 100) / 100;
    }

    private void CheckAlerts()
    {
        // crew
        // mults
        CheckMultAlert();
        if(!doAlertMult)
        {
            foreach(Pirate i in crew)
                if(i.m_upgradeCost <= numGold)
                {
                    crewAlert.gameObject.SetActive(true);
                    break;
                }
                else
                    crewAlert.gameObject.SetActive(false);
        }
        else
            crewAlert.gameObject.SetActive(true);

        // ship
        foreach(Multiplier i in ship)
            if(i.m_upgradeCost <= numGold)
            {
                shipAlert.gameObject.SetActive(true);
                break;
            }
            else
                shipAlert.gameObject.SetActive(false);

        // perm
        foreach(Multiplier i in perm)
            if(i.m_upgradeCost <= numRubies)
            {
                permAlert.gameObject.SetActive(true);
                break;
            }
            else
                permAlert.gameObject.SetActive(false);

        // ach
        UpdateAchievementsText();
        for(int i = 0; i < numAchievements; i++)
            if(!achievements[i].m_maxed && current[i] >= achievements[i].m_tiers[achievements[i].m_level-1])
            {
                achAlert.gameObject.SetActive(true);
                break;
            }
            else
                achAlert.gameObject.SetActive(false);

        // prestige
        CalculateKeys();
        if(claimableKeys >= 1)
        {
            prestigeAlert.gameObject.SetActive(true);
            prestigeAlert2.gameObject.SetActive(true);
        }
        else
        {
            prestigeAlert.gameObject.SetActive(false);
            prestigeAlert2.gameObject.SetActive(false);
        }


    }

    // display numbers
    private string DisplayNumber(double number, bool mult = false, string decimals = "f3")
    {
        // if(number >= 1000000000000)
        //     return (number / 1000000000000).ToString(decimals) + "T";
        // else if(number >= 1000000000)
        //     return (number / 1000000000).ToString(decimals) + "B";
        // else if(number >= 1000000)
        //     return (number / 1000000).ToString(decimals) + "M";
        // else
        //     return number.ToString("f0");

        // string[] suffix = new string[] {"Million","Billion","Trillion","Quadrillion","Quintillion","Sextillion","Septillion","Octillion","Nonillion","Decillion",
            // "Undecillion","Duodecillion","Tredecillion","Quattuordecillion","Quindecillion","Sexdecillion","Septendecillion","Octodecillion","Novemdecillion","Vigintillion"};
        string[] suffix = new string[] {"M","B","T","Qa","Qi","Sx","Sp","Oc","No","Dc","Ud","Dd","Td","Qad","Qid","Sxd",
            "Spd","Ocd","Nod","Vg","Uvg","Dvg"};

        int suffixIndex = 0;
        for(int i = 6; i <= suffix.Length+6; i=i+3)
        {
            if(number/Math.Pow(10,i)<1000 && number/Math.Pow(10,i)>=1)
            {
                return (number / Math.Pow(10,i)).ToString(decimals) + " " + suffix[suffixIndex];
            }
            suffixIndex++;
        }
        if(mult)
            return number.ToString("f3");
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
        // foreach(Pirate i in crew)
        // {
        //     i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
        //     i.m_clickPower = double.Parse(PlayerPrefs.GetString(i.m_name + ".m_clickPower","0"));
        // }
        crew[0].m_level = PlayerPrefs.GetInt(crew[0].m_name + ".m_level",1);
        crew[0].m_clickPower = double.Parse(PlayerPrefs.GetString(crew[0].m_name + ".m_clickPower","1"));
        foreach(Pirate i in crew)
        {
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
            i.m_clickPower = double.Parse(PlayerPrefs.GetString(i.m_name + ".m_clickPower","0"));
            // mult
            // for(int j = 0; j < numMults; j++)
            //     i.m_tiersBought[j] = bool.Parse(PlayerPrefs.GetString(i.m_name + "tiersBought[" + j + "]", "false"));
            i.m_currentTier = PlayerPrefs.GetInt(i.m_name + "m_currentTier", 0);
        }

        // ship
        foreach(Multiplier i in ship)
        {
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
        }

        // achievements
        numRubies = PlayerPrefs.GetInt("numRubies",0);
        totalCrewUpgrades = PlayerPrefs.GetInt("totalCrewUpgrades",0); //saved in crew
        totalShovelClicks = PlayerPrefs.GetInt("totalShovelClicks",0); //saved in crew
        doubleIdleUses = PlayerPrefs.GetInt("doubleIdleUses", 0);
        foreach(Achievement i in achievements)
        {
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
        }

        // perm
        foreach(Multiplier i in perm)
            i.m_level = PlayerPrefs.GetInt(i.m_name + ".m_level",0);
        permMultPerSec = double.Parse(PlayerPrefs.GetString("permMultPerSec", "0"));

        // greed
        greedPercent = PlayerPrefs.GetFloat("greedPercent", 0);
        greed.value = greedPercent;

        // sound
        musicOn = bool.Parse(PlayerPrefs.GetString("musicOn","true"));
        sfxOn = bool.Parse(PlayerPrefs.GetString("sfxOn","true"));
    }

    private void SaveGold()
    {
        Debug.Log("Saved");
        PlayerPrefs.SetString("numGold", numGold.ToString("f0"));
        PlayerPrefs.SetString("currentTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("goldPerSec", goldPerSec.ToString());
        PlayerPrefs.SetString("totalGold", totalGold.ToString());

        if(didPrestige)
        {
            PlayerPrefs.SetString("numGold", "0");
            PlayerPrefs.SetString("currentTime", DateTime.Now.ToString());
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

    public void PermButtonDown(int i)
    {   
        isDownPerm = true;
        holdInt = i;
        holdStart = Time.time;
        UpgradePerm(holdInt);
    }

    public void PermButtonUp()
    {
        isDownPerm = false;
    }

    public void MusicController()
    {
        // switch on/off
        if(musicOn)
        {
            musicOn = false;
            musicOnObject.gameObject.SetActive(false);
            music[songIndex].Pause();
        }
        else
        {
            musicOn = true;
            musicOnObject.gameObject.SetActive(true);
            music[songIndex].Play();
        }

        // loop through songs

        SaveSound();
    }

    public void SoundEffectsController()
    {
        if(sfxOn)
        {
            sfxOn = false;
            sfxButtonOn.gameObject.SetActive(false);
            clickSound.mute = true;
        }
        else
        {
            sfxOn = true;
            sfxButtonOn.gameObject.SetActive(true);
            clickSound.mute = false;
        }

        SaveSound();
    }

    private void InitSound()
    {
        songIndex = UnityEngine.Random.Range(0,numSongs);

        // check if music on/off
        if(musicOn)
        {
            musicOnObject.gameObject.SetActive(true);
            music[songIndex].Play();
        }
        else
        {
            musicOnObject.gameObject.SetActive(false);
            // music[songIndex].Pause();
        }

        if(sfxOn)
        {
            sfxButtonOn.gameObject.SetActive(true);
            clickSound.mute = false;
        }
        else
        {
            sfxButtonOn.gameObject.SetActive(false);
            clickSound.mute = true;
        }
    }

    private void MusicUpdater()
    {
        if(musicOn)
        {
            if(music[songIndex].isPlaying == false)
            {
                songIndex++;
                if(songIndex >= numSongs)
                    songIndex = 0;
                music[songIndex].Play();
            }
        }
    }

    private void SaveSound()
    {
        PlayerPrefs.SetString("musicOn",musicOn.ToString());
        PlayerPrefs.SetString("sfxOn",sfxOn.ToString());
    }

    public void ClickSound()
    {
        if(sfxOn)
            clickSound.Play();
    }

}
