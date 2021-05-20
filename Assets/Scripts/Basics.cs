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
    // shovel vars
    public Pirate shovel = new Pirate("Shovel",1,1,10,1);
    public Text shovelText;
    public Text shovelUpgradeText;
    // scouter vars
    public Pirate scouter = new Pirate("Scouter",0,0,100,2);
    public Text scouterText;
    public Text scouterUpgradeText;
    // hunter vars
    public Pirate hunter = new Pirate("Hunter", 0,0,1000,4);
    public Text hunterText;
    public Text hunterUpgradeText;
    // soldier Vars
    public Text soldierText;
    public Pirate soldier = new Pirate("Soldier",0,0,10000,8);
    public Text soldierUpgradeText;
    // captain vars
    public Pirate captain = new Pirate("Captain", 0,0,100000,16);
    public Text captainText;
    public Text captainUpgradeText;

    // crew
    int numCrew = 5;
    // public Pirate[] crew = {
    //     new Pirate("Shovel",1,1,10,1),
    //     new Pirate("Scouter",0,0,100,2),
    //     new Pirate("Hunter", 0,0,1000,4),
    //     new Pirate("Soldier",0,0,10000,8),
    //     new Pirate("Captain", 0,0,100000,16),
    // };

    // Start is called before the first frame update
    void Start()
    {
        doUpdateCrewMenuText = true;
        Load();
        UpdateCrewText();
        InvokeRepeating("Save",5f,5f);
    }

    // Update is called once per frame
    void Update()
    {
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
        numGold += shovel.m_clickPower;
    }

    // upgrade funcitons
    public void ShovelUpgrade()
    {
        numGold -= shovel.Upgrade(numGold);
        doUpdateCrewMenuText = shovel.m_didUpdate;
    }

    public void SoldierUpgrade()
    {
        numGold -= soldier.Upgrade(numGold);
        doUpdateCrewMenuText = soldier.m_didUpdate;
    }

    public void ScouterUpgrade()
    {
        numGold -= scouter.Upgrade(numGold);
        doUpdateCrewMenuText = scouter.m_didUpdate;
    }

    public void HunterUpgrade()
    {
        numGold -= hunter.Upgrade(numGold);
        doUpdateCrewMenuText = hunter.m_didUpdate;
    }

    public void CaptainUpgrade()
    {
        numGold -= captain.Upgrade(numGold);
        doUpdateCrewMenuText = captain.m_didUpdate;
    }


    // text functions
    private void getGoldSec()
    {
        // soldier
        goldPerSec += soldier.m_clickPower * Time.deltaTime;
        // scouter
        goldPerSec += scouter.m_clickPower * Time.deltaTime;
        // hunter
        goldPerSec += hunter.m_clickPower * Time.deltaTime;
        // captain
        goldPerSec += captain.m_clickPower * Time.deltaTime;
    }

    private void UpdateUpgradeText()
    {
        // shovel
        shovelText.text = shovel.m_headerText;
        shovelUpgradeText.text = shovel.m_upgradeText;
        // soldier
        soldierText.text = soldier.m_headerText;
        soldierUpgradeText.text = soldier.m_upgradeText;
        // scouter
        scouterText.text = scouter.m_headerText;
        scouterUpgradeText.text = scouter.m_upgradeText;
        // hunter
        hunterText.text = hunter.m_headerText;
        hunterUpgradeText.text = hunter.m_upgradeText;
        // captain
        captainText.text = captain.m_headerText;
        captainUpgradeText.text = captain.m_upgradeText;
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
        shovel.m_level = PlayerPrefs.GetInt("shovel.m_level",0);
        shovel.m_clickPower = double.Parse(PlayerPrefs.GetString("shovel.m_clickPower","0"));
    }

    public void Save()
    {
        PlayerPrefs.SetString("numGold", numGold.ToString("f0"));
        PlayerPrefs.SetString("goldPerSec", goldPerSec.ToString());
        Debug.Log(goldPerSec);


        // shovel 
        PlayerPrefs.SetInt("shovel.m_level", shovel.m_level);
        PlayerPrefs.SetString("shovel.m_clickPower", shovel.m_clickPower.ToString("f0"));

        Debug.Log("saved");
    }

    private void UpdateCrewText()
    {
        shovel.UpdateText();
        scouter.UpdateText();
        soldier.UpdateText();
        hunter.UpdateText();
        captain.UpdateText();
    }
}
