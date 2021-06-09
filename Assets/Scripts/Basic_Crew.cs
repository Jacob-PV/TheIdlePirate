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
        new Pirate("Shovel",1,1,10,1),
        new Pirate("Swashbuckler",0,0,100,2),
        new Pirate("Cannon Fondler",0,0,1000,4),
        new Pirate("Scout",0,0,10000,8),
        new Pirate("Hunter",0,0,100000,16),
        new Pirate("Blunderbuss",0,0,1000000,32),
        new Pirate("Soldier",0,0,10000000,64),
        new Pirate("Sword Master",0,0,100000000,128),
        new Pirate("Sharpshooter", 0,0,1000000000,256),
        new Pirate("Heavy Gunner",0,0,10000000000,512),
        new Pirate("Captain", 0,0,100000000000,1024),
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
        numGold += crew[0].m_clickPower;
        totalGold += crew[0].m_clickPower;
    }

    // upgrade funcitons
    public void Upgrade(int i)
    {
        numGold -= crew[i].Upgrade(numGold);
        doUpdateCrewText = crew[i].m_didUpdate;
        if(doUpdateCrewText)
            clickSound.Play();
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
