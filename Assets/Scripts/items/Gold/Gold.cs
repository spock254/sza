using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold 
{
    private int goldAmount;

    public Gold(int goldAmount)
    {
        this.goldAmount = goldAmount;
    }

    public int GoldAmount { get => goldAmount; set => goldAmount = value; }
}
