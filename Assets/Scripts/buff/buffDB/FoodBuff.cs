using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBuff : IBuff
{
    static float playerOriginMovement = 0;
    static PlayerMovement playerMovement;

    IBuff rebuff = null;
    public void Buff()
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        playerOriginMovement = Global.Buff.Player.SPEED;
        playerMovement.speed = 1.6f;
    }

    public void Debuff()
    {
        playerMovement.speed = playerOriginMovement;

        if (this.rebuff != null) 
        {
            this.rebuff.Buff();
            this.rebuff = null;
        }

        //Debug.Log(playerMovement.speed);
    }

    public void SetRebuff(IBuff rebuff)
    {
        this.rebuff = rebuff;
    }
}
