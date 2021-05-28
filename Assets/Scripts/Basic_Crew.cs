using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// CREW
public partial class Basics : MonoBehaviour
{
    // VARS
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
    }

    // upgrade funcitons
    public void Upgrade(int i)
    {
        numGold -= crew[i].Upgrade(numGold);
        doUpdateCrewText = crew[i].m_didUpdate;
    }

    private void UpdateCrewText()
    {
        foreach(Pirate i in crew)
            i.UpdateText();
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
}
