using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public double multPerSec;

    private bool doUpdateCrewText;
    private bool doUpdateShipText;

    // achievments
    public GameObject achMenu;
    public int totalCrewUpgrades;

    // Start is called before the first frame update
    void Start()
    {
        // frames
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        isFirstRun = true;
        totalCrewUpgrades = 0;
        numRubies = 0;

        doUpdateCrewText = true;
        doUpdateShipText = true;
        multPerSec = 1;
        Load();
        
        InitShipText();
        InitCrewText();

        InvokeRepeating("SaveGold",5f,5f);

        //tmp
        rubiesText.text = "Rubies: 0";
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
    }

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
    // public void HoldUpgrade(int i)
    // {
    //     float elpased = 0f;
    //     while(true)
    //     {
    //         elpased += Time.deltaTime;
    //         Debug.Log(elpased);
    //         if(Input.GetMouseButtonUp(0))
    //             break;
    //     }
    //     Debug.Log("out");
    // }
}
