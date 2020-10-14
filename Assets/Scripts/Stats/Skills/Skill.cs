using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ISkill
{
    const int MAX_LVL = 10;

    int lvl = 0;
    float exp = 0;

    int[] lvl_lut;

    public Skill() 
    {
        lvl_lut = new int[MAX_LVL];

        for (int i = 0; i < lvl_lut.Length; i++)
        {
            int j = i + 1;
            lvl_lut[i] = j * j * 100;
        }
    }

    public void AddExp(float exp)
    {
        this.exp += exp;

        while (lvl < MAX_LVL - 1 && this.exp > lvl_lut[lvl]) 
        {
            lvl++;
        }

    }

    public CraftType GetCraftType()
    {
        return CraftType.Cooking;
    }

    public float GetSkillExp()
    {
        return exp;
    }

    public int GetSkillLvl()
    {
        return lvl;
    }

    public float ExpToNextLvl() 
    {
        return (lvl + 1 < MAX_LVL - 1) ? lvl_lut[lvl + 1] : lvl_lut[MAX_LVL - 1];
    }

    public float GetMinCurrentLvlExp() 
    {
        return lvl_lut[lvl];
    }
}
