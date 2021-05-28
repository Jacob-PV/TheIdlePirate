using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ACHIEVMENTS
public partial class Basics : MonoBehaviour
{
    // VARS
    bool doUpdateAchText = true;

    const int numAchievements = 2;
    public Achievement[] achievements = {
        new Achievement("Crew Upgrades", new double[] {10,50,100},new int[] {5,10,20}),
        new Achievement("Total Gold", new double[] {1000,10000,100000,1000000}, new int[] {5, 10, 20, 40}),
    };
    public Text[] achievmentsHeader = new Text[numAchievements];
    public Text[] achievmentsClaim = new Text[numAchievements];
    public Button[] claimButton = new Button[numAchievements];

    private double[] current = new double[numAchievements];

    // METHODS
    public void UpdateAchievementsText()
    {
        // update internal text
        if(doUpdateAchText)
        {
            foreach(Achievement i in achievements)
                i.UpdateText();
            doUpdateAchText = false;
        }

        // update external text
        current[0] = totalCrewUpgrades;
        current[1] = totalGold;
        for(int i = 0; i < numAchievements; i++)
        {
            // text
            achievmentsHeader[i].text = achievements[i].m_headerText + current[i] + "/" + achievements[i].m_tiers[achievements[i].m_level-1];
            achievmentsClaim[i].text = achievements[i].m_rewardText;
            // button
            if(current[i] >= achievements[i].m_tiers[achievements[i].m_level-1])
                claimButton[i].interactable = true;
            else
                claimButton[i].interactable = false;
        }
    }

    public void Claim(int i)
    {
        if(claimButton[i].interactable == true)
        {
            numRubies += achievements[i].m_rewardTiers[achievements[i].m_level-1];
            achievements[i].m_level++;
            doUpdateAchText = true;
            SaveAch();
        }
    }

    private void SaveAch()
    {
        foreach(Achievement i in achievements)
        {
            PlayerPrefs.SetInt(i.m_name + ".m_level", i.m_level);
        } 

        PlayerPrefs.SetInt("numRubies", numRubies);
    }
}
