using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpeedDebuff : IBuff
{
    static float playerOriginMovement = 0;
    static PlayerMovement playerMovement;
    public void Buff()
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        playerOriginMovement = playerMovement.speed;
        playerMovement.speed = 1f;
    }

    public void Debuff()
    {
        playerMovement.speed = playerOriginMovement;

        Debug.Log(playerMovement.speed);
    }
}
