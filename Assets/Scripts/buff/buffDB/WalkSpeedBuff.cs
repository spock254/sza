using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpeedBuff : IBuff
{
    static float playerOriginMovement = 0;
    static PlayerMovement playerMovement;
    static BuffController buffController;
    static IBuff rebuff = null;
    public void Buff()
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        playerOriginMovement = Global.Buff.Player.SPEED;
        playerMovement.speed = 1.6f;
    }

    public void Debuff()
    {
        //Debug.Log(WalkSpeedBuff.rebuff != null);
        //Debug.Log("-------------- WalkSpeedBuff");
        playerMovement.speed = playerOriginMovement;

        if (WalkSpeedBuff.rebuff != null)
        {
            if (buffController == null)
            {
                buffController = Global.Component.GetBuffController();
            }

            if (buffController.IsBuffExistByTypeName(BuffType.WalkSpeedDebuff, false) == false)
            {
                WalkSpeedBuff.rebuff.Buff();
                WalkSpeedBuff.rebuff = null;
            }
        }
    }

    public bool IsBuffed()
    {
        return playerMovement.speed == 1.6f;
    }

    public void SetRebuff(IBuff rebuff)
    {
        WalkSpeedBuff.rebuff = rebuff;
    }
}
