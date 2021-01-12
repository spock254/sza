using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpeedDebuff : IBuff
{
    static float playerOriginMovement = 0;
    static PlayerMovement playerMovement;

    static IBuff rebuff = null;
    public void Buff()
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        playerOriginMovement = Global.Buff.Player.SPEED;
        playerMovement.speed = 1f;

    }

    public void Debuff()
    {
        Debug.Log(WalkSpeedDebuff.rebuff != null);
        playerMovement.speed = playerOriginMovement;

        if (WalkSpeedDebuff.rebuff != null) 
        {
            WalkSpeedDebuff.rebuff.Buff();
            WalkSpeedDebuff.rebuff = null;
        }

        //Debug.Log(playerMovement.speed);
    }

    public void SetRebuff(IBuff rebuff)
    {
        WalkSpeedDebuff.rebuff = rebuff;
    }
}
