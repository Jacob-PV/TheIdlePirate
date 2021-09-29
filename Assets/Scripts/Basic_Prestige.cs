using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// PRESTIGE
public partial class Basics : MonoBehaviour
{
    private int timesPrestiged;

    // handle prestiging
    public void Prestige()
    {
        // reset
        didPrestige = true;
        SaveGold();
        SaveCrew(true);
        SaveShip(true);
        SaveAch();
        didPrestige = false;

        Load();

        foreach(Pirate i in crew)
            i.InitText();

        foreach(Multiplier i in ship)
            i.InitText();

        // doUpdateShipText = true;
        DoUpdateShipText();
        // doUpdateCrewText = true;
        DoUpdateCrewText();

        // add keys
        keys += claimableKeys;
        claimableKeys = 0;
        timesPrestiged++;
        PlayerPrefs.SetInt("timesPrestiged",timesPrestiged);
        PlayerPrefs.SetString("keys",keys.ToString());
    }

    private void CalculateKeys()
    {
        // og 10^15
        claimableKeys = Math.Floor(150 * Math.Pow((totalGold/Math.Pow(10,12)),.5) - keys);
    }
}
