using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GREED
public partial class Basics : MonoBehaviour
{
    private float greedPercent;
    public Slider greed;
    public GameObject greedMenu;
    public Text greedText;
    private int[] crewLevels = new int[numCrew];
    private bool areLevels;
    private int takeAwayIndex;
    public Text greeOutputText;
    private int[] levelsRemoved = new int[numCrew];
    private bool didLoseGreed;
    private int crewLostGreed;

    // set the value fo the greed slider and save it
    private void CheckGreedSlider()
    {
        greedPercent = greed.value;
        greedText.text = "Greed Boost: " + greedPercent + "%\nChance to lose crewmate: " + greedPercent + "%";
        PlayerPrefs.SetFloat("greedPercent", greedPercent);
        getGoldSec();
        GetShovelPower();
        UpdateHeaderText();
    }

    // see is each crewmate has a level and if so how many
    private void GreedLevels()
    {
        // make crew Levels array
        for(int i = 0; i < numCrew; i++)
        {
            // dont take away shovels condition
            if(i == 0)
                crewLevels[i] = 0;
            else
            {
                crewLevels[i] = crew[i].m_level;
            }
        }
        // check if there are any levels
        foreach(int i in crewLevels)
        {
            if(i > 0)
            {
                areLevels = true;
                break;
            }
            else
                areLevels = false;
        }
    }

    // calculate greed punishment
    private void GreedPunish()
    {
        idleGold = 0;
        // set all to 0
        for(int i = 0; i < numCrew; i++)
            levelsRemoved[i] = 0;

        didLoseGreed = false;
        greeOutputText.text = "Greed";
        // calculate idle gold and punish
        while(secTimeAway >= 3600)
        {
            GreedLevels();
            if(areLevels && UnityEngine.Random.Range(0,100) < greedPercent)
            {
                while(true)
                {
                    takeAwayIndex = UnityEngine.Random.Range(1,numCrew);
                    if(crewLevels[takeAwayIndex] > 0)
                    {
                        levelsRemoved[takeAwayIndex]++;
                        crew[takeAwayIndex].m_level--;
                        crew[takeAwayIndex].InitText();
                        crewLevels[takeAwayIndex]--;
                        didLoseGreed = true;
                        crewLostGreed++;
                        PlayerPrefs.SetInt("crewLostGreed", crewLostGreed);
                        break;
                    }
                }
                getGoldSec();
                // Debug.Log(goldPerSec);

                idleGold += goldPerSec * 3600;
            }
            secTimeAway -= 3600;
        }
        idleGold += goldPerSec * secTimeAway;
        // output
        if(!didLoseGreed)
            greeOutputText.text += ":\nYou lost no one to greed!";
        else
            greeOutputText.text += " Casualties:";
        for(int i = 0; i < numCrew; i++)
        {
            if(levelsRemoved[i] > 0)
                greeOutputText.text += "\n" + crew[i].m_name + " " + levelsRemoved[i] + "x";
        }
        SaveCrew();
    }
}