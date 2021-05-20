using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate
{
    public string m_name;
    public string m_headerText;
    public string m_upgradeText;
    public int m_level;
    public double m_clickPower;
    public double m_upgradeCost;
    private double m_costBase;
    private double m_clickBase;
    private float m_costCoefficient;
    public bool m_didUpdate;

    public Pirate(string name, int level = 0, double clickPower = 0, double costBase = 1, double clickBase = 1, float costCoefficient = 1.5f)
    {
        m_name = name;
        m_level = level;
        m_clickPower = clickPower;
        m_upgradeCost = costBase;
        m_costBase = costBase;
        m_clickBase = clickBase;
        m_costCoefficient = costCoefficient;
        m_headerText = m_name + ": " + m_clickPower + " Gold/Sec\nLevel " + m_level; 
        m_upgradeText = "Upgrade\nCost:\n" + DisplayNumber(m_upgradeCost);
        m_didUpdate = true;
    }

    ~Pirate(){}

    public double Upgrade(double gold)
    {
        double cost = 0;
        m_didUpdate = false;
        if(gold >= m_upgradeCost)
        {
            m_didUpdate = true;
            m_level++;
            cost = m_upgradeCost;
            m_clickPower = m_level * m_clickBase;
            // suggested 1.07 to 1.17
            // upgradeBase * (coefficent ^ numOwned)
            m_upgradeCost = m_costBase * Mathf.Pow(m_level+1,m_costCoefficient);
            m_upgradeCost = Mathf.Round((float)m_upgradeCost);
            m_headerText = m_name + ": " + m_clickPower + " Gold/Sec\nLevel " + m_level; 
            m_upgradeText = "Upgrade\nCost:\n" + m_upgradeCost;
        }
        return cost;
    }

    // display numbers
    private string DisplayNumber(double number, string decimals = "f3")
    {
        if(number >= 1000000000)
            return (number / 1000000000).ToString(decimals) + "B";
        else if(number >= 1000000)
            return (number / 1000000).ToString(decimals) + "M";
        else
            return number.ToString("f0");
    }

    public void UpdateText()
    {
        m_headerText = m_name + ": " + m_clickPower + " Gold/Sec\nLevel " + m_level; 
        m_upgradeText = "Upgrade\nCost:\n" + m_upgradeCost;
    }
}
