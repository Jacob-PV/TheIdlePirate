using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// CREW
public partial class Basics : MonoBehaviour
{
    // VARS
    const int numCrew = 11;
    const int numMults = 7;
    public Pirate[] crew = {
        // price: 12^x
        // power: 3^(x-1)
        new Pirate("Shovel",1,1,10,1),
        new Pirate("Swashbuckler",0,0,144,3),
        new Pirate("Cannon Fondler",0,0,1728,9),
        new Pirate("Scout",0,0,20736,27),
        new Pirate("Hunter",0,0,248832,81),
        new Pirate("Blunderbuss",0,0,2985984,243),
        new Pirate("Soldier",0,0,35831808,729),
        new Pirate("Sword Master",0,0,429981696,729),
        new Pirate("Sharpshooter", 0,0,5159780352,6561),
        new Pirate("Heavy Gunner",0,0,61917364224,19683),
        new Pirate("Captain", 0,0,743008370688,59049),
    };

    public Text[] headerText = new Text[numCrew];
    public Text[] upgradeText = new Text[numCrew];
    public Button[] upgradeButton = new Button[numCrew];

    // mults
    public Text[] multText = new Text[numMults];
    public Text[] multButtonText = new Text[numMults];
    public Button[] multButton = new Button[numMults];
    public GameObject indMultMenu;

    // mult2
    public Text indMultTotalBoostText;
    public Text indMultLevelText;
    public Text indMultBoostText;
    public Text indMultNextLevelText;
    public Button indMultButton;
    public Text indMultButtonText;
    private int currentTier;

    // METHODS
    private void InitCrewText()
    {
        foreach(Pirate i in crew)
            i.InitText();
    }

    private void UpdateHeaderText()
    {
        goldPerSecText.text = "Gold/Sec: " + DisplayNumber(goldPerSec);
        multPerSecText.text = "Multiplier: " + multPerSec + "x";
    }
    
    private void UpdateUpgradeText()
    {
        for(int i = 0; i < numCrew; i++)
        {
            headerText[i].text = crew[i].m_headerText;
            upgradeText[i].text = crew[i].m_upgradeText;
        }
    }

    // click image funciton
    public void ShovelClick()
    {
        GetShovelPower();
        numGold += shovelPower;
        totalGold += shovelPower;
        totalShovelClicks++;
    }

    private void GetShovelPower()
    {
        shovelPower = crew[0].m_clickPower;
        shovelPower *= multPerSec;
        shovelPower *= permMultPerSec;
        shovelPower *= keys*0.5 + 1;
        shovelPower *= (greedPercent + 100) / 100;
    }

    // upgrade funcitons
    public void Upgrade(int i)
    {
        numGold -= crew[i].Upgrade(numGold);
        doUpdateCrewText = crew[i].m_didUpdate;
        if(doUpdateCrewText)
            ClickSound();
    }

    private void UpdateCrewText()
    {
        foreach(Pirate i in crew)
            i.UpdateText();
    }

    public void SaveCrew(bool prestige = false)
    {
        // crew
        foreach(Pirate i in crew)
        {
            PlayerPrefs.SetInt(i.m_name + ".m_level", i.m_level);
            PlayerPrefs.SetString(i.m_name + ".m_clickPower", i.m_clickPower.ToString("f0"));
            // mult
            // for(int j = 0; j < numMults; j++)
            //     PlayerPrefs.SetString(i.m_name + "tiersBought[" + j + "]", i.m_tiersBought[j].ToString());
            PlayerPrefs.SetInt(i.m_name + "m_currentTier", i.m_currentTier);
        } 

        // achievments
        PlayerPrefs.SetInt("totalCrewUpgrades", totalCrewUpgrades);
        PlayerPrefs.SetInt("totalShovelClicks", totalShovelClicks);

        if(prestige)
        {
            foreach(Pirate i in crew)
            {
                PlayerPrefs.SetInt(i.m_name + ".m_level", 0);
                PlayerPrefs.SetInt(i.m_name + "m_currentTier", 0);
            }
            PlayerPrefs.SetInt("Shovel.m_level", 1);
        }
    }

    // button
    private void colorButton()
    {
        for(int i = 0; i < numCrew; i++)
        {
            if(crew[i].m_upgradeCost <= numGold)
                upgradeButton[i].interactable = true;
            else
                upgradeButton[i].interactable = false;
        }
    }

    public void OpenIndMults(int n)
    {
        indMultInt = n;
        UpdateIndMultText();
        indMultMenu.gameObject.SetActive(true);
    }

    private void UpdateIndMultText()
    {
        foreach(Text i in multText)
            i.text = "2x " + crew[indMultInt].m_name + " GPS";
        // for(int i = 0; i < numMults; i++)
        // {
        //     if(!crew[indMultInt].m_tiersBought[i])
        //         multButtonText[i].text = DisplayNumber(crew[indMultInt].m_upgradeTierCosts[i]);
        //     else
        //         multButtonText[i].text = "Claimed";
        // }
        currentTier = crew[indMultInt].m_currentTier;
        indMultButtonText.text = DisplayNumber(crew[indMultInt].m_upgradeTierCosts[currentTier]);
        if(currentTier < numMults)
        {
            indMultButtonText.text = DisplayNumber(crew[indMultInt].m_upgradeTierCosts[currentTier]);
            indMultTotalBoostText.text = "Current " + crew[indMultInt].m_name + " Boost: " + DisplayNumber((crew[indMultInt].m_indMult - 1) * 100) + "%";
            indMultLevelText.text = "Level " + crew[indMultInt].m_upgradeTiers[currentTier].ToString();
            indMultBoostText.text = "50% " + crew[indMultInt].m_name + " GPS Boost";
            if(currentTier+1 < numMults)
                indMultNextLevelText.text = "Next Tier: Level " + crew[indMultInt].m_upgradeTiers[currentTier+1].ToString();
            else
                indMultNextLevelText.text = "Next Tier: None";
        }
        else
        {
            indMultButtonText.text = "MAXED";
            indMultTotalBoostText.text = "Current " + crew[indMultInt].m_name + " Boost: " + DisplayNumber((crew[indMultInt].m_indMult - 1) * 100) + "%";
            indMultLevelText.text = "Level " + crew[indMultInt].m_upgradeTiers[currentTier-1].ToString();
            indMultBoostText.text = "Maxed";
            indMultNextLevelText.text = "Next Tier: None";
        }
    }

    private void ColorMults()
    {
        // for(int i = 0; i < numMults; i++)
        //     if(!crew[indMultInt].m_tiersBought[i] && crew[indMultInt].m_level >= crew[indMultInt].m_upgradeTiers[i] && crew[indMultInt].m_upgradeTierCosts[i] <= numGold)
        //         multButton[i].interactable = true;
        //     else
        //         multButton[i].interactable = false;
        currentTier = crew[indMultInt].m_currentTier;
        if(currentTier < numMults && crew[indMultInt].m_level >= crew[indMultInt].m_upgradeTiers[currentTier] && crew[indMultInt].m_upgradeTierCosts[currentTier] <= numGold)
            indMultButton.interactable = true;
        else
            indMultButton.interactable = false;
    }

    // n in index of mult 0 at top 1 next down...
    public void BuyMult()
    {
        crew[indMultInt].m_currentTier++;
        // crew[indMultInt].m_indMult += 2;
        // if(crew[indMultInt].m_indMult == 3)
        //     crew[indMultInt].m_indMult = 2;
        crew[indMultInt].InitText();
        ColorMults();
        UpdateIndMultText();
        doUpdateCrewText = true;
    }

    public GameObject[] muteAlert = new GameObject[numCrew];
    private bool doAlertMult;
    private void CheckMultAlert()
    {
        doAlertMult = false;
        for(int i = 0; i < numCrew; i++)
        {
            currentTier = crew[i].m_currentTier;
            if(crew[i].m_upgradeTiers[currentTier] <= crew[i].m_level && crew[i].m_upgradeTierCosts[currentTier] <= numGold)
            {
                muteAlert[i].SetActive(true);
                doAlertMult = true;
            }
            else
                muteAlert[i].SetActive(false);
        }
    }
}
