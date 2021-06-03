using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PRESTIGE
public partial class Basics : MonoBehaviour
{
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

        doUpdateShipText = true;
        doUpdateCrewText = true;

        // add keys
        keys += claimableKeys;
        claimableKeys = 0;
    }
}
