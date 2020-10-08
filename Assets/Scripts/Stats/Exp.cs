using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp
{
    private int current_exp;
    private int lvl;

    public readonly int[] lvllt;
   // public readonly int MAX_LVL = 100;

    public int Current_exp { get => current_exp; set => current_exp = value; }
    public int Lvl { get => lvl; set => lvl = value; }

    public Exp(int exp, int lvl)
    {
        this.current_exp = exp;
        this.lvl = lvl;

        lvllt = FillLVLLT(Global.MAX_LVL);
    }

    public Exp(int lvl)
    {
        this.lvl = lvl;
        lvllt = FillLVLLT(Global.MAX_LVL);
        this.current_exp = lvllt[lvl];
    }

    public Exp()
    {
        this.lvl = 0;
        this.current_exp = 0;

        lvllt = FillLVLLT(Global.MAX_LVL);
    }

    private int[] FillLVLLT(int max_lvl) 
    {
        int[] lt = new int[max_lvl];
        for (int i = 0; i < max_lvl; i++)
        {
            lt[i] = i * 2000;
        }

        return lt;
    }

    public int NextExpAmount() 
    {
        return lvllt[lvl + 1];
    }

    public bool isLvlUp() 
    {
        return current_exp == lvllt[lvl + 1];
    }
}
