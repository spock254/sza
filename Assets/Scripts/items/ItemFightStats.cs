using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemFightStats
{
    private float attack;
    private float defence;

    public ItemFightStats(float attack, float defence)
    {
        Attack = attack;
        Defence = defence;
    }

    public float Attack { get => attack; set => attack = value; }
    public float Defence { get => defence; set => defence = value; }
}
