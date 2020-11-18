using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_waitTillPlayerReachPos : BaseState<NPC_DATA_waitTillPlayerReachPos>
{
    Transform playerTransform;
    public override void Enter()
    {
        base.Enter();

        playerTransform = Global.Obj.GetPlayerGameObject().GetComponent<Transform>();
    }

    public override void Execute()
    {
        Debug.Log(playerTransform.position.y + " : " + data.point.position.y);
        if (data.IsOnPosition(playerTransform.position)) 
        {
            Debug.Log("GETTTTT");
        }
    }
}
