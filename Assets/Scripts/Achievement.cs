using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Achievement
{
    // vars
    public string m_name;
    public int m_level;
    public double[] m_tiers;
    public int[] m_rewardTiers;
    public string m_headerText;
    public string m_rewardText;
    public bool m_maxed;

    public Achievement(string name, double[] tiers, int[] rewardTiers, int level = 1)
    {
        m_name = name;
        m_tiers = tiers;
        m_rewardTiers = rewardTiers;
        m_level = level;
        UpdateText();
    }

    public void UpdateText()
    {
        m_maxed = (m_level > m_tiers.Length)?true:false;
        if(!m_maxed)
        {
            m_headerText = "Level: " + m_level + "/" + m_tiers.Length + "\n" + m_name + "\n";
            m_rewardText = m_rewardTiers[m_level-1].ToString() + " Rubies\nClaim";
        }
        else
        {
            m_headerText = "Level: MAXED\n" + m_name + " ";
            m_rewardText = "MAXED";
        }
    }
}
