using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PERMANET
public partial class Basics : MonoBehaviour
{
    // VARS
    public double permMultPerSec;
    private bool doUpdatePermText;
    const int numPerm = 1;
    public Multiplier[] perm = {
        new Multiplier("Gold Sail",3,0,3,true),
    };

    public Text[] permHeaderText = new Text[numPerm];
    public Text[] permUpgradeText = new Text[numPerm];
    public Button[] permUpgradeButton = new Button[numShip];

    // METHODS
    private void InitPermText()
    {
        foreach(Multiplier i in perm)
        {
            i.InitText();
        }
    }
    
    private void UpdatePermText()
    {
        for(int i = 0; i < numPerm; i++)
        {
            permHeaderText[i].text = perm[i].m_headerText;
            permUpgradeText[i].text = perm[i].m_upgradeText;
        }
    }

    public void UpgradePerm(int i)
    {
        numRubies -= (int)perm[i].Upgrade(numRubies);
        doUpdatePermText = perm[i].m_didUpdate;
    }

    public void getpermMultSec()
    {
        permMultPerSec = 1;
        for(int i = 0; i < numPerm; i++)
        {
            permMultPerSec *= perm[i].m_currentMultiple;
        }
    }

    private void SavePerm()
    {
        foreach(Multiplier i in perm)
        {
            PlayerPrefs.SetInt(i.m_name + ".m_level", i.m_level);
        } 
        PlayerPrefs.SetString("permMultPerSec", permMultPerSec.ToString());
    }

    // button
    private void permColorButton()
    {
        for(int i = 0; i < numPerm; i++)
        {
            if(perm[i].m_upgradeCost <= numRubies)
                permUpgradeButton[i].interactable = true;
            else
                permUpgradeButton[i].interactable = false;
        }
    }
}
