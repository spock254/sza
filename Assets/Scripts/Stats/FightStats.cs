using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightStats
{
    private float attack;
    private float defence;

    public int[] attackCofWithLvl;
    public int[] defenceCofWithLvl;
    public FightStats(int attack, int defence)
    {
        this.attack = attack;
        this.defence = defence;

        attackCofWithLvl = FillCof();
        defenceCofWithLvl = FillCof();
    }

    public float Attack { get => attack; set => attack = value; }
    public float Defence { get => defence; set => defence = value; }

    private int[] FillCof() 
    {
        int[] cof = new int[Global.MAX_LVL];

        for (int i = 0; i < Global.MAX_LVL; i++)
        {
            cof[i] = i;
        }

        return cof;
    }
}
