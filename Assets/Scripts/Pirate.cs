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
    public double m_indMult;
    public int[] m_upgradeTiers;
    public double[] m_upgradeTierCosts;
    public bool[] m_tiersBought;
    public int m_currentTier;

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
        m_indMult = 1;
        m_currentTier = 0;
        m_upgradeTiers = new int[]{25,50,75,100,200,300,400};
        m_upgradeTierCosts = new double[]{
            2 * m_costBase * Math.Pow(m_costCoefficient,25),
            2 * m_costBase * Math.Pow(m_costCoefficient,50),
            2 * m_costBase * Math.Pow(m_costCoefficient,75),
            2 * m_costBase * Math.Pow(m_costCoefficient,100),
            2 * m_costBase * Math.Pow(m_costCoefficient,200),
            2 * m_costBase * Math.Pow(m_costCoefficient,300),
            2 * m_costBase * Math.Pow(m_costCoefficient,400),
            };
        m_tiersBought = new bool[]{false,false,false,false,false,false,false};
        // m_nextTier = 25;
    }

    ~Pirate(){}

    // private void IndMultHandler()
    // {
    //     int tmpLevel = m_level;

    //     while(tmpLevel > 0)
    //     {
    //         if(tmpLevel - m_nextTier >= 0)
    //         {
    //             m_indMult *= 1.5;
    //             tmpLevel -= m_nextTier;
    //             m_nextTier *= 2;
    //         }
    //         else
    //             tmpLevel -= m_nextTier;
    //     }

    //     m_clickPower = m_level * m_clickBase * m_indMult;
    // }

    public double Upgrade(double gold)
    {
        double cost = 0;
        m_didUpdate = false;
        if(gold >= m_upgradeCost)
        {
            m_didUpdate = true;
            m_level++;
            cost = m_upgradeCost;
            m_clickPower = m_level * m_clickBase * m_indMult;
            // suggested 1.07 to 1.17
            // upgradeBase * (coefficent ^ numOwned)
            m_upgradeCost = m_costBase * Math.Pow(m_costCoefficient,m_level);
            m_upgradeCost = Math.Round((float)m_upgradeCost);
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
            "Spd","Ocd","Nod","Vg","Uvg","Dvg","aa","ab","ac","ad","ae","af","ag","ah","ai","aj","ak","al","am","an","ao",
            "ap","aq","ar","as","at","au","av","aw","ax","ay","az","ba","bb","bc","bd","be","bf","bg","bh","bi","bj","bk",
            "bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz","ca","cb","cc","cd","ce","cf","cg",
            "ch","ci","cj","ck","cl","cm","cn","co","cp","cq","cr","cs","ct","cu","cv","cw","cx","cy","cz","da","db","dc",
            "dd","de","df","dg","dh","di","dj","dk","dl","dm","dn","do","dp","dq","dr","ds","dt","du","dv","dw","dx","dy"};
        
        int suffixIndex = 0;
        for(int i = 6; i <= suffix.Length+6; i=i+3)
        {
            if(number/Math.Pow(10,i)<1000 && number/Math.Pow(10,i)>=1)
            {
                return (number / Math.Pow(10,i)).ToString(decimals) + " " + suffix[suffixIndex];
            }
            if(number/Math.Pow(9,106) > 1)
                return(number.ToString("0.000E0"));
            suffixIndex++;
        }
        return number.ToString("f0");
    }

    public void UpdateText()
    {
        m_headerText = m_name + " (" + m_clickBase + " GPS)\n" + DisplayNumber(m_clickPower) + " Gold/Sec\n" + "Level " + m_level; 
        m_upgradeText = "Upgrade\nCost:\n" + DisplayNumber(m_upgradeCost);
        if(m_name == "Shovel")
            m_headerText = m_name + "\n" + DisplayNumber(m_clickPower) + " Gold/Click\nLevel " + m_level;
    }

    public void InitText()
    {
        // cost
        m_upgradeCost = m_costBase * Math.Pow(m_costCoefficient,m_level);
        m_upgradeCost = Math.Round((float)m_upgradeCost);
        // click power
        m_indMult = 1 + m_currentTier * .5;
        // foreach(bool i in m_tiersBought)
        // {
        //     // if(i)
        //     //     m_indMult += 2;
        //     // if(m_indMult == 3)
        //     //     m_indMult = 2;
        // }

        m_clickPower = m_level * m_clickBase * m_indMult;
        // IndMultHandler();
        UpdateText();
    }
}
