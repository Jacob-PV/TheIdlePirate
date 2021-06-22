using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// CREW
public partial class Basics : MonoBehaviour
{
    // VARS
    const int numCrew = 11;
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
        } 

        // achievments
        PlayerPrefs.SetInt("totalCrewUpgrades", totalCrewUpgrades);
        PlayerPrefs.SetInt("totalShovelClicks", totalShovelClicks);

        if(prestige)
        {
            foreach(Pirate i in crew)
                PlayerPrefs.SetInt(i.m_name + ".m_level", 0);
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
}
