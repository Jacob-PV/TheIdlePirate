using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SHIP
public partial class Basics : MonoBehaviour
{
    // VARS
    const int numShip = 1;
    public Multiplier[] ship = {
        new Multiplier("Sail",2,0,100),
    };

    public Text[] shipHeaderText = new Text[numShip];
    public Text[] shipUpgradeText = new Text[numShip];
    public Button[] shipUpgradeButton = new Button[numShip];

    // METHODS
    private void InitShipText()
    {
        foreach(Multiplier i in ship)
        {
            i.InitText();
        }
    }
    
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
        if(doUpdateShipText)
            clickSound.Play();
    }

    public void getMultSec()
    {
        multPerSec = 1;
        for(int i = 0; i < numShip; i++)
        {
            multPerSec *= ship[i].m_currentMultiple;
        }
    }

    private void SaveShip(bool prestige = false)
    {
        foreach(Multiplier i in ship)
        {
            PlayerPrefs.SetInt(i.m_name + ".m_level", i.m_level);
        } 

        if(prestige)
        {
            foreach(Multiplier i in ship)
                PlayerPrefs.SetInt(i.m_name + ".m_level", 0);
        }
    }

    // button
    private void shipButtonColor()
    {
        for(int i = 0; i < numShip; i++)
        {
            if(ship[i].m_upgradeCost <= numGold)
                shipUpgradeButton[i].interactable = true;
            else
                shipUpgradeButton[i].interactable = false;
        }
    }
}
