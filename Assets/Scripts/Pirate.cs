using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public Pirate(string name, int level = 0, double clickPower = 0, double costBase = 1, double clickBase = 1, float costCoefficient = 1.15f)
    {
        m_name = name;
        m_level = level;
        m_clickPower = clickPower;
        m_upgradeCost = costBase;
        m_costBase = costBase;
        m_clickBase = clickBase;
        m_costCoefficient = costCoefficient;
        UpdateText();
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
            m_upgradeCost = m_costBase * Mathf.Pow(m_costCoefficient,m_level);
            m_upgradeCost = Mathf.Round((float)m_upgradeCost);
            UpdateText();
        }
        return cost;
    }

    // display numbers
    private string DisplayNumber(double number, string decimals = "f3")
    {
        // string[] suffix = new string[] {"Million","Billion","Trillion","Quadrillion","Quintillion","Sextillion","Septillion","Octillion","Nonillion","Decillion",
        //     "Undecillion","Duodecillion","Tredecillion","Quattuordecillion","Quindecillion","Sexdecillion","Septendecillion","Octodecillion","Novemdecillion","Vigintillion"};
        string[] suffix = new string[] {"M","B","T","Qa","Qi","Sx","Sp","Oc","No","Dc","Ud","Dd","Td","Qad","Qid","Sxd",
            "Spd","Ocd","Nod","Vg","Uvg","Dvg"};
        
        int suffixIndex = 0;
        for(int i = 6; i <= suffix.Length+6; i=i+3)
        {
            if(number/Math.Pow(10,i)<1000 && number/Math.Pow(10,i)>=1)
            {
                return (number / Math.Pow(10,i)).ToString(decimals) + " " + suffix[suffixIndex];
            }
            suffixIndex++;
        }
        return number.ToString("f0");
    }

    public void UpdateText()
    {
        m_headerText = m_name + " (" + m_clickBase + " GPS)\n" + DisplayNumber(m_clickPower) + " Gold/Sec\nLevel " + m_level; 
        m_upgradeText = "Upgrade\nCost:\n" + DisplayNumber(m_upgradeCost);
        if(m_name == "Shovel")
            m_headerText = m_name + "\n" + DisplayNumber(m_clickPower) + " Gold/Click\nLevel " + m_level;
    }

    public void InitText()
    {
        // cost
        m_upgradeCost = m_costBase * Mathf.Pow(m_costCoefficient,m_level);
        m_upgradeCost = Mathf.Round((float)m_upgradeCost);
        // click power
        m_clickPower = m_level * m_clickBase;
        UpdateText();
    }
}
