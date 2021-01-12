using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpeedBuff : IBuff
{
    static float playerOriginMovement = 0;
    static PlayerMovement playerMovement;

    static IBuff rebuff = null;
    public void Buff()
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        playerOriginMovement = Global.Buff.Player.SPEED;
        playerMovement.speed = 1.6f;
    }

    public void Debuff()
    {
        playerMovement.speed = playerOriginMovement;

        if (rebuff != null) 
        {
            rebuff.Buff();
            rebuff = null;
        }

        //Debug.Log(playerMovement.speed);
    }

    public void SetRebuff(IBuff rebuff)
    {
        WalkSpeedBuff.rebuff = rebuff;
    }
}
