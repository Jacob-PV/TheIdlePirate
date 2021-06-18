using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ACHIEVMENTS
public partial class Basics : MonoBehaviour
{
    // VARS
    bool doUpdateAchText = true;

    const int numAchievements = 4;
    public Achievement[] achievements = {
        new Achievement("Crew Upgrades", new double[] {10,50,100,1000},new int[] {5,10,20,40}),
        new Achievement("Total Gold", new double[] {1000,1000000,1000000000,1000000000000}, new int[] {5, 10, 20, 40}),
        new Achievement("Total Shovel Clicks", new double[] {10,100,1000,10000}, new int[] {5,10,20,40}),
        new Achievement("Gold Per Second", new double[] {1000,1000000,1000000000,1000000000000,1000000000000000,1000000000000000000}, new int[] {5,10,20,40,80,160}),
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
        UpdateCurrent();
        
        for(int i = 0; i < numAchievements; i++)
        {
            // text
            if(!achievements[i].m_maxed)
                achievmentsHeader[i].text = achievements[i].m_headerText + DisplayNumber(current[i]) + "/" + DisplayNumber(achievements[i].m_tiers[achievements[i].m_level-1]);
            else
                achievmentsHeader[i].text = achievements[i].m_headerText + DisplayNumber(current[i]);
            achievmentsClaim[i].text = achievements[i].m_rewardText;
            // button
            if(!achievements[i].m_maxed && current[i] >= achievements[i].m_tiers[achievements[i].m_level-1])
                claimButton[i].interactable = true;
            else
                claimButton[i].interactable = false;
        }
    }

    private void UpdateCurrent()
    {
        current[0] = totalCrewUpgrades;
        current[1] = totalGold;
        current[2] = totalShovelClicks;
        current[3] = goldPerSec * Time.deltaTime;
    }

    public void Claim(int i)
    {
        if(claimButton[i].interactable == true)
        {
            numRubies += achievements[i].m_rewardTiers[achievements[i].m_level-1];
            achievements[i].m_level++;
            achievements[i].UpdateText();
            SaveAch();
            ClickSound();
        }
    }

    private void InitAchText()
    {
        foreach(Achievement i in achievements)
            i.UpdateText();
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
