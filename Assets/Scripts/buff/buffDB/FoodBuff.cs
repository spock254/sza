﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBuff : IBuff
{
    public void Buff()
    {
        Debug.Log("Food buff");
    }

    public void Debuff()
    {
        Debug.Log("Food debuff");
    }
}
