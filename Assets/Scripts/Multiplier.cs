using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier
{
    public string m_name;
    public string m_headerText;
    public string m_upgradeText;
    public double m_multipleInterval;
    public int m_level;
    public double m_currentMultiple;
    public double m_upgradeCost;
    public double m_costBase;
    public bool m_didUpdate;

    public Multiplier(string name, double multiple, int level, double costBase)
    {
        m_name = name;
        m_multipleInterval = multiple;
        m_level = level;
        m_upgradeCost = costBase;
        m_costBase = costBase;
        m_currentMultiple = m_level * m_multipleInterval;
        UpdateText();
        m_didUpdate = true;
    }

    ~Multiplier(){}

    public double Upgrade(double numGold)
    {
        double cost = 0;
        m_didUpdate = false;
        if(numGold >= m_upgradeCost)
        {
            m_level++;
            cost = m_upgradeCost;
            m_currentMultiple = m_level * m_multipleInterval;
            m_upgradeCost = m_costBase * Mathf.Pow(m_level+1, 2);
            UpdateText();
            m_didUpdate = true;
        }
        return cost;
    }

    public void UpdateText()
    {
        m_headerText = m_name + ": " + m_currentMultiple + "x\nLevel " + m_level;
        m_upgradeText = "Upgrade\nCost:\n" + m_upgradeCost;
    }

    public void InitText()
    {
        m_currentMultiple = m_level * m_multipleInterval;
        if(m_currentMultiple == 0)
            m_currentMultiple = 1;
        m_upgradeCost = m_costBase * Mathf.Pow(m_level+1, 2);
        UpdateText();
    }
}
