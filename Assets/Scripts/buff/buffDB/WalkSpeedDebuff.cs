﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpeedDebuff : IBuff
{
    static float playerOriginMovement = 0;
    static PlayerMovement playerMovement;
    public void Buff()
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        playerOriginMovement = Global.Buff.Player.SPEED;
        playerMovement.speed = 1f;
    }

    public void Debuff(IBuff buff)
    {
        playerMovement.speed = playerOriginMovement;

        if (buff != null) 
        {
            buff.Buff();
        }

        Debug.Log(playerMovement.speed);
    }
}
