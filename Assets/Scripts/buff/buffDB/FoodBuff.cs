using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBuff : IBuff
{
    static float playerOriginMovement = 0;
    static PlayerMovement playerMovement;
    public void Buff()
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        playerOriginMovement = Global.Buff.Player.SPEED;
        playerMovement.speed = 1.6f;
    }

    public void Debuff()
    {
        playerMovement.speed = playerOriginMovement;

        Debug.Log(playerMovement.speed);
    }
}
