using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private bool m_isRuby;

    public Multiplier(string name, double multiple, int level, double costBase, bool ruby = false)
    {
        m_name = name;
        m_multipleInterval = multiple;
        m_level = level;
        m_upgradeCost = costBase;
        m_costBase = costBase;
        m_currentMultiple = m_level * m_multipleInterval;
        UpdateText();
        m_didUpdate = true;
        m_isRuby = ruby;
    }

    ~Multiplier(){}

    public double Upgrade(double numCoin)
    {
        double cost = 0;
        m_didUpdate = false;

        // gold
        if(numCoin >= m_upgradeCost)
        {
            m_level++;
            cost = m_upgradeCost;
            m_currentMultiple = m_level * m_multipleInterval + 1;
            if(!m_isRuby)
                m_upgradeCost = m_costBase * Math.Pow(2, m_level);
            else 
            { 
                if(m_level == 0)
                    m_upgradeCost = m_costBase;
                else
                    m_upgradeCost = Math.Round(m_costBase * 1.15 * m_level);
            } 
            UpdateText();
            m_didUpdate = true;
        }

        // ruby
        // if(m_isRuby && numCoin >= m_upgradeCost)
        // {
        //     m_level++;
        //     cost = m_upgradeCost;
        //     m_currentMultiple = m_level * m_multipleInterval;
        //     m_upgradeCost = Math.Round(m_costBase * Math.Pow(1.15, m_level));  
        //     UpdateText();
        //     m_didUpdate = true;
        // }
        return cost;
    }

    public void UpdateText()
    {
        m_headerText = m_name + " (" + (m_multipleInterval * 100) + "%)\nGPS Boost: " + ((m_currentMultiple - 1) * 100).ToString("f0") + "%\nLevel " + m_level;
        if(!m_isRuby)
            m_upgradeText = "Upgrade\nCost:\n" + DisplayNumber(m_upgradeCost);
        else
            m_upgradeText = "Upgrade\nCost:\n" + DisplayNumber(m_upgradeCost) + " Rubies";
    }

    public void InitText()
    {
        m_currentMultiple = m_level * m_multipleInterval + 1;
        if(m_currentMultiple == 0)
            m_currentMultiple = 1;
        if(!m_isRuby)
            m_upgradeCost = m_costBase * Math.Pow(2, m_level);
        else 
        { 
            if(m_level == 0)
                m_upgradeCost = m_costBase;
            else
                m_upgradeCost = Math.Round(m_costBase * 1.15 * m_level);
        }  
        UpdateText();
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
            suffixIndex++;
        }
        return number.ToString("f0");
    }
}
