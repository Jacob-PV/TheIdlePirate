using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SHIP
public partial class Basics : MonoBehaviour
{
    // VARS
    const int numShip = 4;
    public Multiplier[] ship = {
        new Multiplier("Sails",0.01,0,1000),
        new Multiplier("Rudder",0.03,0,1000000),
        new Multiplier("Crows Nest",0.05,0,1000000000),
        new Multiplier("Cannons",0.07,0,1000000000000),
    };

    public Text[] shipHeaderText = new Text[numShip];
    public Text[] shipUpgradeText = new Text[numShip];
    public Button[] shipUpgradeButton = new Button[numShip];

    // METHODS

    // initilize internal text of the ship
    private void InitShipText()
    {
        foreach(Multiplier i in ship)
        {
            i.InitText();
        }
    }
    
    // update external text for ship
    private void UpdateShipText()
    {
        for(int i = 0; i < numShip; i++)
        {
            shipHeaderText[i].text = ship[i].m_headerText;
            shipUpgradeText[i].text = ship[i].m_upgradeText;
        }
    }

    // handle upgrading ship item i
    public void UpgradeShip(int i)
    {
        numGold -= ship[i].Upgrade(numGold);
        // doUpdateShipText = ship[i].m_didUpdate;
        if(ship[i].m_didUpdate)
        {
            DoUpdateShipText();
            ClickSound();
        }
    }

    // cacluate total multipliers from ship items
    public void getMultSec()
    {
        multPerSec = 1;
        for(int i = 0; i < numShip; i++)
        {
            multPerSec *= ship[i].m_currentMultiple;
        }
    }

    // save ship data, different if after prestige 
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

    // set each ship item button availability
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
